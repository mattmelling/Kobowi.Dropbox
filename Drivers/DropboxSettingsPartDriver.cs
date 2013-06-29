using Kobowi.Dropbox.Models;
using Orchard.ContentManagement.Drivers;

namespace Kobowi.Dropbox.Drivers
{
    public class DropboxSettingsPartDriver : ContentPartDriver<DropboxSettingsPart> {
        protected override string Prefix {
            get { return "DropboxSettingsPart"; }
        }
        protected override DriverResult Editor(DropboxSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_DropboxSettingsPart_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts.DropboxSettingsPart",
                                    Model: part,
                                    Prefix: Prefix));
        }
        protected override DriverResult Editor(DropboxSettingsPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}