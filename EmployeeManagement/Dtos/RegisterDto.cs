namespace EmployeeManagement.Dtos;

public class RegisterDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
}
