using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GamerPrivileges
    {
        internal GamerPrivileges()
        {
        }

        public GamerPrivilegeSetting AllowCommunication => GamerPrivilegeSetting.Everyone;

        public bool AllowOnlineSessions => true;

        public GamerPrivilegeSetting AllowProfileViewing => GamerPrivilegeSetting.Everyone;

        public bool AllowPurchaseContent
        {
            get
            {
                return false; //!
            }
        }

        public bool AllowTradeContent => true;

        public GamerPrivilegeSetting AllowUserCreatedContent => GamerPrivilegeSetting.Everyone;
    }
}
