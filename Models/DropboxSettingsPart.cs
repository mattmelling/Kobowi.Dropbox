using Orchard.ContentManagement;

namespace Kobowi.Dropbox.Models {
    public class DropboxSettingsPart : ContentPart<DropboxSettingsPartRecord> {
        public virtual string ApiKey { 
            get { return Record.ApiKey; }
            set { Record.ApiKey = value; }
        }
        public virtual string ApiSecret {
            get { return Record.ApiSecret; }
            set { Record.ApiSecret = value; }
        }
    }
}