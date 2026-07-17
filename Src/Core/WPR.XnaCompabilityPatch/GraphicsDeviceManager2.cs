using Microsoft.Xna.Framework;
using System;

namespace WPR.XnaCompability
{
    public class GraphicsDeviceManager2 : GraphicsDeviceManager
    {
        public new static int DefaultBackBufferWidth = 800;
        public new static int DefaultBackBufferHeight = 480;

        public static Action<DisplayOrientation>? RequestOrientation;

        public GraphicsDeviceManager2(Game game)
            : base(game)
        {

        }

#if !__MOBILE__
        public new bool IsFullScreen
        {
            get => false;
            set => base.IsFullScreen = false;
        }
#endif

        public new void ApplyChanges()
        {
            base.ApplyChanges();
            RequestOrientationChange(PreferredBackBufferWidth, PreferredBackBufferHeight);
        }

        public static void RequestOrientationChange(int width, int height)
        {
            DisplayOrientation device_orientation = default;

            if (width > height)
            {
                device_orientation = DisplayOrientation.LandscapeRight;
            }
            else
            {
                device_orientation = DisplayOrientation.Portrait;
            }
            RequestOrientation?.Invoke(device_orientation);
        }
    }
}
