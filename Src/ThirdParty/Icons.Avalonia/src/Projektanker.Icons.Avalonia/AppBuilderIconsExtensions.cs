using System;

namespace Projektanker.Icons.Avalonia
{
    public static class AppBuilderIconsExtensions
    {
        public static TAppBuilder WithIcons<TAppBuilder>(this TAppBuilder appBuilder, Action<IIconProviderContainer> configure)
             where TAppBuilder : class
        {
            var iconProvider = new IconProvider();
            configure(iconProvider);

            // Try to register icon provider via common pattern if available
            try
            {
                // If appBuilder has a "With" method, attempt to call it via reflection
                var withMethod = typeof(TAppBuilder).GetMethod("With", new Type[] { typeof(object) });
                if (withMethod != null)
                {
                    withMethod.Invoke(appBuilder, new object[] { iconProvider });
                }
            }
            catch
            {
                // ignore - optional integration
            }

            return appBuilder;
        }
    }
}