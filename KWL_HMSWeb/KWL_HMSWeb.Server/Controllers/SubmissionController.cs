using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging;
using KWL_HMSWeb.Services;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/submission")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly BlobStorageService _blobStorageService;
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(DatabaseContext context, BlobStorageService blobStorageService, ILogger<SubmissionController> logger)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        // GET: api/Submission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmission()
        {
            try
            {
                var submissions = await _context.Submission.ToListAsync();
                _logger.LogInformation("Successfully retrieved all submissions.");
                return submissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving submissions.");
                return StatusCode(500, "An error occurred while retrieving submissions.");
            }
        }

        // GET: api/Submission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
            try
            {
                var submission = await _context.Submission.FindAsync(id);

                if (submission == null)
                {
                    _logger.LogWarning("Submission with ID {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved submission with ID {Id}.", id);
                return submission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving submission with ID {Id}.", id);
                return StatusCode(500, "An error occurred while retrieving the submission.");
            }
        }

        // PUT: api/Submission/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubmission(int id, Submission submission)
        {
            if (id != submission.submission_id)
            {
                return BadRequest();
            }

            _context.Entry(submission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Submission
        [HttpPost]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission, IFormFile file)
        {
            try
            {
                _logger.LogInformation("Attempting to store the file '{FileName}'", file?.FileName);

                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file provided or file is empty.");
                    return BadRequest("Please upload a valid file.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var fileName = file.FileName;
                    string filePath = await _blobStorageService.UploadFileAsync(stream, fileName);
                    _logger.LogInformation("File '{FileName}' successfully stored at '{FilePath}'", fileName, filePath);
                    submission.video_path = filePath;
                }

                _logger.LogInformation("Attempting to create database entry for submission '{SubmissionId}'", submission.submission_id);
                _context.Submission.Add(submission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Database entry successfully created for submission '{SubmissionId}'", submission.submission_id);

                return CreatedAtAction("GetSubmission", new { id = submission.submission_id }, submission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the submission.");
                return StatusCode(500, "An error occurred while processing the submission.");
            }
        }

        // DELETE: api/Submission/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            var submission = await _context.Submission.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            _context.Submission.Remove(submission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubmissionExists(int id)
        {
            return _context.Submission.Any(e => e.submission_id == id);
        }
    }
}

