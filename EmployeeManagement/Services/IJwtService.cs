namespace EmployeeManagement.Services
{
    public interface IJwtService
    {
        string generateToken(int userId, string email, string role);
    }

}
