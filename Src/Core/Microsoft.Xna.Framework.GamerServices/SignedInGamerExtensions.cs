using System;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class SignedInGamerExtensions
    {
        public static IAsyncResult BeginAwardAvatarAssets(this SignedInGamer gamer,
            string[] assetKeys, AsyncCallback? callback, object? asyncState)
        {
            ArgumentNullException.ThrowIfNull(gamer);
            ArgumentNullException.ThrowIfNull(assetKeys);

            var completion = new TaskCompletionSource<object?>(asyncState,
                TaskCreationOptions.RunContinuationsAsynchronously);
            completion.SetResult(null);
            callback?.Invoke(completion.Task);
            return completion.Task;
        }

        public static void EndAwardAvatarAssets(this SignedInGamer gamer, IAsyncResult result)
        {
            ArgumentNullException.ThrowIfNull(gamer);
            ArgumentNullException.ThrowIfNull(result);

            if (result is Task task)
            {
                task.GetAwaiter().GetResult();
            }
        }
    }
}
