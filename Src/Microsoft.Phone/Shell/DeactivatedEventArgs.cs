using System;

namespace Microsoft.Phone.Shell
{
    /// <summary>
    /// Provides event data for application deactivation events in the phone shell environment.
    /// </summary>
    public class DeactivatedEventArgs : EventArgs
    {
        // Private field storing the deactivation reason
        private DeactivationReason _reason;

        /// <summary>
        /// Initializes a new instance of the DeactivatedEventArgs class with default UserAction reason
        /// </summary>
        public DeactivatedEventArgs()
        {
            this._reason = DeactivationReason.UserAction;
        }

        /// <summary>
        /// Initializes a new instance of the DeactivatedEventArgs class with specified deactivation reason
        /// </summary>
        /// <param name="reason">The enumeration value indicating the deactivation cause</param>
        public DeactivatedEventArgs(DeactivationReason reason)
        {
            this._reason = reason;
        }

        /// <summary>
        /// Gets the reason for application deactivation
        /// </summary>
        /// <value>
        /// One of the DeactivationReason enumeration values describing the deactivation cause
        /// </value>
        public DeactivationReason Reason
        {
            get
            {
                return this._reason;
            }
        }
    }
}