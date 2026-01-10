using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Projektanker.Icons.Avalonia
{
    public partial class Icon : TemplatedControl
    {
        public static readonly DirectProperty<Icon, DrawingImage> DrawingImageProperty =
            AvaloniaProperty.RegisterDirect<Icon, DrawingImage>(nameof(DrawingImage), o => o.DrawingImage);

        public static readonly StyledProperty<string> ValueProperty =
            AvaloniaProperty.Register<Icon, string>(nameof(Value));

        public static readonly StyledProperty<IconAnimation> AnimationProperty =
            AvaloniaProperty.Register<Icon, IconAnimation>(nameof(Animation));

        private DrawingImage _drawingImage = new DrawingImage();

        static Icon()
        {
        }

        public DrawingImage DrawingImage
        {
            get => _drawingImage;
            private set => SetAndRaise(DrawingImageProperty, ref _drawingImage, value);
        }

        public string Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public IconAnimation Animation
        {
            get => GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        private void OnValueChanged()
        {
            // Try to interpret Value as path data; if that fails, produce an empty drawing.
            try
            {
                string path = Value ?? string.Empty;
                Geometry geometry = Geometry.Parse(path);

                var drawing = new GeometryDrawing()
                {
                    Geometry = geometry,
                    Brush = Foreground ?? new SolidColorBrush(0),
                };

                DrawingImage = new DrawingImage { Drawing = drawing };
            }
            catch
            {
                DrawingImage = new DrawingImage { Drawing = new GeometryDrawing() };
            }
        }

        private void OnForegroundChanged()
        {
            if (DrawingImage?.Drawing is GeometryDrawing geometryDrawing)
            {
                DrawingImage.Drawing = new GeometryDrawing
                {
                    Geometry = geometryDrawing.Geometry,
                    Brush = Foreground,
                };
            }
        }
    }
}