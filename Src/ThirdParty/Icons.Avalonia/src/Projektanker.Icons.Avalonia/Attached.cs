using System;
using Avalonia;
using Avalonia.Controls;

namespace Projektanker.Icons.Avalonia
{
    public static class Attached
    {
        /// <summary>
        /// Identifies the <seealso cref="IconProperty"/> avalonia attached property.
        /// </summary>
        public static readonly AttachedProperty<string> IconProperty =
            AvaloniaProperty.RegisterAttached<Icon, ContentControl, string>("Icon", string.Empty);

        static Attached()
        {
            IconProperty.Changed.Subscribe(new PropertyChangedObserver<string>(IconChanged));
        }

        /// <summary>
        /// Accessor for attached property <see cref="IconProperty"/>
        /// </summary>
        public static string GetIcon(ContentControl target)
        {
            return target.GetValue(IconProperty);
        }

        /// <summary>
        /// Accessor for attached property <see cref="IconProperty"/>
        /// </summary>
        public static void SetIcon(ContentControl target, string value)
        {
            target.SetValue(IconProperty, value);
        }

        private static void IconChanged(AvaloniaPropertyChangedEventArgs<string> evt)
        {
            if (evt.Sender is not ContentControl target)
            {
                return;
            }

            // Read the actual attached property value from the target to avoid depending on BindingValue<T> internals
            string value = GetIcon(target) ?? string.Empty;

            target.Content = new Icon()
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