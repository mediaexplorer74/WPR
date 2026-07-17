using System;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class GamerServicesComponent : GameComponent
    {
        public GamerServicesComponent(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            GamerServicesDispatcher.Update();
            base.Update(gameTime);
        }
    }
}
