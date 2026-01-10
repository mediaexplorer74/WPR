using System.Collections.Generic;

namespace DesktopNotifications
{
    /// <summary>
    /// </summary>
    public class Notification
    {
        public Notification()
        {
            Buttons = new List<(string Title, string ActionId)>();
        }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? ImagePath { get; set; }

        // NOTE: This only works on packaged app (Android or WinRT)
        // The sound name needs to be in resource folder
        public string? SoundUri { get; set; }

        public List<(string Title, string ActionId)> Buttons { get; }
    }
}