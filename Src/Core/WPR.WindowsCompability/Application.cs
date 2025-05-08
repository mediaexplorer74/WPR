using System;

namespace WPR.WindowsCompability
{
    public class Application
    {
        private static Application? _Current;
        public event EventHandler<ApplicationUnhandledExceptionEventArgs>? UnhandledException;

        public string? ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                productId = value;
            }
        }
        private ResourceDictionary _Resources;
        private string? productId;

        internal Application()
        {
            _Resources = new ResourceDictionary();
        }

        public static Application Current 
        {
            get 
            {
                if (_Current == null)
                {
                    _Current = new Application();
                }
                return _Current;
            }
        }

        public ResourceDictionary Resources
        {
            get
            {
                return _Resources;
            }
        }
    }
}