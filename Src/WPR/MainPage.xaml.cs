// Main Page

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace WPR
{
    public sealed partial class MainPage : Page
    {
        readonly Game1 _game;

     
        public MainPage()
        {
            this.InitializeComponent();


            // RnD / TODO
            // Create the game.
            var launchArguments = string.Empty;

            //RnD: Uncomment the following line to enable game core window "swapchaining"
            /*_game = MonoGame.Framework.XamlGame<Game1>.Create(
                launchArguments,
                Window.Current.CoreWindow,
                swapChainPanel);*/


            this.Loaded += MainPage_Loaded;            
          
        }

       
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize converters
            Resources.Add("BoolToVisibilityConverter", new BoolToVisibilityConverter());
            Resources.Add("InverseBoolToVisibilityConverter", new InverseBoolToVisibilityConverter());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);           
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);          
        }

      
     
        // *** Action bar handling methods - BEGIN ***
        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }//Settings_Click

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }//About_Click


        // *** Action bar handling methods - END ***



        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

    
       

        //Experimental
        private async void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            // RnD / TODO
            System.Threading.Tasks.Task<bool> TResult = default;
            
            try
            {
                TResult = FilePatcher.PatchFile("FilePath");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in ProcessButton_Click: {ex}");
                ResultText.Text = $"FileTouch process failed! Exception: {ex.Message}";
                return;
            }

            ResultText.Text = "FileTouch process completed!";
            
        }

   
    }//MainPage class end
}