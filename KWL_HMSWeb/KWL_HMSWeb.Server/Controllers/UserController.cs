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
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(DatabaseContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/User/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            _logger.LogInformation("Retrieved all users.");
            return Ok(new { message = "Success", data = users });
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound(new { message = "User not found" });
            }

            _logger.LogInformation($"User with ID {id} retrieved.");
            return Ok(new { message = "Success", data = user });
        }

        // PUT: api/User/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.user_id)
            {
                _logger.LogWarning("User ID mismatch.");
                return BadRequest(new { message = "User ID mismatch" });
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogError($"User with ID {id} not found during update.");
                    return NotFound(new { message = "User not found" });
                }
                else
                {
                    _logger.LogError($"Concurrency error during update for User ID {id}.");
                    throw;
                }
            }

            return Ok(new { message = "User updated successfully" });
        }

        // POST: api/User/create
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New user created with ID {user.user_id}.");
            return CreatedAtAction("GetUser", new { id = user.user_id }, new { message = "User created successfully", data = user });
        }

        // DELETE: api/User/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"Attempted to delete user with ID {id}, but user was not found.");
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with ID {id} deleted successfully.");
            return Ok(new { message = "User deleted successfully" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.user_id == id);
        }
    }
}

