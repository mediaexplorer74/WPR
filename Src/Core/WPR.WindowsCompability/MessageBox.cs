using System;
using System.Windows;
using System.Threading.Tasks;

namespace WPR.WindowsCompability
{
    public static class MessageBox
    {
        public static Func<string, string, MessageBoxButton, Task<MessageBoxResult>>? ShowSimpleImpl;

        public static MessageBoxResult Show(string title, string caption, MessageBoxButton buttons)
        {
            return ShowSimpleImpl?.Invoke(title, caption, buttons).GetAwaiter().GetResult()
                ?? MessageBoxResult.OK;
        }
    }

}
