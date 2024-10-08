using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KWL_HMSWeb.Server.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Secure Login Method
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Login login)
        {
            try
            {
                var user = await _context.Login.SingleOrDefaultAsync(u => u.username == login.username);
                if (user == null)
                {
                    Log("Login failed: User not found", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Verify password with BCrypt
                if (!VerifyPassword(login.password, user.password))
                {
                    Log("Login failed: Incorrect password", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Retrieve the user type
                var userDetail = await _context.Users.SingleOrDefaultAsync(u => u.login_id == user.login_id);
                if (userDetail == null)
                {
                    Log("Login failed: User details not found", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Generate JWT token on success, including user type
                var token = GenerateJwtToken(user, userDetail.user_type);
                Log("Login successful", true);
                return Ok(new { message = "Login successful", token });
            }
            catch (Exception ex)
            {
                Log($"Login failed: {ex.Message}", false);
                return StatusCode(500, "Internal server error");
            }
        }

        // Register Method
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Login login)
        {
            try
            {
                // Check if username already exists
                if (await _context.Login.AnyAsync(u => u.username == login.username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                // Hash the password with BCrypt
                login.password = HashPassword(login.password);

                _context.Login.Add(login);
                await _context.SaveChangesAsync();

                Log("User registered successfully", true);
                return CreatedAtAction(nameof(GetLogin), new { id = login.login_id }, new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                Log($"Registration failed: {ex.Message}", false);
                return StatusCode(500, "Internal server error");
            }
        }

        // BCrypt Password Hashing Helper Method
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // BCrypt Password Verification Helper Method
        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }

        // JWT Token Generation using environment variables
        private string GenerateJwtToken(Login user, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve JWT settings from environment variables
            var jwtSecret = Environment.GetEnvironmentVariable("KWLCodes_JWT_SECRET");
            var jwtIssuer = Environment.GetEnvironmentVariable("KWLCodes_JWT_ISSUER");
            var jwtAudience = Environment.GetEnvironmentVariable("KWLCodes_JWT_AUDIENCE");

            var key = Encoding.UTF8.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.login_id.ToString()),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, userType) // Include user type
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Logging Helper Method
        private void Log(string message, bool success)
        {
            Console.WriteLine($"[{DateTime.Now}] {(success ? "SUCCESS" : "FAILURE")}: {message}");
        }

        // GET: api/Login/5
        [HttpGet("view/{id}")]
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
        [HttpPut("update/{id}")]
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

        // DELETE: api/Login/{id}
        [HttpDelete("delete/{id}")]
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
