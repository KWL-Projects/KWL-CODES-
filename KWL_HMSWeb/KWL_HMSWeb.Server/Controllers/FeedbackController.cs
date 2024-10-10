using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(DatabaseContext context, ILogger<FeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedback()
        {
            return await _context.Feedback.ToListAsync();
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedback.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            if (id != feedback.feedback_id)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            // Log success
            _logger.LogInformation("Feedback successfully added with ID: {FeedbackId}", feedback.feedback_id);

            return CreatedAtAction("GetFeedback", new { id = feedback.feedback_id }, feedback);
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();

            // Log success
            _logger.LogInformation("Feedback successfully deleted with ID: {FeedbackId}", id);

            return NoContent();
        }

        // POST: api/Feedback/submit
        [HttpPost("submit")]
        public async Task<ActionResult> SubmitFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                _logger.LogError("Feedback submission failed: Feedback is null.");
                return BadRequest("Feedback cannot be null.");
            }

            try
            {
                _context.Feedback.Add(feedback);
                await _context.SaveChangesAsync();

                // Log success
                _logger.LogInformation("Feedback successfully submitted with ID: {FeedbackId}", feedback.feedback_id);

                return Ok(new { Message = "Feedback submitted successfully.", FeedbackId = feedback.feedback_id });
            }
            catch (Exception ex)
            {
                // Log failure
                _logger.LogError(ex, "Feedback submission failed.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while submitting feedback.");
            }
        }

        // GET: api/Feedback/view/{submissionId}
        [HttpGet("view/{submissionId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> ViewFeedback(int submissionId)
        {
            try
            {
                var feedbackList = await _context.Feedback
                    .Where(f => f.submission_id == submissionId)
                    .ToListAsync();

                if (feedbackList == null || !feedbackList.Any())
                {
                    _logger.LogWarning("No feedback found for submission ID: {SubmissionId}", submissionId);
                    return NotFound(new { Message = "No feedback found for this submission." });
                }

                // Log success
                _logger.LogInformation("Feedback retrieved for submission ID: {SubmissionId}", submissionId);

                return Ok(feedbackList);
            }
            catch (Exception ex)
            {
                // Log failure
                _logger.LogError(ex, "Error retrieving feedback for submission ID: {SubmissionId}", submissionId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving feedback.");
            }
        }

        // GET: api/Feedback/download-marks/{submissionId}
        [HttpGet("download-marks/{submissionId}")]
        public async Task<IActionResult> DownloadMarks(int submissionId)
        {
            try
            {
                // Retrieve the marks data for the given submission ID
                var feedbackList = await _context.Feedback
                    .Where(f => f.submission_id == submissionId)
                    .ToListAsync();

                if (feedbackList == null || !feedbackList.Any())
                {
                    _logger.LogWarning("No feedback found for submission ID: {SubmissionId}", submissionId);
                    return NotFound(new { Message = "No feedback found for this submission." });
                }

                // Prepare the data into a CSV format
                var csvData = new StringBuilder();
                csvData.AppendLine("Feedback ID, Submission ID, User ID, Feedback, Mark Received");

                foreach (var feedback in feedbackList)
                {
                    csvData.AppendLine($"{feedback.feedback_id}, {feedback.submission_id}, {feedback.user_id}, \"{feedback.feedback}\", {feedback.mark_received}");
                }

                // Convert CSV data to byte array
                var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

                // Log success
                _logger.LogInformation("Marks file successfully prepared for submission ID: {SubmissionId}", submissionId);

                // Set headers to force file download
                var fileContentResult = new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = $"Marks_Submission_{submissionId}.csv"
                };

                return fileContentResult;
            }
            catch (Exception ex)
            {
                // Log failure
                _logger.LogError(ex, "Error occurred while downloading marks for submission ID: {SubmissionId}", submissionId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while downloading marks.");
            }
        }


        private bool FeedbackExists(int id)
        {
            return _context.Feedback.Any(e => e.feedback_id == id);
        }
    }
}
