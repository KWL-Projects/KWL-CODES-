using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace KWLCodes_HMSProject.Maui.Services
{
    public class FilesService
    {
        private readonly HttpClient _httpClient; // HttpClient to make HTTP requests
        private readonly ILogger<FilesService> _logger; // Logger for logging activities

        // Constructor to inject HttpClient and ILogger<FilesService>
        public FilesService(HttpClient httpClient, ILogger<FilesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Method to upload a file to the server
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            // Create a new MultipartFormDataContent
            using (var content = new MultipartFormDataContent())
            {
                // Add the file stream to the content
                content.Add(new StreamContent(fileStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/octet-stream") }
                }, "file", fileName); // 'file' is the key used in the API

                try
                {
                    // Send a POST request to the UploadFile endpoint
                    var response = await _httpClient.PostAsync("api/files/upload", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content if the upload was successful
                        var result = await response.Content.ReadAsAsync<dynamic>(); // Adjust this to your expected response type
                        return result.FileUrl; // Return the file URL
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to upload file: {response.ReasonPhrase}");
                        return null; // Handle error case
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file.");
                    return null; // Handle exception case
                }
            }
        }

        // Optional: Method to get the list of files
        public async Task<List<string>> GetFileListAsync()
        {
            try
            {
                // Send a GET request to the GetFileList endpoint
                var response = await _httpClient.GetAsync("api/files/files");
                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                var fileNames = await response.Content.ReadAsAsync<List<string>>(); // Adjust this to your expected response type
                return fileNames; // Return the list of file names
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching file list.");
                return null; // Handle exception case
            }
        }
    }
}
