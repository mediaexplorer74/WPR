using System;

namespace Microsoft.Phone.Tasks
{
    public abstract class ChooserBase<TTaskEventArgs> where TTaskEventArgs : TaskEventArgs
    {
        public ChooserBase()
        {
        }

        public event EventHandler<TTaskEventArgs>? Completed;

        public void Show()
        {
            // Choosers are phone-shell UI. Desktop compatibility leaves them
            // pending/offline and does not fabricate a successful completion.
        }
    }
}
