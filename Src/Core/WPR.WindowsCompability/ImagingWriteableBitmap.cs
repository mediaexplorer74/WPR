// System.Windows.Media.Imaging "imitation"

using System;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace WPR.WindowsCompability
{
    // projection: System.Windows.Media.Imaging.WriteableBitmap
    public class WriteableBitmap 
    {
        
        Int32 ImgActualWidth;
        Int32 ImgActualHeight;

        
        public WriteableBitmap(Int32 ActualWidth, Int32 ActualHeight)
        {
            /*
            writeableBitmap = new WriteableBitmap(
                (int)ActualWidth,
                (int)ActualHeight,
                96,
                96,
                default,//PixelFormats.Bgr32,
                null);
            */
            
            ImgActualWidth = ActualWidth;
            ImgActualHeight = ActualHeight;
        }

        public void Invalidate()
        {
            return;
        }

        public Int32[] get_Pixels()
        {
            int stride = ImgActualWidth;// * 4;
            int size = ImgActualHeight;// * stride;
            Int32[] pixels = new Int32[size];
            //img.CopyPixels(pixels, stride, 0);
            return pixels; //RnD
        }

    }
}
