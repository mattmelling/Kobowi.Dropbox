using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Kobowi.Dropbox.Providers {
    public class DropboxSearchMenu : INavigationProvider {
        public Localizer T { get; set; }

        public DropboxSearchMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName { get { return "mediaproviders"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("dropbox")
                .Add(T("Dropbox"), "10",
                    menu => menu.Action("Index", "DropboxStorage", new { area = "Kobowi.Dropbox" })
                        .Permission(Orchard.MediaLibrary.Permissions.ManageMediaContent));
        }
    }
}