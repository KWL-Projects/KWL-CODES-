using KWL_HMSWeb.Services; // Importing the BlobStorageService
using Microsoft.AspNetCore.Http; // For handling HTTP requests and responses
using Microsoft.AspNetCore.Mvc; // For creating API Controllers
using Microsoft.Extensions.Logging; // For logging information, warnings, and errors
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/upload")] // Base route for this controller is 'api/upload'
public class FilesController : ControllerBase
{
    private readonly BlobStorageService _blobStorageService; // Service to interact with Azure Blob Storage
    private readonly ILogger<FilesController> _logger; // Logger for logging activities

    // Constructor to inject the BlobStorageService and ILogger<FilesController>
    public FilesController(BlobStorageService blobStorageService, ILogger<FilesController> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    // HTTP POST method for uploading a file - /api/upload/upload
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // Check if the uploaded file is valid
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Upload attempt with an invalid file."); // Log a warning if no file is provided
            return BadRequest("Please upload a valid file."); // Return a bad request response
        }

        using (var stream = file.OpenReadStream())
        {
            var fileName = file.FileName; // Get the file name
            try
            {
                // Attempt to upload the file using the BlobStorageService
                var result = await _blobStorageService.UploadFileAsync(stream, fileName);
                _logger.LogInformation($"File '{fileName}' uploaded successfully."); // Log successful upload
                return Ok(new { FileUrl = result }); // Return the URL of the uploaded file
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading file '{fileName}'."); // Log the error if an exception occurs
                return StatusCode(500, "Internal server error."); // Return a 500 Internal Server Error response
            }
        }
    }

    // HTTP GET method for downloading a file by its name - api/upload/download/{fileName}
    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        try
        {
            _logger.LogInformation($"Download initiated for file '{fileName}'.");

            // Attempt to download the file using the BlobStorageService
            var fileStream = await _blobStorageService.DownloadFileAsync(fileName);

            if (fileStream == null)
            {
                _logger.LogWarning($"File '{fileName}' not found."); // Log a warning if the file is not found
                return NotFound("File not found."); // Return a 404 Not Found response
            }

            // Determine the MIME type based on the file extension
            var contentType = GetContentType(fileName);

            _logger.LogInformation($"File '{fileName}' downloaded successfully."); // Log successful download

            // Set the Content-Disposition header to suggest a filename

            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            Response.ContentType = contentType; // e.g., video/mp4

            return File(fileStream, contentType); // Return the file as a downloadable stream
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading file '{fileName}'."); // Log the error if an exception occurs
            return StatusCode(500, "Internal server error."); // Return a 500 Internal Server Error response
        }
    }

    // Helper method to determine the content type based on the file extension
    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            ".mov" => "video/quicktime",
            ".wmv" => "video/x-ms-wmv",
            ".flv" => "video/x-flv",
            ".mkv" => "video/x-matroska",
            _ => "application/octet-stream", // Default to binary for unknown types
        };
    }

    // HTTP GET method to get a list of all file names - api/upload/files
    [HttpGet("files")]
    public async Task<IActionResult> GetFileList()
    {
        try
        {
            // Retrieve the list of file names using the BlobStorageService
            var fileNames = await _blobStorageService.GetAllFileNamesAsync();
            return Ok(fileNames); // Return the list of file names
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching file list."); // Log the error if an exception occurs
            return StatusCode(500, "Internal server error."); // Return a 500 Internal Server Error response
        }
    }
}

