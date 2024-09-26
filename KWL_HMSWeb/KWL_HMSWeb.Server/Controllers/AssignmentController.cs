using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging;
namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AssignmentController> _logger;

        public AssignmentController(DatabaseContext context, ILogger<AssignmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Assignment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignment()
        {
            try
            {
                var assignments = await _context.Assignment.ToListAsync();
                _logger.LogInformation("Assignments retrieved successfully.");
                return assignments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assignments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Assignment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            try
            {
                var assignment = await _context.Assignment.FindAsync(id);

                if (assignment == null)
                {
                    _logger.LogWarning("Assignment with id {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Assignment with id {Id} retrieved successfully.", id);
                return assignment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assignment with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Assignment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, Assignment assignment)
        {
            if (id != assignment.assignment_id)
            {
                _logger.LogWarning("Assignment id mismatch.");
                return BadRequest();
            }

            _context.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Assignment with id {Id} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    _logger.LogWarning("Assignment with id {Id} not found.", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency error updating assignment with id {Id}.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assignment with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/Assignment
        [HttpPost]
        public async Task<ActionResult<Assignment>> PostAssignment(Assignment assignment)
        {
            // Data verification
            if (string.IsNullOrEmpty(assignment.subject_id) || string.IsNullOrEmpty(assignment.assignment_name) || assignment.due_date == default)
            {
                _logger.LogWarning("Invalid assignment data provided.");
                return BadRequest("Invalid assignment data.");
            }

            try
            {
                _context.Assignment.Add(assignment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Assignment created successfully with id {Id}.", assignment.assignment_id);

                return CreatedAtAction("GetAssignment", new { id = assignment.assignment_id }, assignment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assignment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Assignment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            try
            {
                var assignment = await _context.Assignment.FindAsync(id);
                if (assignment == null)
                {
                    _logger.LogWarning("Assignment with id {Id} not found.", id);
                    return NotFound();
                }

                _context.Assignment.Remove(assignment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Assignment with id {Id} deleted successfully.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting assignment with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.assignment_id == id);
        }
    }
}
