using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class SignedInGamerCollection : GamerCollection<SignedInGamer1>
    {
        internal SignedInGamerCollection()
        {
        }

        public SignedInGamerCollection(List<SignedInGamer1> gamerList)
            : base(gamerList)
        {

        }

        public SignedInGamer1 this[PlayerIndex index]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    SignedInGamer1 gamer = base[i];
                    if (gamer.PlayerIndex == index)
                    {
                        return gamer;
                    }
                }
                return null;
            }
        }
    }
}
