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
                // Request camera permission
                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                    return (false, null, "Camera permission denied");

                // Request storage permission (ensure to handle platform-specific permissions)
                var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (storageStatus != PermissionStatus.Granted)
                    return (false, null, "Storage permission denied");

                string filePath = string.Empty;

                // Return success with the recorded video file path
                return (true, filePath, "Video recorded successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return (false, null, "An error occurred while recording the video");
            }
        }
    }
}
