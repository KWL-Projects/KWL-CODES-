using System;
using Microsoft.Maui.Controls;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class ViewAssignments : ContentPage
    {
        public ViewAssignments()
        {
            InitializeComponent();
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            // Load Assiggnments Here
        }

        private async void OnUploadVideoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new UploadVideo());
        }

        private async void OnViewVideoFeedbackClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new ViewVideoFeedback());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
