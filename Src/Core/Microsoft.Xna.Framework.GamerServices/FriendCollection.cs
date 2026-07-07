using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class FriendCollection : GamerCollection<FriendGamer>, IDisposable
    {
        internal FriendCollection()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _isDisposed;

        private void Dispose(bool disposing)
        {
            _isDisposed = true;
        }

        public bool IsDisposed => _isDisposed;

    }
}
