using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace KWLCodes_HMSProject.Maui.Services
{
    public class FilesService
    {
        private readonly HttpClient _httpClient; // HttpClient for making requests
        private readonly ILogger<FilesService> _logger; // Logger for logging activities

        // Constructor to inject HttpClient and ILogger
        public FilesService(HttpClient httpClient, ILogger<FilesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Method to upload a file to the server
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(fileStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/octet-stream") }
                }, "file", fileName);

                try
                {
                    var response = await _httpClient.PostAsync("api/files/upload", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<dynamic>();
                        return result.FileUrl; // Return the file URL from the response
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to upload file: {response.ReasonPhrase}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file.");
                    return null;
                }
            }
        }

        // Optional: Method to get the list of files
        public async Task<List<string>> GetFileListAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/files/files");
                response.EnsureSuccessStatusCode();

                var fileNames = await response.Content.ReadFromJsonAsync<List<string>>();
                return fileNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching file list.");
                return null;
            }
        }
    }
}
