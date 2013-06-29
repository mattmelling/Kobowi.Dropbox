using DropNet;
using Orchard;
using Orchard.Security;

namespace Kobowi.Dropbox.Services {
    public interface IDropboxService : IDependency {
        DropNetClient GetClient();
        DropNetClient GetClient(IUser user);
    }
}