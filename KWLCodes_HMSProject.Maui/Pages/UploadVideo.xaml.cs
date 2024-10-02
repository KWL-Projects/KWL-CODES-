using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

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
            await AnimateButton(button);

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

                    // Verify file type (optional, as FilePickerFileType.Videos already filters video files)
                    if (IsValidVideoFile(filePath))
                    {
                        // Display success message
                        StatusLabel.Text = $"Success: Video selected from {filePath}";
                        LogEntry("Success", $"Video selected: {filePath}");

                        // Compress the video
                        await CompressVideo(filePath);
                    }
                    else
                    {
                        StatusLabel.Text = "Failure: Invalid video file type.";
                        LogEntry("Failure", "Invalid video file type selected.");
                    }
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
            await AnimateButton(button);

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

        private async Task CompressVideo(string filePath)
        {
            try
            {
                string outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), "compressed_" + Path.GetFileName(filePath));

                // Set the FFmpeg path
                FFmpeg.SetExecutablesPath("path_to_ffmpeg");

                // Compress the video with specific parameters
                var conversion = await FFmpeg.Conversions.New()
                    .AddParameter($"-i \"{filePath}\"")
                    .AddParameter("-c:v libx264")
                    .AddParameter("-b:v 1M")
                    .AddParameter("-c:a aac")
                    .AddParameter("-b:a 128k")
                    .SetOutput(outputFilePath)
                    .Start();

                // Display success message
                StatusLabel.Text = $"Success: Video compressed and saved to {outputFilePath}";
                LogEntry("Success", $"Video compressed: {outputFilePath}");
            }
            catch (Exception ex)
            {
                // Display failure message with detailed error
                StatusLabel.Text = $"Failure: Could not compress video. {ex.Message}";
                LogEntry("Failure", $"Could not compress video. Exception: {ex}");
            }
        }

        private bool IsValidVideoFile(string filePath)
        {
            var validExtensions = new[] { ".mp4", ".mov", ".avi" };
            return validExtensions.Contains(Path.GetExtension(filePath).ToLower());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);  // Slightly enlarge button
            await button.ScaleTo(1.0, 100);  // Return to normal size
        }

        private void LogEntry(string status, string message)
        {
            var logMessage = $"{DateTime.Now}: {status} - {message}";
            // Save logMessage to a log file or database
            Console.WriteLine(logMessage);
        }
    }
}
