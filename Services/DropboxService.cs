using DropNet;
using Kobowi.Dropbox.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;

namespace Kobowi.Dropbox.Services {
    public class DropboxService : IDropboxService {
        private readonly IOrchardServices _orchard;

        public DropboxService(IOrchardServices orchard) {
            _orchard = orchard;
        }
    
        public DropNetClient GetClient() {
            if (_orchard.WorkContext.CurrentUser == null)
                return null;
            var settings = _orchard.WorkContext.CurrentSite.As<DropboxSettingsPart>();
            return new DropNetClient(settings.ApiKey, settings.ApiSecret);
        }

        public DropNetClient GetClient(IUser user) {
            if (user == null) return null;
            var userSettings = user.As<DropboxUserSettingsPart>();
            if (string.IsNullOrEmpty(userSettings.UserToken) 
                || string.IsNullOrEmpty(userSettings.UserSecret))
                return null;
            
            var settings = _orchard.WorkContext.CurrentSite.As<DropboxSettingsPart>();
            return new DropNetClient(settings.ApiKey, settings.ApiSecret,
                                     userSettings.UserToken, userSettings.UserSecret);
        }
    }
}