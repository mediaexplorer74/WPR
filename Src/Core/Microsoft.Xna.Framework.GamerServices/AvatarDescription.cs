using System;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class AvatarDescription
    {
        private static readonly byte[] EmptyDescription = Array.Empty<byte>();

        public AvatarDescription(byte[] data)
        {
            BodyType = AvatarBodyType.Male;
        }

        private AvatarDescription(AvatarBodyType bodyType)
        {
            BodyType = bodyType;
        }

        public static AvatarDescription CreateRandom()
        {
            return new AvatarDescription(EmptyDescription);
        }

        public static AvatarDescription CreateRandom(AvatarBodyType bodyType)
        {
            return new AvatarDescription(bodyType);
        }

        public static IAsyncResult BeginGetFromGamer(
            string gamertag, AsyncCallback? callback, object? asyncState)
        {
            var source = new System.Threading.Tasks.TaskCompletionSource<AvatarDescription>(
                asyncState, System.Threading.Tasks.TaskCreationOptions.RunContinuationsAsynchronously);
            source.SetResult(CreateRandom());
            callback?.Invoke(source.Task);
            return source.Task;
        }

        public static AvatarDescription EndGetFromGamer(IAsyncResult result)
        {
            return ((System.Threading.Tasks.Task<AvatarDescription>)result).GetAwaiter().GetResult();
        }

        public event EventHandler<EventArgs>? Changed;

        public AvatarBodyType BodyType { get; }

        public byte[] Description => EmptyDescription;

        public float Height => 1.0f;

        public bool IsValid => true;
    }
}
