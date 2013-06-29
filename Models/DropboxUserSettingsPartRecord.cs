using Orchard.ContentManagement.Records;

namespace Kobowi.Dropbox.Models {
    public class DropboxUserSettingsPartRecord : ContentPartRecord {
        public virtual string UserToken { get; set; }
        public virtual string UserSecret { get; set; }
    }
}