using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using KWLCodesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KWLCodesAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogin()
        {
            return await _context.Login.ToListAsync();
        }

        /*[HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(long id)
        {
            var Login = await _context.Login.FindAsync(id);

            if (Login == null)
            {
                return NotFound();
            }

            return ItemToDTO(Login);
        }*/
    }
}
