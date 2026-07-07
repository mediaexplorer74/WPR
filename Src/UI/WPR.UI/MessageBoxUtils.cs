using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Widget;
  #else
  using Avalonia.Controls;
  using Avalonia.Threading;
  using Avalonia.Layout;
  using Avalonia.Media;
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

        public sealed class AndroidInstallProgress
        {
            private readonly ProgressDialog _dialog;

            public AndroidInstallProgress(string message)
            {
                _dialog = new ProgressDialog(MainActivity)
                {
                    Indeterminate = false
                };
                _dialog.SetMessage(message);
                _dialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
                _dialog.SetCancelable(false);
                _dialog.Max = 100;
            }

            public void Show()
            {
                MainActivity.RunOnUiThread(() => _dialog.Show());
            }

            public void SetProgress(int percent)
            {
                MainActivity.RunOnUiThread(() => _dialog.Progress = Math.Clamp(percent, 0, 100));
            }

            public void Dismiss()
            {
                MainActivity.RunOnUiThread(() =>
                {
                    if (_dialog.IsShowing)
                    {
                        _dialog.Dismiss();
                    }
                });
            }
        }
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
                catch (System.MissingMethodException)
                {
                    var tcs = new TaskCompletionSource<MessageBox.Avalonia.Enums.ButtonResult>(TaskCreationOptions.RunContinuationsAsynchronously);
                    
                    var w = new Window
                    {
                        Title = title,
                        Width = 420,
                        Height = 200,
                        CanResize = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    
                    var panel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Spacing = 12,
                        Margin = new Thickness(16)
                    };
                    
                    var content = new TextBlock
                    {
                        Text = text,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.White
                    };
                    
                    var buttonsPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                        Spacing = 8
                    };
                    
                    void CloseWith(MessageBox.Avalonia.Enums.ButtonResult r)
                    {
                        tcs.TrySetResult(r);
                        w.Close();
                    }
                    
                    void AddButton(string caption, MessageBox.Avalonia.Enums.ButtonResult r)
                    {
                        var b = new Button { Content = caption, MinWidth = 80 };
                        b.Click += (_, __) => CloseWith(r);
                        buttonsPanel.Children.Add(b);
                    }
                    
                    switch (buttons)
                    {
                        case MessageBox.Avalonia.Enums.ButtonEnum.Ok:
                            AddButton("OK", MessageBox.Avalonia.Enums.ButtonResult.Ok);
                            break;
                        case MessageBox.Avalonia.Enums.ButtonEnum.OkCancel:
                            AddButton("Cancel", MessageBox.Avalonia.Enums.ButtonResult.Cancel);
                            AddButton("OK", MessageBox.Avalonia.Enums.ButtonResult.Ok);
                            break;
                        case MessageBox.Avalonia.Enums.ButtonEnum.YesNo:
                            AddButton("No", MessageBox.Avalonia.Enums.ButtonResult.No);
                            AddButton("Yes", MessageBox.Avalonia.Enums.ButtonResult.Yes);
                            break;
                        case MessageBox.Avalonia.Enums.ButtonEnum.YesNoCancel:
                            AddButton("Cancel", MessageBox.Avalonia.Enums.ButtonResult.Cancel);
                            AddButton("No", MessageBox.Avalonia.Enums.ButtonResult.No);
                            AddButton("Yes", MessageBox.Avalonia.Enums.ButtonResult.Yes);
                            break;
                        default:
                            AddButton("OK", MessageBox.Avalonia.Enums.ButtonResult.Ok);
                            break;
                    }
                    
                    panel.Children.Add(content);
                    panel.Children.Add(buttonsPanel);
                    w.Content = panel;
                    
                    if (modalOnWindow && MainWindow != null)
                    {
                        _ = w.ShowDialog(MainWindow);
                    }
                    else
                    {
                        w.Show();
                    }
                    
                    return tcs.Task;
                }
            };
            
            if (dispatchMain)
            {
                return Dispatcher.UIThread.InvokeAsync(returnTaskFunc);
            }

            return returnTaskFunc();
#endif
        }

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
                try
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
                }
                catch (System.MissingMethodException)
                {
                    var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                    
                    var w = new Window
                    {
                        Title = title,
                        Width = 420,
                        Height = 220,
                        CanResize = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    
                    var panel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Spacing = 10,
                        Margin = new Thickness(16)
                    };
                    
                    var desc = new TextBlock { Text = description, TextWrapping = TextWrapping.Wrap, Foreground = Brushes.White };
                    var input = new TextBox { Text = defaultText };
                    
                    var buttons = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                        Spacing = 8
                    };
                    
                    var btnCancel = new Button { Content = "Cancel", MinWidth = 80 };
                    btnCancel.Click += (_, __) => { tcs.TrySetResult(defaultText); w.Close(); };
                    
                    var btnOk = new Button { Content = "OK", MinWidth = 80 };
                    btnOk.Click += (_, __) => { tcs.TrySetResult(input.Text ?? defaultText); w.Close(); };
                    
                    buttons.Children.Add(btnCancel);
                    buttons.Children.Add(btnOk);
                    
                    panel.Children.Add(desc);
                    panel.Children.Add(input);
                    panel.Children.Add(buttons);
                    w.Content = panel;
                    
                    if (isModal && MainWindow != null)
                    {
                        _ = w.ShowDialog(MainWindow);
                    }
                    else
                    {
                        w.Show();
                    }
                    
                    return await tcs.Task;
                }
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
