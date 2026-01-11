using System;
using System.Threading.Tasks;
using System.Diagnostics;
using WPR.Common;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace WPR.UI.Services
{
    public class RegistrationService
    {
        // Using a placeholder URL - the user should replace this with the actual registration URL
        private const string REGISTRATION_URL = "https://example.com/register";
        private const string CHECK_REGISTRATION_URL = "https://example.com/check-registration";
        
        public static async Task<bool> RegisterAsync(string email, string password)
        {
            try
            {
                Log.Info(LogCategory.Startup, $"Attempting to register user: {email}");
                
                using (var httpClient = new HttpClient())
                {
                    var userData = new
                    {
                        Email = email,
                        Password = password,
                        ProductKey = "WPR-0.0.15", // Placeholder product identifier
                        MachineId = Environment.MachineName // Basic machine identification
                    };
                    
                    var json = JsonConvert.SerializeObject(userData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    
                    var response = await httpClient.PostAsync(REGISTRATION_URL, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseContent);
                        
                        // Save registration token or credentials to configuration
                        Configuration.Current.RegistrationToken = result.token ?? "";
                        Configuration.Current.UserEmail = email;
                        Configuration.Current.IsRegistered = true;
                        Configuration.Current.Save();
                        
                        Log.Info(LogCategory.Startup, $"Registration completed for user: {email}");
                        return true;
                    }
                    else
                    {
                        Log.Error(LogCategory.Startup, $"Registration failed with status: {response.StatusCode}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.Startup, $"Registration failed: {ex.Message}");
                return false;
            }
        }
        
        public static async Task<bool> RegisterViaUrlAsync(string registrationUrl)
        {
            try
            {
                Log.Info(LogCategory.Startup, $"Attempting to register via URL: {registrationUrl}");
                
                // Extract registration parameters from the URL if needed
                var uri = new Uri(registrationUrl);
                var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
                
                var email = queryParameters["email"] ?? "";
                var token = queryParameters["token"] ?? "";
                
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(token))
                {
                    // Save the registration info
                    Configuration.Current.RegistrationToken = token;
                    Configuration.Current.UserEmail = email;
                    Configuration.Current.IsRegistered = true;
                    Configuration.Current.Save();
                    
                    Log.Info(LogCategory.Startup, $"Registration via URL completed: {registrationUrl}");
                    return true;
                }
                else
                {
                    Log.Warn(LogCategory.Startup, "Incomplete registration data in URL");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.Startup, $"Registration via URL failed: {ex.Message}");
                return false;
            }
        }
        
        public static async Task<bool> CheckRegistrationStatusAsync()
        {
            try
            {
                // Check if the application has been registered previously
                if (Configuration.Current.IsRegistered && !string.IsNullOrEmpty(Configuration.Current.RegistrationToken))
                {
                    // Optionally verify the registration status with the server
                    using (var httpClient = new HttpClient())
                    {
                        var registrationCheck = new
                        {
                            Token = Configuration.Current.RegistrationToken,
                            UserEmail = Configuration.Current.UserEmail,
                            MachineId = Environment.MachineName
                        };
                        
                        var json = JsonConvert.SerializeObject(registrationCheck);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        
                        // Make a request to verify registration status
                        var response = await httpClient.PostAsync(CHECK_REGISTRATION_URL, content);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            dynamic result = JsonConvert.DeserializeObject(responseContent);
                            
                            bool isValid = result.isValid ?? false;
                            
                            if (isValid)
                            {
                                Log.Info(LogCategory.Startup, "Registration status verified successfully");
                                return true;
                            }
                        }
                    }
                }
                
                Log.Info(LogCategory.Startup, "Application not registered or registration expired");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.Startup, $"Registration status check failed: {ex.Message}");
                return false;
            }
        }
    }
}