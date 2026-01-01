namespace EmployeeManagement.Models;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;


    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
}

