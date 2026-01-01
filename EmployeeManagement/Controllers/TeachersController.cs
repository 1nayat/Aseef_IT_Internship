using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeachersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher(CreateTeacherDto dto)
    {
        var teacher = new Teacher { Name = dto.Name };
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return Ok(teacher);
    }
}
