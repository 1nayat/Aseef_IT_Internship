using EmployeeManagement.Data;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClassesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{classId}/details")]
    public async Task<IActionResult> GetClassDetails(int classId)
    {
        var cls = await _context.Classes
            .FirstOrDefaultAsync(c => c.ClassId == classId && !c.IsDeleted);

        if (cls == null)
            return NotFound("Class not found");

        var assignments = await _context.TeacherClassSubjects
            .Where(tcs => tcs.ClassId == classId)
            .Include(tcs => tcs.Teacher)
            .Include(tcs => tcs.Subject)
            .ToListAsync();

        var teacherSubjects = assignments
            .GroupBy(a => new { a.Teacher.TeacherId, a.Teacher.Name })
            .Select(g => new TeacherSubjectDto
            {
                TeacherId = g.Key.TeacherId,
                TeacherName = g.Key.Name,
                Subjects = g
                    .Select(x => x.Subject.SubjectName)
                    .Distinct()
                    .ToList()
            })
            .ToList();

        var result = new ClassDetailsDto
        {
            ClassId = cls.ClassId,
            ClassName = cls.ClassName,
            TeacherSubjects = teacherSubjects
        };

        return Ok(result);
    }

    [HttpPost("Upsert")]
    public async Task<IActionResult> CreateOrUpdateClass(CreateClassDto dto)
    {
        if (dto.ClassId > 0)
        {
            var existing = await _context.Classes
                .FirstOrDefaultAsync(c => c.ClassId == dto.ClassId);

            if (existing == null)
                return NotFound("Class not found");

            if (existing.IsDeleted)
                return BadRequest("Cannot update a deleted class");

            existing.ClassName = dto.ClassName;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        var cls = new Class
        {
            ClassName = dto.ClassName
        };

        _context.Classes.Add(cls);
        await _context.SaveChangesAsync();

        return Ok(cls);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteClass(int id)
    {
        var cls = await _context.Classes.FindAsync(id);

        if (cls == null)
            return NotFound("Class not found");

        cls.IsDeleted = true; 
        await _context.SaveChangesAsync();

        return Ok("Class soft deleted");
    }
    [HttpGet]
    public async Task<IActionResult> GetClasses()
    {
        return Ok(await _context.Classes
            .Where(c => !c.IsDeleted)
            .ToListAsync());
    }

}
