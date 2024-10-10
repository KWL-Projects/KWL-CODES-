using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/lecturer")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LecturerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Lecturer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers()
        {
            var lecturers = await _context.Lecturer.Include(l => l.User).ToListAsync();
            if (lecturers == null || !lecturers.Any())
            {
                LogFailure("No lecturers found.");
                return NotFound(new { message = "No lecturers found" });
            }

            return lecturers;
        }

        // GET: api/Lecturer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetLecturer(int id)
        {
            var lecturer = await _context.Lecturer.Include(l => l.User)
                                                  .FirstOrDefaultAsync(l => l.user_id == id);

            if (lecturer == null)
            {
                LogFailure($"Lecturer with ID {id} not found.");
                return NotFound(new { message = "Lecturer not found" });
            }

            return lecturer;
        }

        // PUT: api/Lecturer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecturer(int id, Lecturer lecturer)
        {
            if (id != lecturer.user_id)
            {
                LogFailure("Lecturer ID mismatch.");
                return BadRequest(new { message = "Lecturer ID mismatch" });
            }

            _context.Entry(lecturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                LogSuccess($"Lecturer with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(id))
                {
                    LogFailure($"Lecturer with ID {id} not found.");
                    return NotFound(new { message = "Lecturer not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Lecturer updated successfully" });
        }

        // POST: api/Lecturer
        [HttpPost]
        public async Task<ActionResult<Lecturer>> CreateLecturer(Lecturer lecturer)
        {
            try
            {
                _context.Lecturer.Add(lecturer);
                await _context.SaveChangesAsync();

                LogSuccess($"Lecturer with ID {lecturer.user_id} created successfully.");
                return CreatedAtAction(nameof(GetLecturer), new { id = lecturer.user_id }, lecturer);
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to create lecturer: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating the lecturer." });
            }
        }

        // DELETE: api/Lecturer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var lecturer = await _context.Lecturer.FindAsync(id);
            if (lecturer == null)
            {
                LogFailure($"Lecturer with ID {id} not found.");
                return NotFound(new { message = "Lecturer not found" });
            }

            try
            {
                _context.Lecturer.Remove(lecturer);
                await _context.SaveChangesAsync();

                LogSuccess($"Lecturer with ID {id} deleted successfully.");
                return Ok(new { message = "Lecturer deleted successfully" });
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to delete lecturer: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the lecturer." });
            }
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturer.Any(e => e.user_id == id);
        }

        private void LogSuccess(string message)
        {
            // Log the successful action (Implement your logging logic here)
            Console.WriteLine($"SUCCESS: {message} at {DateTime.UtcNow}");
        }

        private void LogFailure(string message)
        {
            // Log the failed action (Implement your logging logic here)
            Console.WriteLine($"FAILURE: {message} at {DateTime.UtcNow}");
        }
    }
}

