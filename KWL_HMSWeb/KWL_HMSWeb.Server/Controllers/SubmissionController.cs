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

        // 1st functionality: View all submissions
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
                _logger.LogError($"Error retrieving submissions: {ex.Message}");
                return StatusCode(500, "Internal server error while retrieving submissions.");
            }
        }

        // 2nd functionality: Browse own submissions
        // GET: api/submission/view/{userId}
        [HttpGet("view/{userId}")]
        public async Task<ActionResult<IEnumerable<Submission>>> BrowseOwnSubmissions(int userId)
        {
            try
            {
                var submissions = await _context.Submission
                    .Where(s => s.user_id == userId) // Filter by userId
                    .ToListAsync();

                if (submissions == null || !submissions.Any())
                {
                    _logger.LogInformation($"No submissions found for user ID: {userId}.");
                    return NotFound($"No submissions found for user ID: {userId}.");
                }

                _logger.LogInformation($"Successfully retrieved submissions for user ID: {userId}.");
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving submissions for user ID: {userId}. Error: {ex.Message}");
                return StatusCode(500, $"Internal server error while retrieving submissions for user ID: {userId}.");
            }
        }

        // Existing methods (POST, PUT, DELETE, etc.) remain the same
        // POST: api/Submission/create
        [HttpPost("create")]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
        {
            _context.Submission.Add(submission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubmission", new { id = submission.submission_id }, submission);
        }

        // PUT: api/Submission/update/5
        [HttpPut("update/{id}")]
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

        // DELETE: api/Submission/delete/5
        [HttpDelete("delete/{id}")]
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


