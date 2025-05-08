using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class SignedOutEventArgs : EventArgs
    {
        private SignedInGamer1 gamer;

        public SignedOutEventArgs(SignedInGamer1 gamer)
        {
            if (gamer == null)
            {
                throw new ArgumentNullException("gamer");
            }
            this.gamer = gamer;
        }

        public SignedInGamer1 Gamer
        {
            get
            {
                return this.gamer;
            }
        }

    }
}
