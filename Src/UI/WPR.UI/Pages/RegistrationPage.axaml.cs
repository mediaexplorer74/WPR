using Avalonia.Controls;
using Avalonia.Interactivity;
using WPR.Common;
using WPR.UI.Services;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using System;

namespace WPR.UI.Pages
{
    public partial class RegistrationPage : UserControl
    {
        public RegistrationPage()
        {
            InitializeComponent();
            
            RegisterButton.Click += OnRegisterButtonClick;
            VerifyRegistrationButton.Click += OnVerifyRegistrationButtonClick;
            
            // Load existing registration info if available
            LoadExistingRegistrationInfo();
        }
        
        private void LoadExistingRegistrationInfo()
        {
            var emailTextBox = this.Get<TextBox>("EmailTextBox");
            var regKeyTextBox = this.Get<TextBox>("RegistrationKeyTextBox");
            var statusTextBlock = this.Get<TextBlock>("StatusTextBlock");
            
            if (!string.IsNullOrEmpty(Configuration.Current?.UserEmail))
            {
                emailTextBox.Text = Configuration.Current.UserEmail;
            }
            
            if (!string.IsNullOrEmpty(Configuration.Current?.RegistrationToken))
            {
                regKeyTextBox.Text = Configuration.Current.RegistrationToken;
            }
            
            if (Configuration.Current?.IsRegistered == true)
            {
                statusTextBlock.Text = "Status: Registered";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Green;
            }
            else
            {
                statusTextBlock.Text = "Status: Not Registered";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
            }
        }
        
        private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var emailTextBox = this.Get<TextBox>("EmailTextBox");
            var regKeyTextBox = this.Get<TextBox>("RegistrationKeyTextBox");
            var statusTextBlock = this.Get<TextBlock>("StatusTextBlock");
            
            string email = emailTextBox.Text;
            string registrationInput = regKeyTextBox.Text;
            
            if (string.IsNullOrWhiteSpace(email))
            {
                statusTextBlock.Text = "Please enter a valid email address.";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
                return;
            }
            
            bool success = false;
            
            // Check if the input looks like a registration URL
            if (Uri.IsWellFormedUriString(registrationInput, UriKind.Absolute))
            {
                // Try to register via URL
                success = await RegistrationService.RegisterViaUrlAsync(registrationInput);
            }
            else if (!string.IsNullOrWhiteSpace(registrationInput))
            {
                // Try to register with email and registration key
                success = await RegistrationService.RegisterAsync(email, registrationInput);
            }
            
            if (success)
            {
                statusTextBlock.Text = "Registration successful!";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Green;
                
                // Show success message
                var msgBox = MessageBoxManager.GetMessageBoxStandardWindow(
                    title: "Registration Success",
                    text: "Your WPR account has been successfully registered!",
                    icon: Icon.Success,
                    windowStartupLocation: WindowStartupLocation.CenterOwner);
                    
                await msgBox.ShowDialog(GetWindow());
            }
            else
            {
                statusTextBlock.Text = "Registration failed. Please check your details and try again.";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
            }
        }
        
        private async void OnVerifyRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            var statusTextBlock = this.Get<TextBlock>("StatusTextBlock");
            
            bool isRegistered = await RegistrationService.CheckRegistrationStatusAsync();
            
            if (isRegistered)
            {
                statusTextBlock.Text = "Status: Registered and verified";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Green;
            }
            else
            {
                statusTextBlock.Text = "Status: Not registered or verification failed";
                statusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
            }
        }
        
        private Window? GetWindow() => VisualRoot as Window;
    }
}