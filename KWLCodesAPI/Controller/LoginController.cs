using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Login>>> GetLogins()
        {
            return await _context.Login.ToListAsync();
        }
    }
}
