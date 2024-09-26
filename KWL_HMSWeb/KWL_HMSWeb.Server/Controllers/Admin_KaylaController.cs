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
    [Route("api/adminkayla")]
    [ApiController]
    public class Admin_KaylaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public Admin_KaylaController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Admin_Kayla
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin_Kayla>>> GetAdmin_Kayla()
        {
            return await _context.Admin_Kayla.ToListAsync();
        }

        // GET: api/Admin_Kayla/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin_Kayla>> GetAdmin_Kayla(int id)
        {
            var admin_Kayla = await _context.Admin_Kayla.FindAsync(id);

            if (admin_Kayla == null)
            {
                return NotFound();
            }

            return admin_Kayla;
        }

        // PUT: api/Admin_Kayla/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin_Kayla(int id, Admin_Kayla admin_Kayla)
        {
            if (id != admin_Kayla.admin_id)
            {
                return BadRequest();
            }

            _context.Entry(admin_Kayla).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Admin_KaylaExists(id))
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

        // POST: api/Admin_Kayla
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin_Kayla>> PostAdmin_Kayla(Admin_Kayla admin_Kayla)
        {
            _context.Admin_Kayla.Add(admin_Kayla);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin_Kayla", new { id = admin_Kayla.admin_id }, admin_Kayla);
        }

        // DELETE: api/Admin_Kayla/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin_Kayla(int id)
        {
            var admin_Kayla = await _context.Admin_Kayla.FindAsync(id);
            if (admin_Kayla == null)
            {
                return NotFound();
            }

            _context.Admin_Kayla.Remove(admin_Kayla);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Admin_KaylaExists(int id)
        {
            return _context.Admin_Kayla.Any(e => e.admin_id == id);
        }
    }
}
