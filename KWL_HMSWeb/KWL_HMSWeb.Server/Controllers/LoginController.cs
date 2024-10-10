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
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous] // This allows access to the login endpoint
        public async Task<IActionResult> Authenticate([FromBody] Login login)
        {
            try
            {
                // Log the username being authenticated
                Log($"Attempting login for username: {login.username}", true);

                // Fetch user by username
                var user = await _context.Login.SingleOrDefaultAsync(u => u.username == login.username);
                if (user == null)
                {
                    Log("Login failed: User not found", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Log the stored password (for debugging)
                Log($"Stored hashed password: {user.password}", true);

                // Verify password with BCrypt
                Log("Verifying password", true);
                if (!VerifyPassword(login.password, user.password))
                {
                    Log($"Login failed: Incorrect password for user {login.username}", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                Log("Password verification successful", true);

                // Retrieve user details
                var userDetail = await _context.Users.SingleOrDefaultAsync(u => u.login_id == user.login_id);
                if (userDetail == null)
                {
                    Log("Login failed: User details not found", false);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Log the user type for debugging
                Log($"User type: {userDetail.user_type}", true);

                // Try to generate JWT token
                string token = null;
                try
                {
                    token = GenerateJwtToken(user, userDetail.user_type);
                }
                catch (Exception ex)
                {
                    Log($"Token generation failed: {ex.Message}", false);
                    // Log the token generation failure, but do not stop the successful login message
                }

                // Log login success even if token fails
                Log("Login successful", true);

                // Return a combined response: login success, and error if token generation failed
                return Ok(new
                {
                    message = "Login successful",
                    token = token, // Can be null if token generation failed
                    tokenError = token == null ? "Token generation failed" : null // Include token error only if token is null
                });
            }
            catch (Exception ex)
            {
                // Log the full exception stack trace
                Log($"Login failed: {ex.ToString()}", false);
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}", details = ex.ToString() });
            }
        }

        // GET: api/Login/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Login>>> GetAllLogins()
        {
            var logins = await _context.Login.ToListAsync();

            if (logins == null || !logins.Any())
            {
                return NotFound("No logins found.");
            }

            return Ok(logins); // Return the list of logins
        }

        // GET: api/Login/view/5
        [HttpGet("view/{id}")]
        public async Task<ActionResult<object>> GetLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);

            if (login == null)
            {
                return NotFound("Login not found.");
            }

            var response = new
            {
                Message = "Login retrieved successfully.",
                Data = login
            };

            return Ok(response); // Return success message and the login object
        }

        // PUT: api/Login/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutLogin(int id, Login login)
        {
            if (id != login.login_id)
            {
                return BadRequest(new { message = "ID mismatch: provided ID does not match the login ID.", success = false });
            }

            // Fetch the existing login from the database
            var existingLogin = await _context.Login.FindAsync(id);
            if (existingLogin == null)
            {
                return NotFound(new { message = "Login not found for the provided ID.", success = false });
            }

            // Update the username if it has changed
            if (existingLogin.username != login.username)
            {
                existingLogin.username = login.username;
            }

            // Hash the password if it has changed
            if (!string.IsNullOrEmpty(login.password) && !VerifyPassword(login.password, existingLogin.password))
            {
                existingLogin.password = HashPassword(login.password);
            }

            _context.Entry(existingLogin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
                {
                    return NotFound(new { message = "Login not found for the provided ID during save.", success = false });
                }
                else
                {
                    return StatusCode(500, new { message = "An error occurred while updating the login details.", success = false });
                }
            }

            // Return success message along with updated login details
            return Ok(new
            {
                message = "Login updated successfully.",
                success = true,
                updatedLogin = new
                {
                    login_id = existingLogin.login_id,
                    username = existingLogin.username
                }
            });
        }

        // DELETE: api/Login/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLogin(int id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login == null)
            {
                return NotFound("Login not found.");
            }

            _context.Login.Remove(login);
            await _context.SaveChangesAsync();

            return Ok("Login deleted successfully.");
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

        // JWT Token Generation using IConfiguration (instead of Environment.GetEnvironmentVariable)
        private string GenerateJwtToken(Login user, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve JWT settings from IConfiguration
            var jwtSecret = _configuration["KWLCodes_JWT_SECRET"];
            var jwtIssuer = _configuration["KWLCodes_JWT_ISSUER"];
            var jwtAudience = _configuration["KWLCodes_JWT_AUDIENCE"];

            if (string.IsNullOrEmpty(jwtSecret) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                Log("JWT configuration is missing", false);
                return null; // Return null if JWT config is missing
            }

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

        private bool LoginExists(int id)
        {
            return _context.Login.Any(e => e.login_id == id);
        }

        // Register Method
        /*[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Login login)
        {
            try
            {
                // Check if username already exists
                if (await _context.Login.AnyAsync(u => u.username == login.username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                // Log the received registration details (excluding login_id)
                Log($"Received registration: Username - {login.username}", true);

                // Ensure login_id is not set (let the database auto-generate it)
                login.login_id = 0; // Or, remove this line entirely if unnecessary

                // Hash the password
                login.password = HashPassword(login.password);

                // Add login object to the database
                _context.Login.Add(login);
                await _context.SaveChangesAsync();

                // After saving login, retrieve the login_id (auto-generated by the DB)
                var savedLogin = await _context.Login.SingleOrDefaultAsync(u => u.username == login.username);

                // You can assign a default user role here, or pass the role via the request body
                // Example: Assign the user role dynamically or use a default (e.g., "student")
                string userType = "student"; // Default role

                // Create a new User object and save it to associate with the login
                var newUser = new User
                {
                    login_id = savedLogin.login_id,
                    user_firstname = "DefaultFirstName", // Replace or pass these fields dynamically
                    user_surname = "DefaultLastName",
                    user_type = userType
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                Log("User registered successfully", true);

                // Generate a JWT token after registration
                var token = GenerateJwtToken(savedLogin, userType);
                if (token == null)
                {
                    // If token generation fails, log and return a partial success response
                    Log("Token generation failed", false);
                    return Ok(new
                    {
                        message = "Registration successful, but token generation failed",
                        login_id = savedLogin.login_id,
                        error = "Token generation failed"
                    });
                }

                Log("Registration and token generation successful", true);

                return CreatedAtAction(nameof(GetLogin), new { id = savedLogin.login_id }, new { message = "Registration successful", token });
            }
            catch (Exception ex)
            {
                // Log the full exception
                Log($"Registration failed: {ex.ToString()}", false);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }*/
    }
}
