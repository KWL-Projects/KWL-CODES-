using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KWL_HMSWeb.Server.Models;// Adjust this namespace to where your Feedback model is defined

namespace KWLCodes_HMSProject.Maui.Services
{
    public class FeedbackService
    {
        private readonly HttpClient _httpClient;

        public FeedbackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Provide feedback on video
        public async Task<string> ProvideFeedbackAsync(Feedback feedback)
        {
            var response = await _httpClient.PostAsJsonAsync("api/feedback/submit", feedback);
            response.EnsureSuccessStatusCode(); // Throws if the status code is not a success code
            return await response.Content.ReadAsStringAsync();
        }

        // Get all feedbacks
        public async Task<IEnumerable<Feedback>> GetFeedbackAsync()
        {
            var response = await _httpClient.GetAsync("api/feedback/all");
            response.EnsureSuccessStatusCode();

            var feedbacks = await response.Content.ReadFromJsonAsync<IEnumerable<Feedback>>();
            return feedbacks;
        }

        // View feedback on submissions for a specific user
        public async Task<IEnumerable<Feedback>> ViewFeedbackAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/feedback/submission/{userId}");
            response.EnsureSuccessStatusCode();

            var feedbacks = await response.Content.ReadFromJsonAsync<IEnumerable<Feedback>>();
            return feedbacks;
        }

        // Update feedback
        public async Task<string> UpdateFeedbackAsync(int feedbackId, Feedback updatedFeedback)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/feedback/update/{feedbackId}", updatedFeedback);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Delete feedback
        public async Task<string> DeleteFeedbackAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/feedback/delete/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Download marks for a user
        public async Task<byte[]> DownloadMarksAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/feedback/download-marks/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}

