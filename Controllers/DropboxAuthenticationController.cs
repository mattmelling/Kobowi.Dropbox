using System.Web.Mvc;
using DropNet;
using DropNet.Models;
using Kobowi.Dropbox.Models;
using Kobowi.Dropbox.Services;
using Kobowi.Dropbox.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc;

namespace Kobowi.Dropbox.Controllers {
    public class DropboxAuthenticationController : Controller {
        private readonly IDropboxService _dropbox;
        private readonly IOrchardServices _orchard;
        private readonly IHttpContextAccessor _httpContext;

        public DropboxAuthenticationController(IDropboxService dropbox,
                                               IOrchardServices orchard,
                                               IHttpContextAccessor httpContext) {
            _dropbox = dropbox;
            _orchard = orchard;
            _httpContext = httpContext;
        }

        public ActionResult AuthCallback(string uid, string oauth_token, string redirectUrl) {
            var settings = _orchard.WorkContext.CurrentSite.As<DropboxSettingsPart>();
            var userLogin = _httpContext.Current().Session["DropnetUserLogin"] as UserLogin;

            var client = new DropNetClient(settings.ApiKey, settings.ApiSecret,
                                           userLogin.Token, userLogin.Secret);
            _httpContext.Current().Session["DropnetUserLogin"] = client.GetAccessToken();
            var userSettings = _orchard.WorkContext.CurrentUser.As<DropboxUserSettingsPart>();
            userSettings.UserToken = client.UserLogin.Token;
            userSettings.UserSecret = client.UserLogin.Secret;
            return View();
        }

        public ActionResult Authorise(string redirectUrl) {
            var client = _dropbox.GetClient();
            var url = client.GetTokenAndBuildUrl(
                string.Format("{0}/Kobowi.Dropbox/DropboxAuthentication/AuthCallback",
                              _orchard.WorkContext.CurrentSite.BaseUrl));
            _httpContext.Current().Session["DropnetUserLogin"] = client.UserLogin;
            return View(new DropboxAuthoriseViewModel {
                AuthoriseUrl = url,
                RedirectUrl = redirectUrl
            });
        }
    }
}