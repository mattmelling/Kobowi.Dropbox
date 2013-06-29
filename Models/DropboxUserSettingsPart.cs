using Orchard.ContentManagement;

namespace Kobowi.Dropbox.Models {
    public class DropboxUserSettingsPart : ContentPart<DropboxUserSettingsPartRecord> {
        public string UserToken {
            get { return Record.UserToken; }
            set { Record.UserToken = value; }
        }
        public string UserSecret {
            get { return Record.UserSecret; }
            set { Record.UserSecret = value; }
        }
    }
}