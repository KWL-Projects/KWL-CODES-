using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KWL_HMSWeb.Server.Models;

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

        // POST: api/assignment/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromBody] Assignment assignment)
        {
            // Date verification logic
            if (assignment.due_date < DateTime.Now)
            {
                _logger.LogError("Assignment creation failed: Due date is in the past.");
                return BadRequest("Due date must be in the future.");
            }

            try
            {
                _context.Assignment.Add(assignment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Assignment created successfully: Assignment ID {assignment.assignment_id}");
                return Ok(new { message = "Assignment created successfully.", assignment_id = assignment.assignment_id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating assignment: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // GET: api/assignment/view/{id}
        [HttpGet("view/{id}")]
        public async Task<IActionResult> ViewAssignment(int id)
        {
            try
            {
                var assignment = await _context.Assignment.FindAsync(id);
                if (assignment == null)
                {
                    _logger.LogWarning($"Assignment with ID {id} not found.");
                    return NotFound($"Assignment with ID {id} not found.");
                }

                _logger.LogInformation($"Assignment with ID {id} retrieved successfully.");
                return Ok(assignment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving assignment: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // GET: api/assignment/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                var assignments = await _context.Assignment.ToListAsync();

                if (!assignments.Any())
                {
                    _logger.LogWarning("No assignments found.");
                    return NotFound("No assignments found.");
                }

                _logger.LogInformation("All assignments retrieved successfully.");
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving assignments: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // PUT: api/assignment/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] Assignment updatedAssignment)
        {
            if (id != updatedAssignment.assignment_id)
            {
                _logger.LogError("Assignment update failed: Assignment ID mismatch.");
                return BadRequest("Assignment ID mismatch.");
            }

            _context.Entry(updatedAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Assignment with ID {id} updated successfully.");
                return Ok(new { message = "Assignment updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    _logger.LogWarning($"Assignment with ID {id} not found.");
                    return NotFound($"Assignment with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating assignment: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        // DELETE: api/assignment/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            try
            {
                var assignment = await _context.Assignment.FindAsync(id);
                if (assignment == null)
                {
                    _logger.LogWarning($"Assignment with ID {id} not found.");
                    return NotFound($"Assignment with ID {id} not found.");
                }

                _context.Assignment.Remove(assignment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Assignment with ID {id} deleted successfully.");
                return Ok(new { message = "Assignment deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting assignment: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.assignment_id == id);
        }
    }
}



