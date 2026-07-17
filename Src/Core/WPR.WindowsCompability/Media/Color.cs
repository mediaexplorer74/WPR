namespace WPR.WindowsCompability.Media
{
    public struct Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static Color FromArgb(byte alpha, byte red, byte green, byte blue) => new()
        {
            A = alpha,
            R = red,
            G = green,
            B = blue
        };
    }
}
