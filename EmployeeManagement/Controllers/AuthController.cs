using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwt;

    public AuthController(AppDbContext context, IJwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

  
    [HttpPost("register")]
    public async Task<IActionResult> register(RegisterDto dto)
    {
        var exists = await _context.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (exists)
            return BadRequest("email already exists");

        var roleExists = await _context.Roles
            .AnyAsync(r => r.RoleId == dto.RoleId);

        if (!roleExists)
            return BadRequest("invalid role");

        var deptExists = await _context.Departments
            .AnyAsync(d => d.DepartmentId == dto.DepartmentId);

        if (!deptExists)
            return BadRequest("invalid department");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = dto.RoleId,
            DepartmentId = dto.DepartmentId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("user registered successfully");
    }

 
    [HttpPost("login")]
    public async Task<IActionResult> login(LoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Role) 
            .FirstOrDefaultAsync(u => u.Email == dto.email);

        if (user == null)
            return Unauthorized("invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(dto.password, user.PasswordHash))
            return Unauthorized("invalid credentials");

        var token = _jwt.generateToken(
            user.UserId,
            user.Email,
            user.Role.RoleName
        );

        return Ok(new
        {
            token,
            user.UserId,
            user.Email,
            role = user.Role.RoleName
        });
    }
}
