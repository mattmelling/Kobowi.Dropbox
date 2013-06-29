using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DropNet.Models;
using Kobowi.Dropbox.Services;
using Orchard;
using Orchard.MediaLibrary.Services;

namespace Kobowi.Dropbox.Controllers {
    public class DropboxStorageController : Controller {
        private readonly IDropboxService _dropbox;
        private readonly IOrchardServices _orchard;
        private readonly IMediaLibraryService _media;

        public DropboxStorageController(IDropboxService dropbox,
                                        IOrchardServices orchard,
                                        IMediaLibraryService media) {
            _dropbox = dropbox;
            _orchard = orchard;
            _media = media;
        }

        public ActionResult Index(int id) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return RedirectToRoute(
                    new RouteValueDictionary {
                        {"area", "Kobowi.Dropbox"},
                        {"controller", "DropboxAuthentication"},
                        {"action", "Authorise"}, {
                            "redirectUrl", Url.RouteUrl(new RouteValueDictionary {
                                {"area", "Kobowi.Dropbox"},
                                {"controller", "DropboxStorage"},
                                {"action", "Index"},
                                {"id", id}
                            }
                        )}
                    });
            return View(id);
        }

        public ActionResult List(string path) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return new HttpUnauthorizedResult();
            return Json(client.GetMetaData(path).Contents.Select(i => new {
                name = i.Name,
                isFolder = i.Is_Dir,
                icon = i.Icon,
                size = i.Size,
                extension = i.Extension,
                modified = i.Modified,
                hasThumb = i.Thumb_Exists
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Thumb(string path) {
            return File(GetThumbnail(path, ThumbnailSize.ExtraLarge), "image/jpeg");
        }

        public ActionResult Preview(string path) {
            return File(GetThumbnail(path, ThumbnailSize.ExtraLarge), "image/jpeg");
        }

        [HttpPost]
        public ActionResult Upload(int id, string path, string name) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return new HttpUnauthorizedResult();
            var file = client.GetFile(path);
            _media.ImportStream(id, new MemoryStream(file), name);
            return Json(new {
                status = "ok"
            });
        }

        #region Private stuff

        private byte[] GetThumbnail(string path, ThumbnailSize size) {
            var client = _dropbox.GetClient(_orchard.WorkContext.CurrentUser);
            if (client == null)
                return null;
            return client.GetThumbnail(path, size);
        }

        #endregion
    }
}