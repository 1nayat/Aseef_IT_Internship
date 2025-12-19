using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Dtos;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        return await _context.Departments.ToListAsync();
    }
}
