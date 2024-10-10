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
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public StudentController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Student.Include(s => s.User).ToListAsync();
            if (students == null || !students.Any())
            {
                LogFailure("No students found.");
                return NotFound(new { message = "No students found" });
            }

            return students;
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Student.Include(s => s.User)
                                                .FirstOrDefaultAsync(s => s.user_id == id);

            if (student == null)
            {
                LogFailure($"Student with ID {id} not found.");
                return NotFound(new { message = "Student not found" });
            }

            return student;
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.user_id)
            {
                LogFailure("Student ID mismatch.");
                return BadRequest(new { message = "Student ID mismatch" });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                LogSuccess($"Student with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    LogFailure($"Student with ID {id} not found.");
                    return NotFound(new { message = "Student not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Student updated successfully" });
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                _context.Student.Add(student);
                await _context.SaveChangesAsync();

                LogSuccess($"Student with ID {student.user_id} created successfully.");
                return CreatedAtAction(nameof(GetStudent), new { id = student.user_id }, student);
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to create student: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating the student." });
            }
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                LogFailure($"Student with ID {id} not found.");
                return NotFound(new { message = "Student not found" });
            }

            try
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();

                LogSuccess($"Student with ID {id} deleted successfully.");
                return Ok(new { message = "Student deleted successfully" });
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to delete student: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the student." });
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.user_id == id);
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

