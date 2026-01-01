using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/assignments")]
public class TeacherAssignmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeacherAssignmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AssignTeacher(AssignTeacherDto dto)
    {
        if (!await _context.Teachers.AnyAsync(t => t.TeacherId == dto.TeacherId))
            return NotFound("Teacher not found");

        if (!await _context.Classes.AnyAsync(c => c.ClassId == dto.ClassId))
            return NotFound("Class not found");

        if (!await _context.Subjects.AnyAsync(s => s.SubjectId == dto.SubjectId))
            return NotFound("Subject not found");

        bool exists = await _context.TeacherClassSubjects.AnyAsync(x =>
            x.TeacherId == dto.TeacherId &&
            x.ClassId == dto.ClassId &&
            x.SubjectId == dto.SubjectId);

        if (exists)
            return BadRequest("Assignment already exists");

        var assignment = new TeacherClassSubject
        {
            TeacherId = dto.TeacherId,
            ClassId = dto.ClassId,
            SubjectId = dto.SubjectId
        };

        _context.TeacherClassSubjects.Add(assignment);
        await _context.SaveChangesAsync();

        return Ok("Teacher assigned successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        var data = await _context.TeacherClassSubjects
            .Include(x => x.Teacher)
            .Include(x => x.Class)
            .Include(x => x.Subject)

            .Select(x => new
            {
                Teacher = x.Teacher.Name,
                Class = x.Class.ClassName,
                Subject = x.Subject.SubjectName
            })
            .ToListAsync();

        return Ok(data);
    }

}
