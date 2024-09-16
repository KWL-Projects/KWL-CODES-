using System;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;

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

        private async void OnRecordVideoClicked(object sender, EventArgs e)
        {
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
