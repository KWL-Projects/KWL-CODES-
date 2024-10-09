﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net; 

namespace KWL_HMSWeb.Server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext context, ILogger<UserController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                // Check if the username already exists
                if (await _context.Login.AnyAsync(u => u.username == userRegisterDto.username))
                {
                    _logger.LogWarning($"Registration failed: Username '{userRegisterDto.username}' already exists.");
                    return BadRequest(new { message = "Username already exists" });
                }

                // Log the received registration details
                _logger.LogInformation($"Received registration: Username - {userRegisterDto.username}");

                // Create a new Login object and hash the password
                var login = new Login
                {
                    username = userRegisterDto.username,
                    password = HashPassword(userRegisterDto.password) // Hash the password
                };

                // Add login object to the database
                _context.Login.Add(login);
                await _context.SaveChangesAsync();

                // After saving login, retrieve the login_id (auto-generated by the DB)
                var savedLogin = await _context.Login.SingleOrDefaultAsync(u => u.username == login.username);

                // Create a new User object and associate it with the login
                var newUser = new User
                {
                    login_id = savedLogin.login_id,
                    user_firstname = userRegisterDto.user_firstname,
                    user_surname = userRegisterDto.user_surname,
                    user_type = userRegisterDto.user_type // Use the user_type from the DTO
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Log successful registration
                _logger.LogInformation($"User registered successfully: Username - {userRegisterDto.username}");

                // Generate a JWT token after registration
                var token = GenerateJwtToken(savedLogin, newUser.user_type);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token generation failed after registration.");
                    return Ok(new
                    {
                        message = "Registration successful, but token generation failed",
                        login_id = savedLogin.login_id,
                        error = "Token generation failed"
                    });
                }

                // Log token generation success
                _logger.LogInformation("Token generated successfully after registration.");

                return CreatedAtAction(nameof(Register), new { id = savedLogin.login_id }, new { message = "Registration successful", token });
            }
            catch (Exception ex)
            {
                // Log the full exception
                _logger.LogError($"Registration failed: {ex.ToString()}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/User/create
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Do not set user_id, let EF handle it
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New user created with ID {user.user_id}.");
            return CreatedAtAction("GetUser", new { id = user.user_id }, new { message = "User created successfully", data = user });
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
        public async Task<IActionResult> PutUser(int id, User updatedUser)
        {
            if (id != updatedUser.user_id)
            {
                _logger.LogWarning("User ID mismatch.");
                return BadRequest(new { message = "User ID mismatch" });
            }

            var existingUser = await _context.Users.Include(u => u.Login).FirstOrDefaultAsync(u => u.user_id == id);

            if (existingUser == null)
            {
                _logger.LogError($"User with ID {id} not found during update.");
                return NotFound(new { message = "User not found" });
            }

            // Prevent modification of user_id and login_id
            updatedUser.user_id = existingUser.user_id;
            updatedUser.login_id = existingUser.login_id;

            // Optionally prevent modification of Login details (if needed)
            updatedUser.Login.login_id = existingUser.Login.login_id;

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);

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

        // BCrypt Password Hashing Helper Method
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string GenerateJwtToken(Login user, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve JWT settings from IConfiguration
            var jwtSecret = _configuration["KWLCodes_JWT_SECRET"];
            var jwtIssuer = _configuration["KWLCodes_JWT_ISSUER"];
            var jwtAudience = _configuration["KWLCodes_JWT_AUDIENCE"];

            if (string.IsNullOrEmpty(jwtSecret) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                _logger.LogWarning("JWT configuration is missing");
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.user_id == id);
        }
    }
}

