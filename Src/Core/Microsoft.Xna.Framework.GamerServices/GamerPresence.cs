using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GamerPresence
    {
        internal GamerPresence()
        {
        }

        public GamerPresenceMode PresenceMode { get; set; } = GamerPresenceMode.None;

        public int PresenceValue { get; set; }
    }
}
