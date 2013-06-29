using Kobowi.Dropbox.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Kobowi.Dropbox.Handlers {
    public class DropboxUserSettingsPartHandler : ContentHandler {
        public DropboxUserSettingsPartHandler(IRepository<DropboxUserSettingsPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}