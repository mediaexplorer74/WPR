using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.Shell
{
    public class ClosingEventArgs : EventArgs
    {
        private ClosingReason _reason;

        public ClosingEventArgs()
        {
            this._reason = ClosingReason.UserAction;
        }

        public ClosingEventArgs(ClosingReason reason)
        {
            this._reason = reason;
        }

        public ClosingReason Reason
        {
            get
            {
                return this._reason;
            }
        }
    }
}
