using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Dtos;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Role>> AddRole(CreateRoleDto dto)
    {
        var role = new Role
        {
            RoleName = dto.RoleName
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return Ok(role);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }
}
