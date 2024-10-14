using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using KWLCodes_HMSProject.Maui.Services; // Ensure you have this directive

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class UploadVideo : ContentPage
    {
        private readonly FilesService _filesService; // Injected FilesService

        public UploadVideo(FilesService filesService) // Constructor
        {
            InitializeComponent();
            _filesService = filesService;
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

                    if (IsValidVideoFile(filePath))
                    {
                        StatusLabel.Text = $"Success: Video selected from {filePath}";
                        LogEntry("Success", $"Video selected: {filePath}");

                        await CompressVideo(filePath); // Call to compress video
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

                LogEntry("Info", $"Input file path: {filePath}");
                LogEntry("Info", $"Output file path: {outputFilePath}");

                // Compressing the video using FFMpeg
                await FFMpegArguments
                    .FromFileInput(filePath)
                    .OutputToFile(outputFilePath, true, options => options
                        .WithVideoCodec("libx264")
                        .WithVideoBitrate(1000)
                        .WithAudioCodec("aac")
                        .WithAudioBitrate(128))
                    .ProcessAsynchronously();

                StatusLabel.Text = $"Success: Video compressed and saved to {outputFilePath}";
                LogEntry("Success", $"Video compressed: {outputFilePath}");

                // Uploading the compressed video
                using (var fileStream = File.OpenRead(outputFilePath))
                {
                    var fileUrl = await _filesService.UploadFileAsync(fileStream, Path.GetFileName(outputFilePath));
                    if (fileUrl != null)
                    {
                        StatusLabel.Text += $"\nVideo uploaded successfully: {fileUrl}";
                        LogEntry("Success", $"Video uploaded: {fileUrl}");
                    }
                    else
                    {
                        StatusLabel.Text += "\nFailure: Video upload failed.";
                        LogEntry("Failure", "Video upload failed.");
                    }
                }
            }
            catch (Exception ex)
            {
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
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }

        private void LogEntry(string status, string message)
        {
            var logMessage = $"{DateTime.Now}: {status} - {message}";
            Console.WriteLine(logMessage);
        }
    }
}
