using System;
using System.Collections.Generic;

namespace Microsoft.Phone.Shell
{
    public class ShellTile
    {
        private static readonly List<ShellTile> _ActiveTiles;

        static ShellTile()
        {
            _ActiveTiles = new List<ShellTile>
            {
                new ShellTile(new Uri("/", UriKind.Relative))
            };
        }

        private ShellTile(Uri navigationUri)
        {
            NavigationUri = navigationUri;
        }

        public static IEnumerable<ShellTile> ActiveTiles => _ActiveTiles;
        public Uri NavigationUri { get; private set; }

        public void Update(ShellTileData data)
        {

        }
    }
}
