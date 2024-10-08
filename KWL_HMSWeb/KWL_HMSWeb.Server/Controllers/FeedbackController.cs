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

        // 1st functionality: View feedback on your submissions
        // GET: api/feedback/submissions/{userId}
        [HttpGet("submissions/{userId}")]
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
        // POST: api/feedback/submit
        [HttpPost("submit")]
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
        // GET: api/feedback/download-marks/{userId}
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

        // 4th functionality: Update feedback on video
        // PUT: api/feedback/update/{feedbackId}
        [HttpPut("update/{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] Feedback updatedFeedback)
        {
            if (updatedFeedback == null || string.IsNullOrEmpty(updatedFeedback.feedback) || updatedFeedback.mark_received == null)
                return BadRequest("Invalid feedback input.");

            try
            {
                var existingFeedback = await _context.Feedback.FindAsync(feedbackId);

                if (existingFeedback == null)
                    return NotFound("Feedback not found.");

                // Update the feedback fields
                existingFeedback.feedback = updatedFeedback.feedback;
                existingFeedback.mark_received = updatedFeedback.mark_received;

                _context.Feedback.Update(existingFeedback);
                await _context.SaveChangesAsync();

                // Log success
                Log("Feedback successfully updated for feedback ID: " + feedbackId);

                return Ok("Feedback updated successfully.");
            }
            catch (Exception ex)
            {
                // Log failure
                Log("Failed to update feedback for feedback ID: " + feedbackId + ". Error: " + ex.Message);
                return StatusCode(500, "An error occurred while updating feedback.");
            }
        }


        private void Log(string message)
        {
            // Implement your logging functionality here (e.g., write to a file or database)
            Console.WriteLine(message);
        }
    }
}




