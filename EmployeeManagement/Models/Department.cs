namespace EmployeeManagement.Models;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
