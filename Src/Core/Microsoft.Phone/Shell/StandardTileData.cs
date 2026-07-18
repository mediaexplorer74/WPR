using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Shell
{
    public class StandardTileData : ShellTileData
    {
        public Uri? BackgroundImage { get; set; }
        public int? Count { get; set; }
        public string? BackContent { get; set; }
        public Uri? BackBackgroundImage { get; set; }
        public string? BackTitle { get; set; }
    }
}
