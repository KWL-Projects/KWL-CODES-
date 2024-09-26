using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using KWL_HMSWeb.Services;

namespace KWL_HMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoStreamController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IServices _videoService; // Service to retrieve file path from DB
        private readonly ILogService _logService; // Service to handle log entries

        public VideoStreamController(BlobServiceClient blobServiceClient, IServices videoService, ILogService logService)
        {
            _blobServiceClient = blobServiceClient;
            _videoService = videoService;
            _logService = logService; // Initialize logging service
        }

        // Stream video using file path stored in the database
        [HttpGet("stream/{videoId}")]
        public async Task<IActionResult> StreamVideo(int videoId)
        {
            try
            {
                // Retrieve the file path from the database using the videoId
                var filePath = await _videoService.GetVideoFilePathAsync(videoId);
                if (string.IsNullOrEmpty(filePath))
                {
                    await _logService.LogAsync($"Video {videoId} not found.");
                    return NotFound();
                }

                // Parse the file path to get the container name and blob name
                var blobUri = new Uri(filePath);
                string containerName = blobUri.Segments[1].TrimEnd('/');
                string blobName = string.Join("", blobUri.Segments.Skip(2));

                // Get the blob client using the path
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                if (!await blobClient.ExistsAsync())
                {
                    await _logService.LogAsync($"Blob {blobName} not found in container {containerName}.");
                    return NotFound();
                }

                // Get file properties like size for range requests
                var request = HttpContext.Request;
                string rangeHeader = request.Headers["Range"];
                var blobProperties = await blobClient.GetPropertiesAsync();
                long fileSize = blobProperties.Value.ContentLength;

                var blobStream = await blobClient.OpenReadAsync(new BlobOpenReadOptions(false));

                if (string.IsNullOrEmpty(rangeHeader))
                {
                    await _logService.LogAsync($"Video {videoId} streamed successfully.");
                    return new FileStreamResult(blobStream, "video/mp4")
                    {
                        EnableRangeProcessing = true
                    };
                }

                // Handle partial content requests
                long from = 0, to = fileSize - 1;
                var unit = "bytes";
                var range = rangeHeader.Replace("bytes=", "").Split('-');
                if (!string.IsNullOrWhiteSpace(range[0]))
                {
                    from = long.Parse(range[0]);
                }
                if (!string.IsNullOrWhiteSpace(range[1]))
                {
                    to = long.Parse(range[1]);
                }

                long length = to - from + 1;
                HttpContext.Response.Headers["Accept-Ranges"] = unit;
                HttpContext.Response.Headers["Content-Range"] = $"{unit} {from}-{to}/{fileSize}";
                HttpContext.Response.ContentLength = length;
                HttpContext.Response.StatusCode = 206; // Partial Content

                // Stream the specific range of the video
                blobStream.Seek(from, SeekOrigin.Begin);
                await _logService.LogAsync($"Video {videoId} streamed successfully with range {from}-{to}.");
                return new FileStreamResult(blobStream, "video/mp4")
                {
                    EnableRangeProcessing = true
                };
            }
            catch (Exception ex)
            {
                await _logService.LogAsync($"Error streaming video {videoId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
