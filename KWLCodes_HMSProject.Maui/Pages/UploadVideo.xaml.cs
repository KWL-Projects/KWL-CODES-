using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class UploadVideo : ContentPage
    {
        public UploadVideo()
        {
            InitializeComponent();
        }

        private async void OnSelectVideoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await button.ScaleTo(1.1, 100);  // Slightly enlarge button
            await button.ScaleTo(1.0, 100);

            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a Video",
                    FileTypes = FilePickerFileType.Videos
                });

                if (result != null)
                {
                    string filePath = result.FullPath;

                    // Display success message
                    StatusLabel.Text = $"Success: Video selected from {filePath}";
                    LogEntry("Success", $"Video selected: {filePath}");

                    // Here you can implement logic to use the selected video using filePath
                }
            }
            catch (Exception ex)
            {
                // Display failure message
                StatusLabel.Text = "Failure: Could not select video.";
                LogEntry("Failure", ex.Message);
            }
        }

        private async void OnRecordVideoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await button.ScaleTo(1.1, 100);  // Slightly enlarge button
            await button.ScaleTo(1.0, 100);

            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var video = await MediaPicker.Default.CaptureVideoAsync();
                    if (video != null)
                    {
                        var filePath = video.FullPath;
                        // Handle the captured video file
                        StatusLabel.Text = "Status: Video recorded successfully!";
                        LogEntry("Success", $"Video recorded: {filePath}");
                    }
                }
                else
                {
                    StatusLabel.Text = "Status: Video capture not supported on this device.";
                    LogEntry("Failure", "Video capture not supported.");
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Status: An error occurred: {ex.Message}";
                LogEntry("Failure", ex.Message);
            }
        }

        private void LogEntry(string status, string message)
        {
            var logMessage = $"{DateTime.Now}: {status} - {message}";
            // Save logMessage to a log file or database
            Console.WriteLine(logMessage);
        }
    }
}
