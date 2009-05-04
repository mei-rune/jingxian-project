using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace jingxian.ui
{
    using Empinia.UI;

    class Helper
    {


        public static string GetPath(string path, IVirtualFileSystem virtualFs)
        {
            if (System.IO.Path.IsPathRooted(path))
                return System.IO.File.Exists(path) ? path : null;

            path = virtualFs.GetBinPath(path);

            if (System.IO.File.Exists(path))
                return System.IO.Path.GetFullPath(path);

            return null;
        }

        public static Image GetImage(string iconId,string defaultId,IIconResourceService iconService, IVirtualFileSystem virtualFs  )
        {
            try
            {
                if (iconId.StartsWith("$(file)/"))
                {
                    string path = GetPath(iconId.Substring("$(file)/".Length), virtualFs);
                    if (string.IsNullOrEmpty(path))
                        return iconService.GetBitmap(defaultId);
                    else
                        return Image.FromFile(path);
                }
                else
                {
                    return iconService.GetBitmap(iconId);
                }
            }
            catch
            {
                return iconService.GetBitmap(defaultId);
            }
        }
    }
}
