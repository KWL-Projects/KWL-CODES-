using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace KWLCodes_HMSProject.Maui
{
    public partial class DownloadVideoPage : ContentPage
    {
        public DownloadVideoPage()
        {
            InitializeComponent();
        }

        private async void OnDownloadVideosClicked(object sender, EventArgs e)
        {
            // Logic for downloading videos
            await DownloadStudentVideosAsync();
        }

        private async Task DownloadStudentVideosAsync()
        {
            // Replace with your logic to download video files
            // This can be from Azure, your backend API, etc.

            string videoUrl = "https://yourserver.com/path-to-student-video.mp4";
            string localFilePath = Path.Combine(FileSystem.CacheDirectory, "student-video.mp4");

            using (var client = new HttpClient())
            {
                var videoData = await client.GetByteArrayAsync(videoUrl);
                File.WriteAllBytes(localFilePath, videoData);

                await DisplayAlert("Success", "Video downloaded successfully!", "OK");
            }
        }

        private async void OnSubmitFeedbackClicked(object sender, EventArgs e)
        {
            string feedback = FeedbackEditor.Text;

            if (string.IsNullOrWhiteSpace(feedback))
            {
                await DisplayAlert("Error", "Please enter feedback before submitting.", "OK");
                return;
            }

            // Logic to submit feedback to the backend or save it
            await SubmitFeedbackAsync(feedback);
        }

        private async Task SubmitFeedbackAsync(string feedback)
        {
            // Replace this with your backend API call or database logic
            await Task.Delay(1000); // Simulate network call

            await DisplayAlert("Success", "Feedback submitted successfully!", "OK");
        }

    }
}
