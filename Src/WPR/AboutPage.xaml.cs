// Limits Page

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace WPR
{
    public sealed partial class AboutPage : Page
    {
        //private const string RequestLimitSetting = "DailyRequestLimit";
        private const string OpenRouterLimitsUrl = "https://openrouter.ai/api/v1/auth/key";

        public AboutPage()
        {
            InitializeComponent();
           
        }//

        

        // *** Action bar handling methods - BEGIN ***
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }//Settings_Click
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }//Home_Click    

        // *** Action bar handling methods - END ***

    }//AboutPage class end

  
}