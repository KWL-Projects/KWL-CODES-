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
        private readonly BlobStorageService _blobStorageService;
        private readonly string _logFilePath = "path-to-log-file/log.txt"; // Configurable log file path

        public SubmissionController(DatabaseContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        // GET: api/Submission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmission()
        {
            using (StreamWriter log = new StreamWriter(_logFilePath, true))
            {
                try
                {
                    var submissions = await _context.Submission.ToListAsync();
                    log.WriteLine($"{DateTime.Now}: Successfully retrieved all submissions.");
                    return submissions;
                }
                catch (Exception ex)
                {
                    log.WriteLine($"{DateTime.Now}: An error occurred while retrieving submissions - {ex.Message}");
                    return StatusCode(500, "An error occurred while retrieving submissions.");
                }
            }
        }

        // GET: api/Submission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
            using (StreamWriter log = new StreamWriter(_logFilePath, true))
            {
                try
                {
                    var submission = await _context.Submission.FindAsync(id);

                    if (submission == null)
                    {
                        log.WriteLine($"{DateTime.Now}: Submission with ID {id} not found.");
                        return NotFound();
                    }

                    log.WriteLine($"{DateTime.Now}: Successfully retrieved submission with ID {id}.");
                    return submission;
                }
                catch (Exception ex)
                {
                    log.WriteLine($"{DateTime.Now}: An error occurred while retrieving submission with ID {id} - {ex.Message}");
                    return StatusCode(500, "An error occurred while retrieving the submission.");
                }
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
            using (StreamWriter log = new StreamWriter(_logFilePath, true))
            {
                try
                {
                    log.WriteLine($"{DateTime.Now}: Attempting to store the file '{file?.FileName}'");

                    if (file == null || file.Length == 0)
                    {
                        log.WriteLine($"{DateTime.Now}: No file provided or file is empty.");
                        return BadRequest("Please upload a valid file.");
                    }

                    using (var stream = file.OpenReadStream())
                    {
                        var fileName = file.FileName;
                        string filePath = await _blobStorageService.UploadFileAsync(stream, fileName);
                        log.WriteLine($"{DateTime.Now}: File '{fileName}' successfully stored at '{filePath}'");
                        submission.video_path = filePath;
                    }

                    log.WriteLine($"{DateTime.Now}: Attempting to create database entry for submission '{submission.submission_id}'");
                    _context.Submission.Add(submission);
                    await _context.SaveChangesAsync();
                    log.WriteLine($"{DateTime.Now}: Database entry successfully created for submission '{submission.submission_id}'");

                    return CreatedAtAction("GetSubmission", new { id = submission.submission_id }, submission);
                }
                catch (Exception ex)
                {
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
