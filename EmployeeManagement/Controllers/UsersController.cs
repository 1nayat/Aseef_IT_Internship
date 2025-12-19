using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Dtos;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddUser(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            RoleId = dto.RoleId,
            DepartmentId = dto.DepartmentId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .Select(u => new
            {
                u.UserId,
                u.Name,
                u.Email,
                Role = u.Role.RoleName,
                Department = u.Department.DepartmentName
            })
            .ToListAsync();

        return Ok(users);
    }


   
}
