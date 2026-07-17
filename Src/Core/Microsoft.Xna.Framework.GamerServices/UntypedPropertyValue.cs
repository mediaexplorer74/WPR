using System;

namespace Microsoft.Xna.Framework.GamerServices
{
    internal sealed class UntypedPropertyValue : PropertyValue
    {
        private object? currentValue;

        public override object GetValue() => currentValue!;

        public override void SetValue(object value)
        {
            if (Equals(value, currentValue))
                return;

            currentValue = value;
            IsChanged = true;
        }
    }
}
