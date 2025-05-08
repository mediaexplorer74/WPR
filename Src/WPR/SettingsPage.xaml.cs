// Settings Page

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;


namespace WPR
{
    public sealed partial class SettingsPage : Page
    {
        private const string ApiKeySetting = "DeepseekApiKey";

        public SettingsPage()
        {
            InitializeComponent();
            LoadApiKey();
        }

        private void LoadApiKey()
        {
            ApiKeyBox.Password = ApplicationData.Current.LocalSettings.Values[ApiKeySetting]?.ToString() ?? "";
        }

        private void SaveApiKey_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values[ApiKeySetting] = ApiKeyBox.Password;
        }

        private async void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Clear Chat History",
                Content = "This will permanently delete all chat messages. Are you sure?",
                PrimaryButtonText = "Clear",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await ChatStorageHelper.DeleteChatAsync();

                // Navigate back to MainPage to see changes
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        // *** Action bar handling methods - BEGIN ***
        private void Limits_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }//Limits_Click
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }//Home_Click    

        // *** Action bar handling methods - END ***
    }
}
