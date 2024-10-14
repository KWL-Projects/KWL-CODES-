using Azure.Storage.Blobs;
using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace KWL_HMSWeb.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<BlobStorageService> _logger;
        private readonly DatabaseContext _context; // Add DatabaseContext for accessing the database

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger, DatabaseContext context)
        {
            _logger = logger;

            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
            _context = context; // Initialize DatabaseContext
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

        // New method to get video paths by assignment ID
        public async Task<List<string>> GetVideosByAssignmentIdAsync(int assignmentId)
        {
            try
            {
                // Retrieve all video paths associated with the provided assignment ID from the database
                var videoPaths = await _context.Submission
                    .Where(s => s.assignment_id == assignmentId) // Ensure this matches your Submission model's field for assignment ID
                    .Select(s => s.video_path)
                    .ToListAsync();

                if (videoPaths == null || videoPaths.Count == 0)
                {
                    _logger.LogWarning($"No videos found for assignment ID '{assignmentId}'.");
                }
                else
                {
                    _logger.LogInformation($"Successfully retrieved {videoPaths.Count} videos for assignment ID '{assignmentId}'.");
                }

                return videoPaths; // Return the list of video paths
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving videos for assignment ID '{assignmentId}'.");
                throw; // Rethrow the exception to handle it in the controller
            }
        }

        // New method to get the video path by submission ID
        public async Task<string> GetVideoPathBySubmissionIdAsync(int submissionId)
        {
            try
            {
                // Retrieve the video path associated with the provided submission ID from the database
                var videoPath = await _context.Submission
                    .Where(s => s.submission_id == submissionId) // Ensure this matches your Submission model's field for submission ID
                    .Select(s => s.video_path)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(videoPath))
                {
                    _logger.LogWarning($"No video found for submission ID '{submissionId}'.");
                }
                else
                {
                    _logger.LogInformation($"Successfully retrieved video path for submission ID '{submissionId}'.");
                }

                return videoPath; // Return the video path
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving video path for submission ID '{submissionId}'.");
                throw; // Rethrow the exception to handle it in the controller
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
}





