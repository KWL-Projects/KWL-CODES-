using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private static List<User> users = new List<User>
    {
        new User { Id = 1, Username = "admin", Password = "password" },
        new User { Id = 2, Username = "user1", Password = "password" }
    };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpPost]
    public IActionResult AddUser([FromBody] User newUser)
    {
        newUser.Id = users.Max(u => u.Id) + 1;
        users.Add(newUser);
        return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = users.Find(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        user.Username = updatedUser.Username;
        user.Password = updatedUser.Password;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = users.Find(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        users.Remove(user);
        return NoContent();
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
