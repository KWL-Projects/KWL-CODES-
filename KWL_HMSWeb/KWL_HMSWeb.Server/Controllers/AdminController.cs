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
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _context.Admin.Include(a => a.User).ToListAsync();
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _context.Admin.Include(a => a.User)
                                            .FirstOrDefaultAsync(a => a.user_id == id);

            if (admin == null)
            {
                LogFailure($"Admin with ID {id} not found.");
                return NotFound(new { message = "Admin not found" });
            }

            return admin;
        }

        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, Admin admin)
        {
            if (id != admin.user_id)
            {
                LogFailure("Admin ID mismatch.");
                return BadRequest(new { message = "Admin ID mismatch" });
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                LogSuccess($"Admin with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    LogFailure($"Admin with ID {id} not found.");
                    return NotFound(new { message = "Admin not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Admin updated successfully" });
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult<Admin>> CreateAdmin(Admin admin)
        {
            try
            {
                _context.Admin.Add(admin);
                await _context.SaveChangesAsync();

                LogSuccess($"Admin with ID {admin.user_id} created successfully.");
                return CreatedAtAction(nameof(GetAdmin), new { id = admin.user_id }, admin);
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to create admin: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating the admin." });
            }
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                LogFailure($"Admin with ID {id} not found.");
                return NotFound(new { message = "Admin not found" });
            }

            try
            {
                _context.Admin.Remove(admin);
                await _context.SaveChangesAsync();

                LogSuccess($"Admin with ID {id} deleted successfully.");
                return Ok(new { message = "Admin deleted successfully" });
            }
            catch (Exception ex)
            {
                LogFailure($"Failed to delete admin: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the admin." });
            }
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.user_id == id);
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
