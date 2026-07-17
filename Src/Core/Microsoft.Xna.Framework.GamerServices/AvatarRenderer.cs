using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.GamerServices;

public class AvatarRenderer : IDisposable
{
    [CLSCompliant(false)]
    public const int BoneCount = 0x47;

    private readonly ReadOnlyCollection<Matrix> bindPose;
    private readonly ReadOnlyCollection<int> parentBones;
    private bool isDisposed;

    public AvatarRenderer(AvatarDescription avatarDescription)
        : this(avatarDescription, true)
    {
    }

    public AvatarRenderer(AvatarDescription avatarDescription, bool useLoadingEffect)
    {
        var bones = new Matrix[BoneCount];
        var parents = new int[BoneCount];
        for (int index = 0; index < BoneCount; index++)
        {
            bones[index] = Matrix.Identity;
            parents[index] = -1;
        }
        bindPose = Array.AsReadOnly(bones);
        parentBones = Array.AsReadOnly(parents);
        AmbientLightColor = Vector3.One;
        LightColor = Vector3.One;
        Projection = Matrix.Identity;
        View = Matrix.Identity;
        World = Matrix.Identity;
    }

    public Vector3 AmbientLightColor { get; set; }

    public ReadOnlyCollection<Matrix> BindPose => bindPose;

    public bool IsDisposed => isDisposed;

    public bool IsLoaded => !isDisposed;

    public AvatarRendererState State => isDisposed
        ? AvatarRendererState.Unavailable
        : AvatarRendererState.Ready;

    public Vector3 LightColor { get; set; }

    public Vector3 LightDirection { get; set; }

    public ReadOnlyCollection<int> ParentBones => parentBones;

    public Matrix Projection { get; set; }

    public Matrix View { get; set; }

    public Matrix World { get; set; }

    public void Draw(IList<Matrix> bones, AvatarExpression expression)
    {
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
}
