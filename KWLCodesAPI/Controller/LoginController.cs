using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KWLCodesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> Users = new List<User>();

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists.");
            }

            user.Password = HashPassword(user.Password);
            Users.Add(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Username == user.Username && u.Password == HashPassword(user.Password));
            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate a token (for simplicity, using a static token here)
            var token = GenerateToken(user.Username);
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private string GenerateToken(string username)
        {
            // Implement token generation logic here (e.g., JWT)
            return "your-secure-token";
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
