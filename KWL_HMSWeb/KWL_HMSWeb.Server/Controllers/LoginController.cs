using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration; // For reading configurations like JWT secret

        public LoginController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogin()
        {
            return await _context.Login.ToListAsync();
        }

        // GET: api/Login/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return login;
        }

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogin(int id, Login login)
        {
            if (id != login.login_id)
            {
                return BadRequest();
            }

            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
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

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Login loginRequest)
        {
            // Fetch user by username
            var login = await _context.Login.FirstOrDefaultAsync(u => u.username == loginRequest.username);

            if (login == null || !BCrypt.Net.BCrypt.Verify(loginRequest.password, login.password))
            {
                // Log the failed attempt
                LogFailure(loginRequest.username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var userDetail = await GetUserDetails(login.login_id);

            if(userDetail == null)
            {
                LogFailure(loginRequest.username);
                return Unauthorized(new { message = "User not registered" });
            }

            // Generate JWT token
            var token = GenerateJwtToken(login, userDetail);

            // Log the successful login
            LogSuccess(loginRequest.username);

            return Ok(new { message = "Login successful", token, userDetail });
        }

        private async Task<object> GetUserDetails(int loginId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.login_id == loginId);
            if (user != null)
            {
                return new { Details = user };
            }
            return null;
        }

        private string GenerateJwtToken(Login user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.login_id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void LogSuccess(string username)
        {
            // Log the successful login attempt (Implement your logging logic here)
            Console.WriteLine($"User {username} successfully logged in at {DateTime.UtcNow}");
        }

        private void LogFailure(string username)
        {
            // Log the failed login attempt (Implement your logging logic here)
            Console.WriteLine($"Failed login attempt for user {username} at {DateTime.UtcNow}");
        }

        /*[HttpPost]
        public async Task<ActionResult<Login>> PostLogin(Login login)
        {
            _context.Login.Add(login);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogin", new { id = login.login_id }, login);
        }*/

        [HttpPost]
        public async Task<ActionResult<Login>> Register(Login login)
        {
            // Encrypt password before saving
            login.password = BCrypt.Net.BCrypt.HashPassword(login.password);

            _context.Login.Add(login);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogin", new { id = login.login_id }, login);
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            _context.Login.Remove(login);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginExists(int id)
        {
            return _context.Login.Any(e => e.login_id == id);
        }
    }
}
