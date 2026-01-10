using System;
using Avalonia;
using AvaloniaMenuItem = Avalonia.Controls.MenuItem;

namespace Projektanker.Icons.Avalonia
{
    public static class MenuItem
    {
        /// <summary>
        /// Identifies the <seealso cref="IconProperty"/> avalonia attached property.
        /// </summary>
        public static readonly AttachedProperty<string> IconProperty =
            AvaloniaProperty.RegisterAttached<Icon, AvaloniaMenuItem, string>("Icon", string.Empty);

        static MenuItem()
        {
            IconProperty.Changed.Subscribe(new PropertyChangedObserver<string>(IconChanged));
        }

        /// <summary>
        /// Accessor for attached property <see cref="IconProperty"/>
        /// </summary>
        public static string GetIcon(AvaloniaMenuItem target)
        {
            return target.GetValue(IconProperty);
        }

        /// <summary>
        /// Accessor for attached property <see cref="IconProperty"/>
        /// </summary>
        public static void SetIcon(AvaloniaMenuItem target, string value)
        {
            target.SetValue(IconProperty, value);
        }

        private static void IconChanged(AvaloniaPropertyChangedEventArgs<string> evt)
        {
            if (evt.Sender is not AvaloniaMenuItem target)
            {
                return;
            }

            // Read the actual attached property value from the target to avoid depending on BindingValue<T> internals
            string value = GetIcon(target) ?? string.Empty;

            target.Icon = new Icon()
            {
                Value = value,
            };
        }

        private sealed class PropertyChangedObserver<T> : IObserver<AvaloniaPropertyChangedEventArgs<T>>
        {
            private readonly Action<AvaloniaPropertyChangedEventArgs<T>> _action;

            public PropertyChangedObserver(Action<AvaloniaPropertyChangedEventArgs<T>> action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            public void OnCompleted() { }
            public void OnError(Exception error) { }
            public void OnNext(AvaloniaPropertyChangedEventArgs<T> value) => _action(value);
        }
    }
}