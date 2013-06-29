using Orchard.ContentManagement.Records;

namespace Kobowi.Dropbox.Models
{
    public class DropboxSettingsPartRecord : ContentPartRecord {
        public virtual string ApiKey { get; set; }
        public virtual string ApiSecret { get; set; }
    }
}