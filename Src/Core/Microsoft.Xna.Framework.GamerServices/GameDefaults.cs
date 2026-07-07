using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class GameDefaults
    {
        internal GameDefaults()
        {
        }

        public bool AccelerateWithButtons => false;

        public bool AutoAim => true;

        public bool AutoCenter => true;

        public bool BrakeWithButtons => false;

        public ControllerSensitivity ControllerSensitivity => ControllerSensitivity.Medium;

        public GameDifficulty GameDifficulty => GameDifficulty.Normal;

        public bool InvertYAxis => false;

        public bool ManualTransmission => false;

        public bool MoveWithRightThumbStick => false;

        public Color? PrimaryColor => null;

        public RacingCameraAngle RacingCameraAngle => RacingCameraAngle.Back;

        public Color? SecondaryColor => null;
    }
}
