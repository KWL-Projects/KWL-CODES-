using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KWL_HMSWeb.Server.Models;

namespace KWLCodes_HMSProject.Maui.Services
{
    public class AssignmentService
    {
        private readonly HttpClient _httpClient;

        public AssignmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch all assignments
        public async Task<List<Assignment>> GetAllAssignmentsAsync()
        {
            try
            {
                var assignments = await _httpClient.GetFromJsonAsync<List<Assignment>>("https://yourserverurl/api/assignment/all");
                return assignments ?? new List<Assignment>(); // Handle null
            }
            catch (Exception ex)
            {
                // Handle any errors, such as logging
                Console.WriteLine($"Error fetching assignments: {ex.Message}");
                return new List<Assignment>(); // Return an empty list on error
            }
        }

        // View assignment by id
        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {
            try
            {
                var assignment = await _httpClient.GetFromJsonAsync<Assignment>($"https://yourserverurl/api/assignment/view/{id}");
                return assignment ?? new Assignment(); // Handle null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignment by ID: {ex.Message}");
                return null; // Return null on error
            }
        }
    }
}
