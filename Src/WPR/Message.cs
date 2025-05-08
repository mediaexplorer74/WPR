// Message Model Class

using Newtonsoft.Json;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WPR
{
    public class Message
    {
        public string Content { get; set; }
        public bool IsUserMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Serializable properties
        //public string IconPath => IsUserMessage ?
        //    "ms-appx:///Assets/Icons/user-icon.png" :
        //    "ms-appx:///Assets/Icons/bot-icon.png";

        [JsonIgnore]
        //public ImageSource Icon => new BitmapImage(new Uri(IconPath));
        public string Icon => IsUserMessage ?
        "ms-appx:///Assets/Icons/user-icon.png" :
        "ms-appx:///Assets/Icons/bot-icon.png";

        [JsonIgnore]
        public string Initials => IsUserMessage ? "U" : "AI";

        [JsonIgnore]
        public SolidColorBrush BubbleColor => IsUserMessage ?
            new SolidColorBrush(Color.FromArgb(255, 0, 120, 212)) :
            new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));

        [JsonIgnore]
        public SolidColorBrush TextColor => IsUserMessage ?
            new SolidColorBrush(Colors.White) :
            new SolidColorBrush(Colors.Black);

        [JsonIgnore]
        public HorizontalAlignment Alignment => IsUserMessage ?
            HorizontalAlignment.Right :
            HorizontalAlignment.Left;

        // Add parameterless constructor for JSON deserialization
        public Message() { }
    }
}
