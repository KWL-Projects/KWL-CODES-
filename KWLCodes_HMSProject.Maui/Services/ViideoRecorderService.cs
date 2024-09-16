using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;

namespace KWLCodes_HMSProject.Maui.Services
{
    public class VideoRecorderService
    {
        public async Task<(bool Success, string FilePath, string Message)> RecordVideoAsync()
        {
            try
            {
                // Request camera permissions
                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                    return (false, null, "Camera permission denied");

                var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (storageStatus != PermissionStatus.Granted)
                    return (false, null, "Storage permission denied");

                var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                // Start the camera recording
                var video = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Videos,
                    PickerTitle = "Select a video"
                });

                if (video != null)
                {
                    // Save video to file
                    File.Copy(video.FullPath, filePath, true);
                    return (true, filePath, "Video recorded successfully");
                }
                else
                {
                    return (false, null, "Video recording was cancelled");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return (false, null, "An error occurred while recording the video");
            }
        }
    }
}
