using Azure.Storage.Blobs;
using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using KWL_HMSWeb.Controllers;

namespace KWL_HMSWeb.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        // Constructor to initialize the Blob Container Client
        public BlobStorageService(IConfiguration configuration)
        {
            // Retrieve connection string and container name from appsettings.json
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            // Create a BlobContainerClient to interact with the blob container
            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        // Method to upload a file to Azure Blob Storage
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                // Create the container if it does not exist
                await _blobContainerClient.CreateIfNotExistsAsync();

                // Get a reference to the blob (file) in the container
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                // Upload the file
                await blobClient.UploadAsync(fileStream, overwrite: true);

                // Return the URI of the uploaded file
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new ApplicationException("Error uploading file to Blob Storage.", ex);
            }
        }

        // Method to download a file from Azure Blob Storage
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                // Get a reference to the blob (file) in the container
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                // Download the file content as a stream
                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new ApplicationException("Error downloading file from Blob Storage.", ex);
            }
        }

    }
    public class VideoService : IVideoService
    {
        private readonly DatabaseContext _context;

        public VideoService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> GetVideoFilePathAsync(int videoId)
        {
            var video = await _context.Submission
                                      .Where(v => v.submission_id == videoId)
                                      .Select(v => v.video_path)
                                      .FirstOrDefaultAsync();

            return video;
        }
    }

}