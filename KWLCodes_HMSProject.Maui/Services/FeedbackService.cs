using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KWLCodes_HMSProject.Maui.Models; // Adjust this to match your actual namespace

namespace KWLCodes_HMSProject.Maui.Services
{
    public class FeedbackService
    {
        private readonly HttpClient _httpClient;

        public FeedbackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:7074/"); // Change this to your API URL
        }

        // Provide feedback on video
        public async Task<string> ProvideFeedbackAsync(Feedback feedback)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/feedback/submit", feedback);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Log error (use an ILogger if you have one available)
                Console.WriteLine($"Error providing feedback: {ex.Message}");
                return null;
            }
        }

        // Get all feedbacks
        public async Task<IEnumerable<Feedback>> GetFeedbackAsync()
        {
            try
            {
                var feedbacks = await _httpClient.GetFromJsonAsync<IEnumerable<Feedback>>("api/feedback/all");
                return feedbacks ?? new List<Feedback>();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error retrieving feedback: {ex.Message}");
                return new List<Feedback>();
            }
        }

        // View feedback on submissions for a specific user
        public async Task<IEnumerable<Feedback>> ViewFeedbackAsync(int userId)
        {
            try
            {
                var feedbacks = await _httpClient.GetFromJsonAsync<IEnumerable<Feedback>>($"api/feedback/submission/{userId}");
                return feedbacks ?? new List<Feedback>();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error viewing feedback for user {userId}: {ex.Message}");
                return new List<Feedback>();
            }
        }

        // Update feedback
        public async Task<string> UpdateFeedbackAsync(int feedbackId, Feedback updatedFeedback)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/feedback/update/{feedbackId}", updatedFeedback);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error updating feedback {feedbackId}: {ex.Message}");
                return null;
            }
        }

        // Delete feedback
        public async Task<string> DeleteFeedbackAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/feedback/delete/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error deleting feedback {id}: {ex.Message}");
                return null;
            }
        }

        // Download marks for a user
        public async Task<byte[]> DownloadMarksAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/feedback/download-marks/{userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error downloading marks for user {userId}: {ex.Message}");
                return null;
            }
        }
    }
}
