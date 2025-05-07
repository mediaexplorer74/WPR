using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Diagnostics;
using System.Threading.Tasks;


namespace WPR
{
   
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
           //
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // RnD / TODO
            System.Threading.Tasks.Task<bool> r = WPR.FileTouch("FilePath");
            if (r.Result == true)
            {
                ResultText.Text = "FileTouch process completed!";
            }
            else
            {
                ResultText.Text = "FileTouch process failed!";
            }
        }
    }
}
