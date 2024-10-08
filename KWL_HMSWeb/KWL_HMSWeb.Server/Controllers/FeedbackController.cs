using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FeedbackController(DatabaseContext context)
        {
            _context = context;
        }

        // Existing methods...

        // 1st functionality: View feedback on your submissions
        [HttpGet("my-submissions/{userId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> ViewMyFeedback(int userId)
        {
            try
            {
                var submissions = await _context.Submission
                    .Where(s => s.user_id == userId)
                    .ToListAsync();

                if (!submissions.Any())
                    return NotFound("No submissions found for the user.");

                var submissionIds = submissions.Select(s => s.submission_id).ToList();
                var feedbacks = await _context.Feedback
                    .Where(f => submissionIds.Contains(f.submission_id))
                    .ToListAsync();

                if (!feedbacks.Any())
                    return NotFound("No feedback found for the user's submissions.");

                // Log success
                Log("Feedback successfully retrieved for user: " + userId);

                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Log failure
                Log("Failed to retrieve feedback for user: " + userId + ". Error: " + ex.Message);
                return StatusCode(500, "An error occurred while retrieving feedback.");
            }
        }

        // 2nd functionality: Provide feedback on video
        [HttpPost("submit-feedback")]
        public async Task<IActionResult> ProvideFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null || string.IsNullOrEmpty(feedback.feedback) || feedback.mark_received == null)
                return BadRequest("Invalid feedback input.");

            try
            {
                _context.Feedback.Add(feedback);
                await _context.SaveChangesAsync();

                // Log success
                Log("Feedback successfully submitted for submission: " + feedback.submission_id);

                return Ok("Feedback submitted successfully.");
            }
            catch (Exception ex)
            {
                // Log failure
                Log("Failed to submit feedback for submission: " + feedback.submission_id + ". Error: " + ex.Message);
                return StatusCode(500, "An error occurred while submitting feedback.");
            }
        }

        // 3rd functionality: Download marks
        [HttpGet("download-marks/{userId}")]
        public async Task<IActionResult> DownloadMarks(int userId)
        {
            try
            {
                var feedbacks = await _context.Feedback
                    .Where(f => _context.Submission
                        .Any(s => s.user_id == userId && s.submission_id == f.submission_id))
                    .ToListAsync();

                if (!feedbacks.Any())
                    return NotFound("No marks found for the user.");

                var marksData = "Submission ID, Mark Received\n";
                foreach (var feedback in feedbacks)
                {
                    marksData += $"{feedback.submission_id}, {feedback.mark_received}\n";
                }

                // Convert marks data to a byte array for download
                var fileBytes = System.Text.Encoding.UTF8.GetBytes(marksData);
                var fileName = $"Marks_User_{userId}.csv";

                // Log success
                Log("Marks file successfully generated for user: " + userId);

                return File(fileBytes, "application/csv", fileName);
            }
            catch (Exception ex)
            {
                // Log failure
                Log("Failed to generate marks file for user: " + userId + ". Error: " + ex.Message);
                return StatusCode(500, "An error occurred while generating the marks file.");
            }
        }

        private void Log(string message)
        {
            // Implement your logging functionality here (e.g., write to a file or database)
            Console.WriteLine(message);
        }
    }
}



