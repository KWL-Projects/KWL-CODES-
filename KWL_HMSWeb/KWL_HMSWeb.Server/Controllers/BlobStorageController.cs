using KWL_HMSWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KWL_HMSWeb.Services;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly BlobStorageService _blobStorageService;

    public FilesController(BlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please upload a valid file.");
        }

        using (var stream = file.OpenReadStream())
        {
            var fileName = file.FileName;
            var result = await _blobStorageService.UploadFileAsync(stream, fileName);

            return Ok(new { FileUrl = result });
        }
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var fileStream = await _blobStorageService.DownloadFileAsync(fileName);

        return File(fileStream, "application/octet-stream", fileName);
    }
}
