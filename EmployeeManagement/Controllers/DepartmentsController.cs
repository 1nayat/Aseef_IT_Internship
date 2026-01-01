using System.Security.Claims;
using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

 
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Department>> AddDepartment(CreateDepartmentDto dto)
    {
        var dept = new Department
        {
            DepartmentName = dto.DepartmentName
        };

        _context.Departments.Add(dept);
        await _context.SaveChangesAsync();

        return Ok(dept);
    }

   
    [Authorize(Roles = "Admin")]
    [HttpPost("upsert")]
    public async Task<IActionResult> UpsertDepartment(UpsertDepartmentDto dto)
    {
        var existing = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == dto.DepartmentId);

        if (existing == null)
        {
            var dept = new Department
            {
                DepartmentName = dto.DepartmentName
            };

            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartments), new { id = dept.DepartmentId }, dept);
        }

        existing.DepartmentName = dto.DepartmentName;
        await _context.SaveChangesAsync();

        return Ok(existing);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = int.Parse(userIdClaim);

        if (role == "Admin")
        {
            return await _context.Departments
                .AsNoTracking()
                .ToListAsync();
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            return Unauthorized();

        var department = await _context.Departments
            .AsNoTracking()
            .Where(d => d.DepartmentId == user.DepartmentId)
            .ToListAsync();

        return department;
    }
}
