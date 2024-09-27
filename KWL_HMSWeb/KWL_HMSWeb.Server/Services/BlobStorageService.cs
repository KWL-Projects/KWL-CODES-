using Azure.Storage.Blobs;
using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using KWL_HMSWeb.Controllers;
using Azure.Storage.Blobs.Models;
using KWL_HMSWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KWL_HMSWeb.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _logger = logger;

            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        // Method to upload a file to Azure Blob Storage
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                _logger.LogInformation($"Uploading file '{fileName}' to Blob Storage.");
                await _blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                await blobClient.UploadAsync(fileStream, overwrite: true);
                _logger.LogInformation($"File '{fileName}' uploaded successfully.");

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading file '{fileName}'.");
                throw;
            }
        }

        // Method to download a file from Azure Blob Storage
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                _logger.LogInformation($"Downloading file '{fileName}' from Blob Storage.");
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading file '{fileName}'.");
                throw;
            }
        }

        // Method to get all file names from the blob container
        public async Task<List<string>> GetAllFileNamesAsync()
        {
            try
            {
                var fileNames = new List<string>();

                // Ensure the container exists
                await _blobContainerClient.CreateIfNotExistsAsync();

                // Retrieve all blobs in the container
                await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
                {
                    fileNames.Add(blobItem.Name); // Add each blob's name to the list
                }

                _logger.LogInformation("Retrieved file names from Blob Storage successfully.");
                return fileNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file names from Blob Storage.");
                throw;
            }
        }
    }
    public class VideoService : IServices
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


