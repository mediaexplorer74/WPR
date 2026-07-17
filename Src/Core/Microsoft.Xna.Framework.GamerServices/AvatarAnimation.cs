using System;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.GamerServices;

public class AvatarAnimation : IDisposable
{
    private const int AvatarBoneCount = 0x47;

    private readonly ReadOnlyCollection<Matrix> boneTransforms;
    private readonly AvatarExpression currentExpression = new();
    private TimeSpan currentPosition;
    private bool isDisposed;

    public AvatarAnimation(AvatarAnimationPreset animationPreset)
    {
        var bones = new Matrix[AvatarBoneCount];
        for (int index = 0; index < bones.Length; index++)
        {
            bones[index] = Matrix.Identity;
        }
        boneTransforms = Array.AsReadOnly(bones);
        Length = TimeSpan.FromSeconds(1);
    }

    public ReadOnlyCollection<Matrix> BoneTransforms => boneTransforms;

    public TimeSpan CurrentPosition
    {
        get => currentPosition;
        set => currentPosition = NormalizePosition(value, loop: false);
    }

    public AvatarExpression Expression => currentExpression;

    public bool IsDisposed => isDisposed;

    public TimeSpan Length { get; }

    public void Update(TimeSpan elapsedAnimationTime, bool loop)
    {
        currentPosition = NormalizePosition(currentPosition + elapsedAnimationTime, loop);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        isDisposed = true;
    }

    private TimeSpan NormalizePosition(TimeSpan value, bool loop)
    {
        if (value < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }
        if (value <= Length)
        {
            return value;
        }
        return loop ? TimeSpan.FromTicks(value.Ticks % Length.Ticks) : Length;
    }
}
