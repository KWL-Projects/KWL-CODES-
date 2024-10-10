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
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.Admin)
                                      .Include(u => u.Student)
                                      .Include(u => u.Lecturer)
                                      .ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("view/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Admin)
                                          .Include(u => u.Student)
                                          .Include(u => u.Lecturer)
                                          .FirstOrDefaultAsync(u => u.user_id == id);

            if (user == null)
            {
                LogFailure($"User with ID {id} not found.");
                return NotFound(new { message = "User not found" });
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.user_id)
            {
                LogFailure("User ID mismatch.");
                return BadRequest(new { message = "User ID mismatch" });
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                LogSuccess($"User with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    LogFailure($"User with ID {id} not found.");
                    return NotFound(new { message = "User not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "User updated successfully" });
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                LogSuccess($"User with ID {user.user_id} created successfully.");
                return CreatedAtAction(nameof(GetUser), new { id = user.user_id }, user);
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to create user: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating the user." });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                LogFailure($"User with ID {id} not found.");
                return NotFound(new { message = "User not found" });
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                LogSuccess($"User with ID {id} deleted successfully.");
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to delete user: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the user." });
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.user_id == id);
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
