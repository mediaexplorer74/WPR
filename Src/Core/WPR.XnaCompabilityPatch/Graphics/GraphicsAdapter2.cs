using Microsoft.Xna.Framework.Graphics;

namespace WPR.XnaCompability.Graphics
{
    public class GraphicsAdapter2 : GraphicsAdapter
    {
        public GraphicsAdapter2(
            DisplayModeCollection modes,
            string name,
            string description
        )
            : base(modes, name, description)
        {
        }

        //RnD
        public DisplayMode get_CurrentDisplayMode()
        {
            //return new DisplayMode(480, 800, CurrentDisplayMode.Format);
            return new DisplayMode(800, 600, CurrentDisplayMode.Format);
            //return new DisplayMode(640, 480, CurrentDisplayMode.Format);
        }
    }
}
