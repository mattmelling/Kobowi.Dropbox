using System.IO;
using System.Web.Mvc;
using DropNet.Exceptions;
using DropNet.Models;
using Kobowi.Dropbox.Extensions;
using Kobowi.Dropbox.Models;
using Kobowi.Dropbox.Services;
using Kobowi.Dropbox.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.MediaLibrary.Services;

namespace Kobowi.Dropbox.Controllers {
    public class DropboxStorageController : Controller {
        private readonly IDropboxService _dropbox;
        private readonly IOrchardServices _orchard;
        private readonly IMediaLibraryService _media;

        public ILogger Logger { get; set; }

        public DropboxStorageController(IDropboxService dropbox,
                                        IOrchardServices orchard,
                                        IMediaLibraryService media) {
            _dropbox = dropbox;
            _orchard = orchard;
            _media = media;

            Logger = NullLogger.Instance;
        }

        public ActionResult Index(string folderPath) {
            // Check for existence of api keys
            var siteSettings = _orchard.WorkContext.CurrentSite.As<DropboxSettingsPart>();
            if (string.IsNullOrEmpty(siteSettings.ApiKey) || string.IsNullOrEmpty(siteSettings.ApiSecret))
                return View("Index_SetupApi");

            var userSettings = _orchard.WorkContext.CurrentUser.As<DropboxUserSettingsPart>();
            if (string.IsNullOrEmpty(userSettings.UserToken) || string.IsNullOrEmpty(userSettings.UserSecret))
                return RedirectToAction("Authorise", "DropboxAuthentication",
                                        new {
                                            redirectUrl = Url.Action("Index", new {folderPath})
                                        });

            try {
                var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
                var contents = client.GetMetaData("").Contents.ToClientViewModel();
                return View(new DropboxStorageViewModel {
                    FolderPath = folderPath,
                    Contents = contents
                });
            }
            catch (DropboxException dbe) {
                Logger.Error(dbe, "{0}, {1}", dbe.StatusCode, dbe.Response.ErrorMessage);
                return View("Index_SetupApi");
            }
        }

        public ActionResult List(string path) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return new HttpUnauthorizedResult();
            try {
                var contents = client.GetMetaData(path).Contents;
                return Json(contents.ToClientViewModel(), JsonRequestBehavior.AllowGet);
            }
            catch (DropboxException dbe) {
                Logger.Error(dbe, "{0}, {1}", dbe.StatusCode, dbe.Response.ErrorMessage);
                return new HttpNotFoundResult();
            }
        }

        public ActionResult Thumb(string path) {
            return DropboxThumbnail(path, ThumbnailSize.Large);
        }

        public ActionResult Preview(string path) {
            return DropboxThumbnail(path, ThumbnailSize.ExtraLarge);
        }

        [HttpPost]
        public ActionResult Upload(string folderPath, string path, string name) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return new HttpUnauthorizedResult();

            try {
                var file = client.GetFile(path);
                _media.ImportMedia(new MemoryStream(file), folderPath, name);
                return Json(new { status = "ok" });
            }
            catch (DropboxException dbe) {
                Logger.Error(dbe, "{0}, {1}", dbe.StatusCode, dbe.Response.ErrorMessage);
                return new HttpNotFoundResult();
            }
        }

        #region Private stuff

        private static string ImageFileTypeFromPath(string path) {
            var ext = Path.GetExtension(path);
            if (ext == null) return "";
            ext = ext.Remove(0, 1);
            if (ext == "jpg")
                return "image/jpeg";
            return "image/" + ext;
        }

        private ActionResult DropboxThumbnail(string path, ThumbnailSize size) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return new HttpUnauthorizedResult();

            try {
                var thumbnail = client.GetThumbnail(path, size);
                return File(thumbnail, ImageFileTypeFromPath(path));
            }
            catch (DropboxException dbe) {
                Logger.Error(dbe, "{0}, {1}", dbe.StatusCode, dbe.Response.ErrorMessage);
                return new HttpNotFoundResult();
            }
        }

        #endregion
    }
}