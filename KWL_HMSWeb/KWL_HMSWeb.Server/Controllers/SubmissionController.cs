using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using KWL_HMSWeb.Services;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/submission")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly BlobStorageService _blobStorageService; // Add BlobStorageService

        // Constructor with dependency injection
        public SubmissionController(DatabaseContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService; // Initialize BlobStorageService
        }

        // GET: api/Submission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmission()
        {
            return await _context.Submission.ToListAsync();
        }

        // GET: api/Submission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
            var submission = await _context.Submission.FindAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            return submission;
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
        //On file upload write in a log file atempting to store a file then store the file, after file was stored write log file to say file sucessfully stored
        //Store return from file upload (file upload returns file path) After file return create a write in log file, atempting to create a database entry, add all data to log file
        //Add the submission to the database, write return of add to database into log file
        [HttpPost]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission, IFormFile file)
        {
            string logFilePath = "path-to-log-file/log.txt";  // Path to your log file
            using (StreamWriter log = new StreamWriter(logFilePath, true))
            {
                try
                {
                    // Log: Attempt to store the file
                    log.WriteLine($"{DateTime.Now}: Attempting to store the file '{file?.FileName}'");

                    // Validate if the file exists
                    if (file == null || file.Length == 0)
                    {
                        log.WriteLine($"{DateTime.Now}: No file provided or file is empty.");
                        return BadRequest("Please upload a valid file.");
                    }

                    // Upload the file using BlobStorageService
                    using (var stream = file.OpenReadStream())
                    {
                        var fileName = file.FileName;

                        // Attempt to upload file
                        string filePath = await _blobStorageService.UploadFileAsync(stream, fileName);

                        // Log: File successfully stored
                        log.WriteLine($"{DateTime.Now}: File '{fileName}' successfully stored at '{filePath}'");

                        // Add the file path to the submission object
                        submission.video_path = filePath;
                    }

                    // Log: Attempt to create database entry
                    log.WriteLine($"{DateTime.Now}: Attempting to create database entry for submission '{submission.submission_id}'");

                    // Add submission to the database
                    _context.Submission.Add(submission);
                    await _context.SaveChangesAsync();

                    // Log: Database entry successfully created
                    log.WriteLine($"{DateTime.Now}: Database entry successfully created for submission '{submission.submission_id}'");

                    // Return the created submission
                    return CreatedAtAction("GetSubmission", new { id = submission.submission_id }, submission);
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during the process
                    log.WriteLine($"{DateTime.Now}: An error occurred - {ex.Message}");
                    return StatusCode(500, "An error occurred while processing the submission.");
                }
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
