using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui
{
    public partial class VideoRecordingPage : ContentPage
    {
        public VideoRecordingPage()
        {
            InitializeComponent();
        }

        private async void OnRecordVideoClicked(object sender, EventArgs e)
        {
            var videoFile = await MediaPicker.CaptureVideoAsync();

            if (videoFile != null)
            {
                await CompressVideoAsync(videoFile);
            }
        }

    }

    private async Task CompressVideoAsync(FileResult videoFile)
    {
        // Logic for compressing video
        var originalVideoPath = videoFile.FullPath;

        // Assume some library or logic here to compress video
        var compressedVideoPath = await VideoCompressor.CompressAsync(originalVideoPath);

        // Do something with the compressed video (save, upload, etc.)
    }

}
