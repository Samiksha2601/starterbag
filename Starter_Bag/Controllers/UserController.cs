using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starter_Bag.Data;
using Starter_Bag.Entities;
using Microsoft.AspNetCore.Identity;
namespace Starter_Bag.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly DataContext _context;

    public UserController(DataContext context)  
    {
        _context = context;
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<UserGetDto>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();

        var result = users.Select(u => new UserGetDto
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            CreatedDate = u.CreatedDate,
            UpdatedDate = u.UpdatedDate
        });

        return Ok(result);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<UserGetDto>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return NotFound();

        return Ok(new UserGetDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate

        });
    }

    [HttpPost]

    public async Task<ActionResult<UserGetDto>> Create(UserCreateDto dto)
    {
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password,
            CreatedDate = DateTimeOffset.UtcNow,
            UpdatedDate = DateTimeOffset.UtcNow,
        };
        
        user.Password = passwordHasher.HashPassword(user, dto.Password);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = new UserGetDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };
        
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, result);

    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return NotFound();
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

}