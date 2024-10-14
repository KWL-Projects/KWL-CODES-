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
        private readonly ILogger<FeedbackController> _logger;
        private readonly DatabaseContext _context;

        public FeedbackController(DatabaseContext context, ILogger<FeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Provide feedback on video
        // POST: api/feedback/submit
        [HttpPost("submit")]
        public async Task<IActionResult> ProvideFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Feedback data is required.");
            }

            try
            {
                _context.Feedback.Add(feedback);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Feedback provided successfully.");
                return Ok("Feedback submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to provide feedback.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get all feedbacks
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedback()
        {
            try
            {
                var feedbacks = await _context.Feedback.ToListAsync();

                if (feedbacks == null || !feedbacks.Any())
                {
                    return NotFound(new { message = "No feedback found." });
                }

                return Ok(new
                {
                    message = "Feedback retrieved successfully.",
                    data = feedbacks
                });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new { message = "An error occurred while retrieving feedback.", error = ex.Message });
            }
        }

        // Get feedback by submission ID
        // GET: api/feedback/submission/{submissionId}
        [HttpGet("submission/{submissionId}")]
        public async Task<IActionResult> GetFeedbackBySubmissionId(int submissionId)
        {
            try
            {
                // Fetch feedback associated with the provided submission ID
                var feedbacks = await _context.Feedback
                    .Where(f => f.submission_id == submissionId)
                    .ToListAsync();

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    _logger.LogWarning($"No feedback found for submission ID: {submissionId}");
                    return NotFound(new { message = "No feedback found for the specified submission." });
                }

                _logger.LogInformation($"Feedback retrieved for submission ID: {submissionId}");
                return Ok(new
                {
                    message = "Feedback retrieved successfully.",
                    data = feedbacks
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve feedback for submission ID: {submissionId}", submissionId);
                return StatusCode(500, "Internal server error");
            }
        }


        // View feedback on your submissions
        // GET: api/feedback/submissions/{userId}
        [HttpGet("ownSubmission/{userId}")]
        public async Task<IActionResult> ViewFeedback(int userId)
        {
            try
            {
                var feedbacks = await _context.Feedback
                    .Where(f => f.Submission.user_id == userId)
                    .ToListAsync();

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    _logger.LogWarning($"No feedback found for user ID: {userId}");
                    return NotFound("No feedback found.");
                }

                _logger.LogInformation($"Feedback retrieved for user ID: {userId}");
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve feedback.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Download marks
        // GET: api/feedback/download-marks/{userId}
        [HttpGet("download-marks/{userId}")]
        public async Task<IActionResult> DownloadMarks(int userId)
        {
            try
            {
                var marksData = await _context.Feedback
                    .Where(f => f.Submission.user_id == userId)
                    .Select(f => new { f.mark_received })
                    .ToListAsync();

                if (marksData == null || marksData.Count == 0)
                {
                    _logger.LogWarning($"No marks found for user ID: {userId}");
                    return NotFound("No marks found.");
                }

                // Prepare your file (e.g., CSV)
                var csv = "MarkReceived\n" + string.Join("\n", marksData.Select(m => m.mark_received));

                // Convert to byte array
                var byteArray = System.Text.Encoding.UTF8.GetBytes(csv);
                var stream = new MemoryStream(byteArray);

                _logger.LogInformation($"Marks downloaded successfully for user ID: {userId}");
                return File(stream, "text/csv", "marks.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download marks.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Update feedback on video
        // PUT: api/feedback/update/{feedbackId}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] Feedback updatedFeedback)
        {
            if (updatedFeedback == null || string.IsNullOrEmpty(updatedFeedback.feedback) || updatedFeedback.mark_received == null)
                return BadRequest("Invalid feedback input.");

            try
            {
                var existingFeedback = await _context.Feedback.FindAsync(id);

                if (existingFeedback == null)
                    return NotFound("Feedback not found.");

                // Update the feedback fields
                existingFeedback.feedback = updatedFeedback.feedback;
                existingFeedback.mark_received = updatedFeedback.mark_received;

                _context.Feedback.Update(existingFeedback);
                await _context.SaveChangesAsync();

                // Log success
                Log("Feedback successfully updated for feedback ID: " + id);

                return Ok("Feedback updated successfully.");
            }
            catch (Exception ex)
            {
                // Log failure with exception details
                Log("Failed to update feedback for feedback ID: " + id + ". Error: " + ex);
                return StatusCode(500, "An error occurred while updating feedback.");
            }
        }

        // DELETE: api/feedback/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var feedback = await _context.Feedback.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound(new { message = "Feedback not found." });
                }

                _context.Feedback.Remove(feedback);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Feedback deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new { message = "An error occurred while deleting feedback.", error = ex.Message });
            }
        }

        private void Log(string message)
        {
            // Implement your logging functionality here (e.g., write to a file or database)
            Console.WriteLine(message);
        }
    }
}






