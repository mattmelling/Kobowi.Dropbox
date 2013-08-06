using Kobowi.Dropbox.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace Kobowi.Dropbox.Handlers {
    public class DropboxSettingsPartHandler : ContentHandler {
        public Localizer T { get; set; }

        public DropboxSettingsPartHandler(IRepository<DropboxSettingsPartRecord> repository) {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<DropboxSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<DropboxSettingsPart>("DropboxSettings", "Parts.DropboxSettingsPart", "media"));
        }
    }
}