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
        private readonly IServices _videoService;
        private readonly ILogService _logService;

        // Constructor: Inject BlobServiceClient, video service, and logging service
        public VideoStreamController(BlobServiceClient blobServiceClient, IServices videoService, ILogService logService)
        {
            _blobServiceClient = blobServiceClient;
            _videoService = videoService;
            _logService = logService;
        }

        // Endpoint to stream video based on videoId - api/videostream/stream/{videoId}
        [HttpGet("stream/{videoId}")]
        public async Task<IActionResult> StreamVideo(int videoId)
        {
            try
            {
                // Retrieve the file path of the video using the video service
                var filePath = await _videoService.GetVideoFilePathAsync(videoId);
                if (string.IsNullOrEmpty(filePath))
                {
                    // Log if video is not found
                    await _logService.LogAsync($"Video {videoId} not found.");
                    return NotFound();
                }

                // Parse the blob URI to get container and blob names
                var blobUri = new Uri(filePath);
                string containerName = blobUri.Segments[1].TrimEnd('/');
                string blobName = string.Join("", blobUri.Segments.Skip(2));

                // Get the blob container and blob clients
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                // Check if the blob exists
                if (!await blobClient.ExistsAsync())
                {
                    // Log if blob is not found
                    await _logService.LogAsync($"Blob {blobName} not found in container {containerName}.");
                    return NotFound();
                }

                // Retrieve blob properties to determine the content length (file size)
                var blobProperties = await blobClient.GetPropertiesAsync();
                long fileSize = blobProperties.Value.ContentLength;

                // Open a read stream to the blob
                var blobStream = await blobClient.OpenReadAsync(new BlobOpenReadOptions(false));

                // Log when the video stream initialization begins
                await _logService.LogAsync($"Video stream initialization started for Video ID {videoId}.");

                // Check if a Range header is provided by the client
                string rangeHeader = HttpContext.Request.Headers["Range"];
                if (string.IsNullOrEmpty(rangeHeader))
                {
                    // Log successful streaming without range
                    await _logService.LogAsync($"Video {videoId} streamed successfully.");
                    return new FileStreamResult(blobStream, "video/mp4")
                    {
                        EnableRangeProcessing = true
                    };
                }

                // Process the range request from the client
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
                HttpContext.Response.StatusCode = 206;

                // Adjust the stream position based on the range
                blobStream.Seek(from, SeekOrigin.Begin);

                // Log the range being streamed
                await _logService.LogAsync($"Video {videoId} is being streamed with range {from}-{to}.");

                return new FileStreamResult(blobStream, "video/mp4")
                {
                    EnableRangeProcessing = true
                };
            }
            catch (Exception ex)
            {
                // Log any errors that occur during streaming
                await _logService.LogAsync($"Error streaming video {videoId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                // Log when the server connection is closed
                await _logService.LogAsync($"Server connection closed for Video ID {videoId}.");
            }
        }
    }
}


