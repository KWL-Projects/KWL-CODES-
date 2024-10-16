using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KWLCodes_HMSProject.Maui.Models;

namespace KWLCodes_HMSProject.Maui.Services
{
    public class AssignmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AssignmentService> _logger;

        // Inject HttpClient and ILogger
        public AssignmentService(HttpClient httpClient, ILogger<AssignmentService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:7074/"); // Change this to your API URL
            _logger = logger;
        }

        // Fetch all assignments from the API
        public async Task<List<Assignment>> GetAllAssignmentsAsync()
        {
            try
            {
                var assignments = await _httpClient.GetFromJsonAsync<List<Assignment>>("api/assignment/all");
                return assignments ?? new List<Assignment>(); // Return an empty list if null
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignments.");
                return new List<Assignment>(); // Return an empty list on error
            }
        }

        // Fetch assignment by ID from the API
        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            try
            {
                var assignment = await _httpClient.GetFromJsonAsync<Assignment>($"api/assignment/view/{id}");
                return assignment ?? new Assignment(); // Return a new Assignment if null
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching assignment with ID {id}.");
                return null; // Return null on error
            }
        }
    }
}
