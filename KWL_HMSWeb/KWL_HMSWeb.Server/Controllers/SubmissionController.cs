using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging; // For logging

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/submission")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SubmissionController> _logger; // Logger

        public SubmissionController(DatabaseContext context, ILogger<SubmissionController> logger)
        {
            _context = context;
            _logger = logger; // Initialize logger
        }

        // POST: api/submission/create
        [HttpPost("create")]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
        {
            try
            {
                _context.Submission.Add(submission);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Submission created successfully with ID: {0}", submission.submission_id);
                return CreatedAtAction(nameof(GetSubmission), new { id = submission.submission_id }, submission);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating submission: {0}", ex.Message);
                return StatusCode(500, "Internal server error while creating submission.");
            }
        }

        // GET: api/submission/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Submission>>> ViewSubmissions()
        {
            try
            {
                var submissions = await _context.Submission.ToListAsync();
                if (submissions == null || !submissions.Any())
                {
                    _logger.LogInformation("No submissions found.");
                    return NotFound("No submissions found.");
                }

                _logger.LogInformation("Successfully retrieved all submissions.");
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving submissions: {0}", ex.Message);
                return StatusCode(500, "Internal server error while retrieving submissions.");
            }
        }

        // GET: api/submission/view/{userId}
        [HttpGet("viewOwn/{userId}")]
        public async Task<ActionResult<IEnumerable<Submission>>> BrowseOwnSubmissions(int userId)
        {
            try
            {
                var submissions = await _context.Submission
                    .Where(s => s.user_id == userId) // Filter by userId
                    .ToListAsync();

                if (submissions == null || !submissions.Any())
                {
                    _logger.LogInformation("No submissions found for user ID: {0}.", userId);
                    return NotFound($"No submissions found for user ID: {userId}.");
                }

                _logger.LogInformation("Successfully retrieved submissions for user ID: {0}.", userId);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving submissions for user ID: {0}. Error: {1}", userId, ex.Message);
                return StatusCode(500, $"Internal server error while retrieving submissions for user ID: {userId}.");
            }
        }

        // GET: api/submission/{id}
        [HttpGet("view/{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
            try
            {
                var submission = await _context.Submission.FindAsync(id);

                if (submission == null)
                {
                    _logger.LogInformation("Submission with ID: {0} not found.", id);
                    return NotFound($"Submission with ID: {id} not found.");
                }

                _logger.LogInformation("Successfully retrieved submission with ID: {0}", id);
                return Ok(submission);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving submission with ID: {0}. Error: {1}", id, ex.Message);
                return StatusCode(500, "Internal server error while retrieving submission.");
            }
        }

        // PUT: api/submission/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutSubmission(int id, Submission submission)
        {
            if (id != submission.submission_id)
            {
                return BadRequest("Submission ID mismatch.");
            }

            _context.Entry(submission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated submission with ID: {0}", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
                {
                    _logger.LogWarning("Submission with ID: {0} not found for update.", id);
                    return NotFound($"Submission with ID: {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating submission with ID: {0}. Error: {1}", id, ex.Message);
                return StatusCode(500, $"Internal server error while updating submission with ID: {id}.");
            }
        }

        // DELETE: api/submission/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            try
            {
                var submission = await _context.Submission.FindAsync(id);
                if (submission == null)
                {
                    _logger.LogInformation("Submission with ID: {0} not found for deletion.", id);
                    return NotFound($"Submission with ID: {id} not found.");
                }

                _context.Submission.Remove(submission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted submission with ID: {0}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting submission with ID: {0}. Error: {1}", id, ex.Message);
                return StatusCode(500, $"Internal server error while deleting submission with ID: {id}.");
            }
        }

        // Helper method to check if submission exists by ID
        private bool SubmissionExists(int id)
        {
            return _context.Submission.Any(e => e.submission_id == id);
        }
    }
}


