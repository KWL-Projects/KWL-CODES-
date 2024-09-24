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
                    LogFileEntry($"Success: Video selected: {filePath}");

                    // Here you can implement logic to use the selected video using filePath
                }
            }
            catch (Exception ex)
            {
                // Display failure message
                StatusLabel.Text = "Failure: Could not select video.";
                LogFileEntry($"Failure: {ex.Message}");
            }
        }

        private async void OnRecordVideoClicked(object sender, EventArgs e)
        {
            try
            {
                // Check and request camera permission
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }

                // Check and request storage permission (for saving video)
                var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }

                // If permissions are granted, proceed with video recording
                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    var result = await MediaPicker.CaptureVideoAsync(new MediaPickerOptions
                    {
                        Title = "Record a Video"
                    });

                    if (result != null)
                    {
                        string filePath = result.FullPath;

                        // Display success message
                        StatusLabel.Text = $"Success: Video recorded at {filePath}";
                        LogFileEntry($"Success: Video recorded: {filePath}");

                        // Here you can implement logic to use the recorded video using filePath
                    }
                }
                else
                {
                    // Display permission denial message
                    StatusLabel.Text = "Permission denied to access camera or storage.";
                }
            }
            catch (Exception ex)
            {
                // Display failure message
                StatusLabel.Text = "Failure: Could not record video.";
                LogFileEntry($"Failure: {ex.Message}");
            }
        }

        private void LogFileEntry(string message)
        {
            // Implement logging logic here (e.g., write to a file, database, etc.)
            Console.WriteLine(message);
        }
    }
}
