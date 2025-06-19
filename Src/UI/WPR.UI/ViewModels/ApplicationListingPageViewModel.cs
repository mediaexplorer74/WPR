using ReactiveUI;
using System.Threading.Tasks;
using WPR.Models;
using WPR.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Reactive;
using Avalonia.Threading;
using System;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;

namespace WPR.UI.ViewModels
{
    public class ApplicationListingPageViewModel : ViewModelBase
    {
        internal delegate void OnProgressNeedSet(int value);
        internal event OnProgressNeedSet? InstallationSetProgress;

        private string _SearchText;
        private ObservableCollection<ApplicationItemViewModel> _Applications;
        private ApplicationItemViewModel? _ChoosenApp;

        public ReactiveCommand<Stream, ApplicationInstallError> InstallRequestCommand;

        public ReactiveCommand<string, Unit> AppSearchCommand;


        public ReactiveCommand<ApplicationItemViewModel?, Unit> RunAppCommand;

        public Interaction<Application, bool> DeleteExistingAppInteraction;

        public ReactiveCommand<ApplicationItemViewModel?, Unit> DeleteAppCommand;
        

        public string SearchText
        {
            get { return _SearchText; }
            set { this.RaiseAndSetIfChanged(ref _SearchText, value); }
        }

        public ApplicationItemViewModel? ChoosenApp
        {
            get { return _ChoosenApp; }
            set
            {
                // Retain the selected app for context menu actions
                // and only launch if it's a new selection or a direct action.
                // The actual launch logic will be handled by the 'Run' menu item command.
                this.RaiseAndSetIfChanged(ref _ChoosenApp, value);
            }
        }

        public CancellationTokenSource? CancelSource { get; set; }

        public ObservableCollection<ApplicationItemViewModel> Applications {
            get { return _Applications; }
            set { this.RaiseAndSetIfChanged(ref _Applications, value); }
        }

        public void UpdateApplicationList(string text)
        {
            try
            {
                var enumerable = ApplicationContext.Current.Applications!
                        .Where(app => app.Name.ToLower().Contains((text != null) 
                        ? text.ToLower() : ""))
                        .OrderBy(app => app.Name.ToLower())
                        .Select(app => new ApplicationItemViewModel(app))
                        .AsEnumerable();

                _ChoosenApp = null;

                // So it can hear change. Replace the ref only does
                // not make it refresh display
                Applications = 
                    new ObservableCollection<ApplicationItemViewModel>(enumerable);
                
                // Subscribe to UninstallRequested event for each ApplicationItemViewModel
                foreach (var appItem in Applications)
                {
                    appItem.UninstallRequested += OnAppUninstallRequested;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine( $"[ex] WPR.UI - ApplicationListingViewModel: " +
                    $"Unable to query application database with exception:\n {ex}");
                Log.Error(LogCategory.AppList,
                    $"Unable to query application database with exception:\n {ex}");
                Applications = new ObservableCollection<ApplicationItemViewModel>();
            }
        }

        // Handler for the UninstallRequested event
        private void OnAppUninstallRequested(object? sender, ApplicationItemViewModel appItem)
        {
            // Call the DeleteApplicationAsync method to uninstall the app
            _ = DeleteApplicationAsync(appItem);
        }

        public void UpdateApplications()
        {
            UpdateApplicationList(SearchText);
        }

        public ApplicationListingPageViewModel()
        {
            _SearchText = "";
            Applications = new ObservableCollection<ApplicationItemViewModel>();
            DeleteExistingAppInteraction = new Interaction<Application, bool>();

            // In ApplicationListingPageViewModel constructor
            var deleteEnabled = this.WhenAnyValue(x => x.ChoosenApp).Select(x => x != null);
            DeleteAppCommand = ReactiveCommand.CreateFromTask<ApplicationItemViewModel>(DeleteApplicationAsync, deleteEnabled);
            RunAppCommand = ReactiveCommand.CreateFromTask<ApplicationItemViewModel>(RunApplicationAsync, deleteEnabled);
            
            InstallRequestCommand = ReactiveCommand.CreateFromTask<Stream, ApplicationInstallError>(
                async fileStream =>
                {
                    CancelSource = new CancellationTokenSource();

                    return await ApplicationInstaller.Install(fileStream,
                        progressValue => InstallationSetProgress!.Invoke(progressValue),
                        app => DeleteExistingAppInteraction!.Handle(app),
                        CancelSource.Token);
                });

            this.WhenAnyValue(v => v.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(20))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(text => UpdateApplicationList(text));
        }
            
            private async Task RunApplicationAsync(ApplicationItemViewModel app) {
               ApplicationLaunchRequest.Ask(app.Model);
            }
            
            private async Task DeleteApplicationAsync(ApplicationItemViewModel app) {
                // Optional: Show confirmation dialog here before uninstalling
                ApplicationContext.Current.Applications.Remove(app.Model);
                await ApplicationContext.Current.SaveChangesAsync(); // Persist the uninstall
                Applications.Remove(app);
                UpdateApplications();
            }
    }
}
