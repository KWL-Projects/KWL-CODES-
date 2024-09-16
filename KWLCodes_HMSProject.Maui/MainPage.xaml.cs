using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;
using Microsoft.Maui.ApplicationModel;

namespace KWLCodes_HMSProject.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly VideoRecorderService _videoRecorderService;

        public MainPage()
        {
            InitializeComponent();
            _videoRecorderService = new VideoRecorderService();
        }

        // Request camera and storage permissions
        private async Task<bool> RequestPermissionsAsync()
        {
            // Request camera permission
            var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            if (cameraStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Camera permission is required to record video.", "OK");
                return false;
            }

            // Request storage permission
            var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (storageStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Storage permission is required to save video.", "OK");
                return false;
            }

            return true;
        }

        // Triggered when the Record Video button is clicked
        private async void OnRecordVideoClicked(object sender, EventArgs e)
        {
            // Ensure permissions are granted
            bool permissionsGranted = await RequestPermissionsAsync();
            if (!permissionsGranted)
            {
                StatusLabel.Text = "Permissions were not granted.";
                return;
            }

            // Record the video and handle the result
            var result = await _videoRecorderService.RecordVideoAsync();

            if (result.Success)
            {
                StatusLabel.Text = $"Success: {result.Message}\nFile Path: {result.FilePath}";
            }
            else
            {
                StatusLabel.Text = $"Failure: {result.Message}";
            }

            // Log entry (optional)
            System.Diagnostics.Debug.WriteLine($"Log Entry: {result.Message}");
        }
    }
}
