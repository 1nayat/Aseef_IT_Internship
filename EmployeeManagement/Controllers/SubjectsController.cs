using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject(CreateSubjectDto dto)
    {
        var subject = new Subject { SubjectName = dto.SubjectName };
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return Ok(subject);
    }
}
