//Waldo Branch

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;
using KWLCodes_HMSProject.Maui;
//using Microsoft.Maui.Essentials; // or the specific Maui class you're using



namespace Test_Waldo.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

    namespace VideoPickerApp
    {
        public partial class MainPage : ContentPage
        {
            public MainPage()
            {
                object value = InitializeComponent();
            }

            private async void OnPickVideoButtonClicked(object sender, EventArgs e)
            {
                try
                {
                    // Open file picker
                    var result = await FilePicker.PickAsync(new PickOptions
                    {
                        PickerTitle = "Select a Video",
                        FileTypes = FilePickerFileType.Videos
                    });

                    if (result != null)
                    {
                        // Verify file type
                        if (result.FileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith(".mov", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith(".avi", StringComparison.OrdinalIgnoreCase))
                        {
                            // Capture the file path
                            var filePath = result.FullPath;

                            // Display success message
                            await DisplayAlert("Success", "Video selected successfully!", "OK");

                            // Store file path (you can adjust where and how you store this)
                            StoreFilePath(filePath);

                            // Log success
                            LogMessage("Video selected successfully: " + filePath);
                        }
                        else
                        {
                            // Display failure message
                            await DisplayAlert("Error", "Selected file is not a supported video format.", "OK");

                            // Log failure
                            LogMessage("Unsupported file type selected: " + result.FileName);
                        }
                    }
                    else
                    {
                        // Handle case where no file was selected
                        await DisplayAlert("Error", "No file selected.", "OK");

                        // Log failure
                        LogMessage("No file selected.");
                    }
                }
                catch (Exception ex)
                {
                    // Display and log any errors that occur
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                    LogMessage("Error occurred: " + ex.Message);
                }
            }

            private void StoreFilePath(string filePath)
            {
                // Implementation to store the file path, could be a database, local storage, etc.
                // Example: Preferences.Set("SelectedVideoPath", filePath);
            }

            private void LogMessage(string message)
            {
                // Log the message, could be to a file or an external logging service
                string logFilePath = Path.Combine(FileSystem.AppDataDirectory, "log.txt");
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
            }
        
        public class VideoUploader
        {
            private readonly HttpClient _httpClient;

            public VideoUploader()
            {
                _httpClient = new HttpClient();
            }

            public async Task UploadVideoAsync(string filePath)
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    Log("Failure: File path is invalid or file does not exist.");
                    return;
                }

                try
                {
                    // Open a connection to the API
                    var apiUrl = "https://your-api-endpoint.com/upload";
                    using var content = new MultipartFormDataContent();
                    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    var streamContent = new StreamContent(fileStream);
                    content.Add(streamContent, "file", Path.GetFileName(filePath));

                    // Upload the file
                    var response = await _httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Log("Success: Video uploaded successfully.");
                    }
                    else
                    {
                        Log($"Failure: Video upload failed with status code {response.StatusCode}.");
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failure: Exception occurred while uploading video. Error: {ex.Message}");
                }
                finally
                {
                    // Close connection to the server
                    _httpClient.Dispose();
                }
            }

            private void Log(string message)
            {
                // Append message to a log file
                var logFilePath = Path.Combine(FileSystem.AppDataDirectory, "upload_log.txt");
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
        }
    }
    namespace VideoStreamer
    {
        public partial class MainPage : ContentPage
        {
            private const string ServerUrl = "https://yourserver.com/api/videostream"; // Replace with your server URL
            private const string LogFilePath = "stream_log.txt";

            public object VideoWebView { get; private set; }

            /*public MainPage()
            {
                InitializeComponent();
            }*/

            private async void OnSelectVideoButtonClicked(object sender, EventArgs e)
            {
                try
                {
                    var videoFile = await PickVideoFileAsync();
                    if (videoFile != null)
                    {
                        await StreamVideoAsync(videoFile);
                    }
                }
                catch (Exception ex)
                {
                    await LogAsync($"Failure: {ex.Message}");
                    await DisplayAlert("Error", "Failed to stream video.", "OK");
                }
            }

            private async Task<FileResult> PickVideoFileAsync()
            {
                try
                {
                    var options = new PickOptions
                    {
                        FileTypes = FilePickerFileType.Videos,
                        PickerTitle = "Select a video file to stream"
                    };

                    return await FilePicker.Default.PickAsync(options);
                }
                catch (Exception ex)
                {
                    await LogAsync($"Failure picking video: {ex.Message}");
                    return null;
                }
            }

            private async Task StreamVideoAsync(FileResult videoFile)
            {
                if (videoFile is null)
                {
                    throw new ArgumentNullException(nameof(videoFile));
                }

                try
                {
                    using var httpClient = new HttpClient();
                    using var content = new MultipartFormDataContent();
                    using var fileStream = await videoFile.OpenReadAsync();
                    using var streamContent = new StreamContent(fileStream);

                    content.Add(streamContent, "video", videoFile.FileName);

                    var response = await httpClient.PostAsync(ServerUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string videoUrl = await response.Content.ReadAsStringAsync();

                        //ERROT MOET NOG FIX
                        //VideoWebView.Source = videoUrl;

                        await LogAsync("Success: Video streamed successfully.");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Server error: {error}");
                    }
                }
                catch (Exception ex)
                {
                    await LogAsync($"Failure streaming video: {ex.Message}");
                    throw;
                }
            }

            private async Task LogAsync(string message)
            {
                var logMessage = $"{DateTime.Now}: {message}\n";
                string fullPath = Path.Combine(FileSystem.AppDataDirectory, LogFilePath);
                await File.AppendAllTextAsync(fullPath, logMessage);
            }
        }
    }

    public class UserAdministration
    {
        private string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app-log.txt");

        public string UpdateUserInformation(UserInfo userInfo)
        {
            try
            {
                // Simulate updating user information in the database or other storage.
                bool updateSuccessful = UpdateUserInDatabase(userInfo);

                if (updateSuccessful)
                {
                    // Log success message
                    LogMessage($"User information for {userInfo.UserName} updated successfully.");

                    // Return success message
                    return "User information updated successfully.";
                }
                else
                {
                    // Log failure message
                    LogMessage($"Failed to update user information for {userInfo.UserName}.");

                    // Return failure message
                    return "Failed to update user information.";
                }
            }
            catch (Exception ex)
            {
                // Log exception message
                LogMessage($"Exception occurred while updating user information: {ex.Message}");

                // Return failure message
                return "An error occurred while updating user information.";
            }
        }

        private bool UpdateUserInDatabase(UserInfo userInfo)
        {
            // Simulate database update process.
            // This is where you would include actual database update logic.
            // Return true if the update is successful, otherwise return false.

            // For demonstration purposes, let's assume the update is successful.
            return true;
        }

        private void LogMessage(string message)
        {
            try
            {
                // Write log entry to the log file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // Handle logging failure (you might want to show a message or take other action)
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        // Add other user-related properties as needed
    }


}
