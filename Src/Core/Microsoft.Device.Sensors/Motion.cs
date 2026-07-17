using System;

namespace Microsoft.Devices.Sensors;

public sealed class Motion : SensorBase<MotionReading>
{
    public static bool IsSupported => false;

    public SensorState State { get; private set; } = SensorState.Ready;
}

public sealed class MotionReading : ISensorReading
{
    public DateTimeOffset Timestamp { get; set; }
    public Microsoft.Xna.Framework.Vector3 Gravity { get; set; }
    public Microsoft.Xna.Framework.Vector3 UserAcceleration { get; set; }
    public Microsoft.Xna.Framework.Matrix RotationMatrix { get; set; }
    public AttitudeReading Attitude { get; set; } = new();
}

public sealed class AttitudeReading
{
    public float Yaw { get; set; }
    public float Pitch { get; set; }
    public float Roll { get; set; }
    public Microsoft.Xna.Framework.Quaternion Quaternion { get; set; }
}
