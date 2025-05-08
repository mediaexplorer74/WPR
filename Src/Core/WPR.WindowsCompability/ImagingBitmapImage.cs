// System.Windows.Media.Imaging "imitation"

using System;


namespace WPR.WindowsCompability
{
    // projection: System.Windows.Media.Imaging.BitmapImage
    public class BitmapImage 
    {
        public BitmapImage()
        {
        }

        //static 
        //BitmapImage(int ActualWidth, int ActualHeight)
        //{
        /*
        writeableBitmap = new WriteableBitmap(
            (int)ActualWidth,
            (int)ActualHeight,
            96,
            96,
            default,//PixelFormats.Bgr32,
            null);
        */
        //    return;
        //}

        
        public void SetSource(System.IO.Stream stream)
        {
            //BitmapImage bit =
            //new BitmapImage(new Uri("/Resources/1.jpg", UriKind.Relative));
            //img.Source = bit;
            //TODO

            //Image image = new Image
            //{
            //    Source = ImageSource.FromFile("forest.png")
            //};

            //Content = image;
            //return;
        }

        public Int32 get_PixelWidth()
        {
            return (Int32)4; //RnD
        }

        public Int32 get_PixelHeight()
        {
            return (Int32)4; //RnD
        }

    }//BitmapSource

   
}

