using Kobowi.Dropbox.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Kobowi.Dropbox.Handlers {
    public class DropboxSettingsPartHandler : ContentHandler {
        public DropboxSettingsPartHandler(IRepository<DropboxSettingsPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<DropboxSettingsPart>("Site"));
        }
    }
}