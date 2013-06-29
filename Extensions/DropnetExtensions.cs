using System.Collections.Generic;
using System.Linq;
using DropNet.Models;

namespace Kobowi.Dropbox.Extensions {
    public static class DropnetExtensions {
        public static dynamic ToClientViewModel(this List<MetaData> contents) {
            return contents.Select(i => new {
                name = i.Name,
                isFolder = i.Is_Dir,
                icon = i.Icon,
                size = i.Size,
                extension = i.Extension,
                modified = i.Modified,
                hasThumb = i.Thumb_Exists
            });
        }
    }
}