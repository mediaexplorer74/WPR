/*using Avalonia.Controls;
using Avalonia.ReactiveUI;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using Avalonia.Threading;
using MessageBox.Avalonia;
using System.Reactive.Linq;
using WPR.Models;
using WPR.Common;
using Avalonia.Platform.Storage;
using DialogHostAvalonia;

namespace WPR.UI.Pages
{
    public partial class ApplicationListingPage : ReactiveUserControl<ApplicationListingPageViewModel>
    {
        private List<FilePickerFileType> AppInstallFileFilters;

        public ApplicationListingPage()
        {
            InitializeComponent();
            DataContext = new ApplicationListingPageViewModel();

            AppInstallFileFilters = new List<FilePickerFileType>
            {
                new FilePickerFileType("XAP file")
                {
                    Patterns = new List<string> { "*.xap" }
                },
                new FilePickerFileType("All files")
                {
                    Patterns = new List<string> { "*.*" }
                }
            };

            this.Get<Button>("addNewAppButton").Click += async delegate
            {
                var result = await GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "Choose XAP file",
                    FileTypeFilter = AppInstallFileFilters
                });

                if ((result != null) && (result.Count >= 1))
                {
                    var InstallProgressWindow = new ProgressView();
                    InstallProgressWindow.CancelRequested += obj => ViewModel!.CancelSource!.Cancel();

                    ViewModel!.InstallationSetProgress += progress => Dispatcher.UIThread.InvokeAsync(()
                        => InstallProgressWindow.Progress = progress);
                    
                    ViewModel!.DeleteExistingAppInteraction!.RegisterHandler(
                        context => Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        {
                            Application app = context.Input;

                            MessageBox.Avalonia.Enums.ButtonResult result = 
                              await MessageBoxUtils.GetMessageDialogResult(
                                title: Properties.Resources.ApplicationAlreadyInstalled,
                                text: String.Format(
                                    Properties.Resources.ApplicationAlreadyInstalledDescription, app.Name),
                                icon: MessageBox.Avalonia.Enums.Icon.Question,
                                buttons: MessageBox.Avalonia.Enums.ButtonEnum.YesNo);

                            context.SetOutput(result == MessageBox.Avalonia.Enums.ButtonResult.Yes);
                        }
                    }));

                    InstallProgressWindow.WhenAnyValue(v => v.IsVisible)
                    .Subscribe
                    (async v => 
                    {
                        if (!v)
                        {
                            return;
                        }

                        var err = await ViewModel!.InstallRequestCommand.Execute(
                            await result[0].OpenReadAsync());
                            
                        string errUserStr = LocaleUtils.GetDisplayName(err);
                        bool failed = err != ApplicationInstallError.None;

                        await MessageBoxUtils.GetMessageDialogResult(
                            title: failed ? Properties.Resources.InstallationFailed : Properties.Resources.InstallationSucceed,
                            text: errUserStr,
                            icon: failed ? MessageBox.Avalonia.Enums.Icon.Error : MessageBox.Avalonia.Enums.Icon.Success);

                        ViewModel!.UpdateApplicationList(ViewModel!.SearchText);

                        DialogHost.Close(null);
                    });

                    await DialogHost.Show(InstallProgressWindow);
                }
            };
        }

        Window GetWindow()
        {
            return VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
        }

        TopLevel GetTopLevel()
        {
            return VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        }

        IStorageProvider GetStorageProvider()
        {
            return GetTopLevel().StorageProvider;
        }
    }
}*/

using Avalonia.Controls;
using Avalonia.ReactiveUI;
using WPR.UI.ViewModels;
using WPR.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using Avalonia.Threading;
using MessageBox.Avalonia;
using System.Reactive.Linq;
using WPR.Models;
using WPR.Common;
using Avalonia.Platform.Storage;
using DialogHostAvalonia;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace WPR.UI.Pages
{
    public partial class ApplicationListingPage : ReactiveUserControl<ApplicationListingPageViewModel>
    {
        private List<FilePickerFileType> AppInstallFileFilters;

        public ApplicationListingPage()
        {
            InitializeComponent();
            DataContext = new ApplicationListingPageViewModel();

            AppInstallFileFilters = new List<FilePickerFileType>
            {
                new FilePickerFileType("XAP file")
                {
                    Patterns = new List<string> { "*.xap" }
                },
                new FilePickerFileType("All files")
                {
                    Patterns = new List<string> { "*.*" }
                }
            };

            this.Get<Button>("addNewAppButton").Click += AddNewAppButton_Click;

            var appListBox = this.Get<ListBox>("appListBox");
            appListBox.DoubleTapped += (_, _) =>
            {
                if (ViewModel?.ChoosenApp != null)
                {
                    ApplicationLaunchRequest.Ask(ViewModel.ChoosenApp.Model);
                }
            };
        }

        private async void AddNewAppButton_Click(object? sender, RoutedEventArgs e)
        {
            var result = await GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose XAP file",
                FileTypeFilter = AppInstallFileFilters
            });

            if ((result != null) && (result.Count >= 1))
            {
#if __ANDROID__
                await InstallXapOnAndroidAsync(result[0]);
#else
                await InstallXapWithDialogHostAsync(result[0]);
#endif
            }
        }

#if __ANDROID__
        private async Task InstallXapOnAndroidAsync(IStorageFile file)
        {
            var progress = new MessageBoxUtils.AndroidInstallProgress(Properties.Resources.InstallingApp);
            progress.Show();

            ViewModel!.CancelSource = new CancellationTokenSource();
            void OnProgress(int value) => progress.SetProgress(value);
            ViewModel.InstallationSetProgress += OnProgress;

            using var deleteExistingReg = ViewModel.DeleteExistingAppInteraction!.RegisterHandler(context =>
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    Application app = context.Input;
                    var msgResult = await MessageBoxUtils.GetMessageDialogResult(
                        title: Properties.Resources.ApplicationAlreadyInstalled,
                        text: string.Format(Properties.Resources.ApplicationAlreadyInstalledDescription, app.Name),
                        icon: MessageBox.Avalonia.Enums.Icon.Question,
                        buttons: MessageBox.Avalonia.Enums.ButtonEnum.YesNo);
                    context.SetOutput(msgResult == MessageBox.Avalonia.Enums.ButtonResult.Yes);
                }));

            try
            {
                await using var stream = await file.OpenReadAsync();
                var err = await ViewModel.InstallRequestCommand.Execute(stream);

                string errUserStr = LocaleUtils.GetDisplayName(err);
                bool failed = err != ApplicationInstallError.None;

                await MessageBoxUtils.GetMessageDialogResult(
                    title: failed ? Properties.Resources.InstallationFailed : Properties.Resources.InstallationSucceed,
                    text: errUserStr,
                    icon: failed ? MessageBox.Avalonia.Enums.Icon.Error : MessageBox.Avalonia.Enums.Icon.Success);

                ViewModel.UpdateApplicationList(ViewModel.SearchText);
            }
            finally
            {
                ViewModel.InstallationSetProgress -= OnProgress;
                progress.Dismiss();
            }
        }
#else
        private async Task InstallXapWithDialogHostAsync(IStorageFile file)
        {
            var InstallProgressWindow = new ProgressView();

            WPR.UI.Views.ProgressView.OnCancelRequested? cancelHandler = args => ViewModel!.CancelSource!.Cancel();
            InstallProgressWindow.CancelRequested += cancelHandler;

            WPR.UI.ViewModels.ApplicationListingPageViewModel.OnProgressNeedSet? progressHandler =
                progress => Dispatcher.UIThread.InvokeAsync(() => InstallProgressWindow.Progress = progress);
            ViewModel!.InstallationSetProgress += progressHandler;

            IDisposable? deleteExistingReg = null;
            deleteExistingReg = ViewModel!.DeleteExistingAppInteraction!.RegisterHandler(context =>
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    Application app = context.Input;

                    MessageBox.Avalonia.Enums.ButtonResult msgResult = await MessageBoxUtils.GetMessageDialogResult(
                        title: Properties.Resources.ApplicationAlreadyInstalled,
                        text: string.Format(Properties.Resources.ApplicationAlreadyInstalledDescription, app.Name),
                        icon: MessageBox.Avalonia.Enums.Icon.Question,
                        buttons: MessageBox.Avalonia.Enums.ButtonEnum.YesNo);

                    context.SetOutput(msgResult == MessageBox.Avalonia.Enums.ButtonResult.Yes);
                }));

            IDisposable? progressSub = null;
            progressSub = InstallProgressWindow.WhenAnyValue(v => v.IsVisible)
                .Subscribe(async v =>
                {
                    if (!v)
                    {
                        progressSub?.Dispose();
                        return;
                    }

                    await using var stream = await file.OpenReadAsync();
                    var err = await ViewModel!.InstallRequestCommand.Execute(stream);

                    string errUserStr = LocaleUtils.GetDisplayName(err);
                    bool failed = err != ApplicationInstallError.None;

                    await MessageBoxUtils.GetMessageDialogResult(
                        title: failed ? Properties.Resources.InstallationFailed : Properties.Resources.InstallationSucceed,
                        text: errUserStr,
                        icon: failed ? MessageBox.Avalonia.Enums.Icon.Error : MessageBox.Avalonia.Enums.Icon.Success);

                    ViewModel!.UpdateApplicationList(ViewModel!.SearchText);
                    DialogHost.Close(null);
                });

            await DialogHost.Show(InstallProgressWindow);

            try
            {
                progressSub?.Dispose();
                deleteExistingReg?.Dispose();
                if (cancelHandler != null) InstallProgressWindow.CancelRequested -= cancelHandler;
                if (progressHandler != null) ViewModel!.InstallationSetProgress -= progressHandler;
            }
            catch { }
        }
#endif

        Window GetWindow() => VisualRoot as Window ?? throw new NullReferenceException("Invalid Owner");
        TopLevel GetTopLevel() => VisualRoot as TopLevel ?? throw new NullReferenceException("Invalid Owner");
        IStorageProvider GetStorageProvider() => GetTopLevel().StorageProvider;
    }
}
