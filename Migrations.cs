using Kobowi.Dropbox.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Kobowi.Dropbox {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable(typeof (DropboxSettingsPartRecord).Name,
                                      table => table.ContentPartRecord()
                                                    .Column<string>("ApiKey")
                                                    .Column<string>("ApiSecret"));
            ContentDefinitionManager.AlterPartDefinition(typeof (DropboxSettingsPartRecord).Name,
                                                         part => part.Attachable(false));
            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable(typeof (DropboxUserSettingsPartRecord).Name,
                                      table => table.ContentPartRecord()
                                                    .Column<string>("UserToken")
                                                    .Column<string>("UserSecret"));
            ContentDefinitionManager.AlterPartDefinition(typeof (DropboxUserSettingsPartRecord).Name,
                                                         part => part.Attachable(false));
            ContentDefinitionManager.AlterTypeDefinition("User", type => type.WithPart(typeof (DropboxUserSettingsPart).Name));
            return 2;
        }
    }
}