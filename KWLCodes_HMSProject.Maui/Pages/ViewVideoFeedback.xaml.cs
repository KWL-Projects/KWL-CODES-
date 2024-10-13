using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Models;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class ViewVideoFeedback : ContentPage
    {
        private readonly FeedbackService _feedbackService;
        private readonly int _submissionId; // To store the submission ID

        public ViewVideoFeedback(int submissionId, FeedbackService feedbackService) // Constructor
        {
            InitializeComponent();
            _submissionId = submissionId; // Assign the submission ID
            _feedbackService = feedbackService; // Assign the FeedbackService
            LoadFeedback(); // Load feedback data
        }

        private async void LoadFeedback()
        {
            try
            {
                var feedbacks = await _feedbackService.ViewFeedbackAsync(_submissionId); // Fetch feedback
                FeedbackCollectionView.ItemsSource = feedbacks; // Bind feedback to the CollectionView
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., show an alert)
                await DisplayAlert("Error", $"Failed to load feedback: {ex.Message}", "OK");
            }
        }

        private async void OnDownloadFeedbackClicked(object sender, EventArgs e)
        {
            try
            {
                // Call your download feedback API using the FeedbackService
                var marksData = await _feedbackService.DownloadMarksAsync(_submissionId); // Replace with actual logic
                if (marksData != null)
                {
                    // Logic to save or handle the downloaded marks data
                    await DisplayAlert("Success", "Feedback downloaded successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to download feedback.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., show an alert)
                await DisplayAlert("Error", $"Failed to download feedback: {ex.Message}", "OK");
            }
        }
    }
}
