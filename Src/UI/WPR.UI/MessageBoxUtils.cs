using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

#if __ANDROID__
using Android.App;
using Android.Widget;
#else
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Layout;
using Avalonia;
#endif

using MessageBox.Avalonia;
using static System.Net.Mime.MediaTypeNames;

namespace WPR.UI
{
    public static class MessageBoxUtils
    {
#if __ANDROID__
        public static Activity MainActivity { get; set; }
#else
        public static Window MainWindow { get; set; }
#endif

        public static Task<MessageBox.Avalonia.Enums.ButtonResult> GetMessageDialogResult(string title,
            string text, MessageBox.Avalonia.Enums.ButtonEnum buttons = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon icon = MessageBox.Avalonia.Enums.Icon.Info, IEnumerable<string> ?buttonTexts = null,
            bool modalOnWindow = true, bool dispatchMain = false)
        {
#if __ANDROID__
            TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult> source = new TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            MainActivity.RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity)!
                    .SetTitle(title)!
                    .SetMessage(text)!;

                switch (buttons)
                {
                    case MessageBox.Avalonia.Enums.ButtonEnum.Ok:
                        if (buttonTexts != null)
                        {
                            var enumerable = buttonTexts.GetEnumerator();
                            enumerable.MoveNext();

                            builder = builder.SetPositiveButton(enumerable.Current, (dialog, which) =>
                            {
                                Common.Log.Error(Common.LogCategory.AppList, "OK reported");
                                source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Ok);
                                (dialog as AlertDialog)!.Dismiss();
                            })!;
                        }
                        else
                        {
                            builder = builder.SetPositiveButton(Android.Resource.String.Ok, (dialog, which) =>
                            {
                                Common.Log.Error(Common.LogCategory.AppList, "OK reported");
                                source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Ok);
                                (dialog as AlertDialog)!.Dismiss();
                            })!;
                        }

                        break;

                    case MessageBox.Avalonia.Enums.ButtonEnum.YesNo:
                        if (buttonTexts != null)
                        {
                            var enumerable = buttonTexts.GetEnumerator();
                            enumerable.MoveNext();

                            builder = builder.SetNegativeButton(enumerable.Current, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.No);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;

                            enumerable.MoveNext();

                            builder = builder
                                .SetPositiveButton(enumerable.Current, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Yes);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;
                            
                        } else
                        {
                            builder = builder
                                .SetPositiveButton(Android.Resource.String.Yes, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.Yes);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!
                                .SetNegativeButton(Android.Resource.String.No, (dialog, which) =>
                                {
                                    source.SetResult(MessageBox.Avalonia.Enums.ButtonResult.No);
                                    (dialog as AlertDialog)!.Dismiss();
                                })!;
                        }
                        break;

                }

                switch (icon)
                {
                    case MessageBox.Avalonia.Enums.Icon.Warning:
                        builder = builder.SetIcon(Android.Resource.Drawable.IcDialogAlert)!;
                        break;

                    case MessageBox.Avalonia.Enums.Icon.Info:
                        builder = builder.SetIcon(Android.Resource.Drawable.IcDialogInfo)!;
                        break;

                    default:
                        break;
                }

                builder.Create()!.Show();
            });

            return source.Task;
#else
            Func<Task<MessageBox.Avalonia.Enums.ButtonResult>> returnTaskFunc = () =>
            {
                try
                {
                    var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                        title: title,
                        text: text,
                        icon: icon,
                        @enum: buttons,
                        windowStartupLocation: WindowStartupLocation.CenterScreen);

                    return modalOnWindow ? msgBox.ShowDialog(MainWindow) : msgBox.Show();
                }
                catch (System.MissingMethodException ex)
                {
                    // Fallback: create a simple Avalonia Window dialog to avoid dependency issues
                    return CreateFallbackMessageBoxAsync(title, text, buttons, buttonTexts, modalOnWindow);
                }
            };

            if (dispatchMain)
            {
                return Dispatcher.UIThread.InvokeAsync(returnTaskFunc);
            }

            return returnTaskFunc();
#endif
        }

#if !__ANDROID__
        static async Task<MessageBox.Avalonia.Enums.ButtonResult> CreateFallbackMessageBoxAsync(string title, string text,
            MessageBox.Avalonia.Enums.ButtonEnum buttons, IEnumerable<string>? buttonTexts, bool modalOnWindow)
        {
            var tcs = new TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var wnd = new Window()
            {
                Title = title,
                Width = 480,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(12)
            };

            var textBlock = new TextBlock()
            {
                Text = text,
                Margin = new Thickness(0, 0, 0, 12)
            };

            var buttonsPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };

            void CloseWith(MessageBox.Avalonia.Enums.ButtonResult r)
            {
                if (!tcs.TrySetResult(r)) return;
                try { wnd.Close(); } catch { }
            }

            if (buttons == MessageBox.Avalonia.Enums.ButtonEnum.Ok)
            {
                var ok = new Button { Content = (buttonTexts != null ? (buttonTexts.FirstOrDefault() ?? "OK") : "OK"), Margin = new Thickness(6,0) };
                ok.Click += (_,__) => CloseWith(MessageBox.Avalonia.Enums.ButtonResult.Ok);
                buttonsPanel.Children.Add(ok);
            }
            else
            {
                // YesNo
                string yesText = "Yes";
                string noText = "No";
                if (buttonTexts != null)
                {
                    var list = new List<string>(buttonTexts);
                    if (list.Count >= 1) yesText = list[0];
                    if (list.Count >= 2) noText = list[1];
                }

                var no = new Button { Content = noText, Margin = new Thickness(6,0) };
                no.Click += (_,__) => CloseWith(MessageBox.Avalonia.Enums.ButtonResult.No);
                var yes = new Button { Content = yesText, Margin = new Thickness(6,0) };
                yes.Click += (_,__) => CloseWith(MessageBox.Avalonia.Enums.ButtonResult.Yes);

                buttonsPanel.Children.Add(no);
                buttonsPanel.Children.Add(yes);
            }

            panel.Children.Add(textBlock);
            panel.Children.Add(buttonsPanel);

            wnd.Content = panel;

            // Show window
            if (modalOnWindow && MainWindow != null)
            {
                // Owner set implicitly by ShowDialog
                _ = wnd.ShowDialog(MainWindow);
            }
            else
            {
                wnd.Show();
            }

            return await tcs.Task.ConfigureAwait(false);
        }
#endif

        public static Task<string> GetInputResult(string title, string description, string defaultText, bool isModal = true, bool dispatchMain = false)
        {
#if __ANDROID__
            TaskCompletionSource<string> source = new TaskCompletionSource<string>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            MainActivity.RunOnUiThread(() =>
            {
                EditText editField = new EditText(MainActivity);
                editField.SetText(defaultText, TextView.BufferType.Editable);

                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity)!
                    .SetTitle(title)!
                    .SetMessage(description)!
                    .SetView(editField)!
                    .SetPositiveButton(Android.Resource.String.Ok, (dialog, which) =>
                    {
                        source.SetResult(editField.Text!);
                        (dialog as AlertDialog)!.Dismiss();
                    })!
                    .SetNegativeButton(Android.Resource.String.Cancel, (dialog, which) =>
                    {
                        source.SetResult(defaultText);
                        (dialog as AlertDialog)!.Dismiss();
                    })!;

                builder.Create()!.Show();
            });

            return source.Task;
        }
#else
            Func<Task<string>> returnTaskFunc = async () =>
            {
                var msgBox = MessageBoxManager.GetMessageBoxInputWindow(
                    new MessageBox.Avalonia.DTO.MessageBoxInputParams()
                    {
                        ContentTitle = title,
                        ContentMessage = description,
                        InputDefaultValue = defaultText,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }
                );

                var result = await (isModal ? msgBox.ShowDialog(MainWindow) : msgBox.Show());
                return result.Message;
            };

            if (dispatchMain)
            {
                return Dispatcher.UIThread.InvokeAsync(returnTaskFunc);
            }

            return returnTaskFunc();
        }
#endif
    }
}
