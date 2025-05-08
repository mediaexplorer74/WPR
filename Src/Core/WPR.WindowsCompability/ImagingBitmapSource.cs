// System.Windows.Media.Imaging "imitation"

using System;


namespace WPR.WindowsCompability
{
    // projection: System.Windows.Media.Imaging.BitmapSource
    public class BitmapSource 
    {
        public BitmapSource()
        {
        }

        //static 
        //BitmapSource(int ActualWidth, int ActualHeight)
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

        //RnD
        //public static void SetSource(System.IO.Stream stream)
        //{       
        //TODO
        //   return;
        //}

        //public static void SetSource()
        //{
        //    //TODO
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
            return (Int32)1; //RnD
        }

        public Int32 get_PixelHeight()
        {
            return (Int32)1; //RnD
        }

    }//BitmapSource

    //RnD
    /*
    public class BitmapSource2 : BitmapSource
    {
        //public static Action<DisplayOrientation>? RequestOrientation;

        public BitmapSource2()
            : base()
        {
            return;
        }

       
        public void SetSource(System.IO.Stream stream)
        {
            //TODO
            return;
        }

        public Int32 get_PixelWidth()
        { 
            return (Int32)4; //RnD
        }

        public Int32 get_PixelHeight()
        {
            return (Int32)4; //RnD
        }
        
    }
    */
}

