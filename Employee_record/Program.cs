
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Department { get; set; }
}

class Program
{
    static Dictionary<int, Employee> employees = new Dictionary<int, Employee>();
    static int nextId = 1;

    static void Main()
    {

        employees.Add(nextId, new Employee { Id = nextId, Name = "Inayat", Department = "IT" });
        nextId++;

        employees.Add(nextId, new Employee { Id = nextId, Name = "Farhan", Department = "HR" });
        nextId++;

        while (true)
        {
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View  Employees");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine()!;
            Console.WriteLine();

            if (choice == "1")
                AddEmployee();
            else if (choice == "2")
                ViewEmployees();
            else if (choice == "3")
                UpdateEmployee();
            else if (choice == "4")
                DeleteEmployee();
            else if (choice == "5")
                break;
            else
                Console.WriteLine("Invalid choice. Try again.");
        }
    }

    static void AddEmployee()
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter Department: ");
        string dept = Console.ReadLine()!;

        Employee emp = new Employee
        {
            Id = nextId,
            Name = name,
            Department = dept
        };

        employees.Add(nextId, emp);
        nextId++;

        Console.WriteLine("Employee added successfully");
    }

    static void ViewEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        Console.WriteLine("Employee List");
        foreach (var item in employees)
        {
            Employee emp = item.Value;
            Console.WriteLine($"Id: {emp.Id}, Name: {emp.Name}, Department: {emp.Department}");
        }
    }

    static void UpdateEmployee()
    {
        Console.Write("Enter Employee Id: ");
        int id = int.Parse(Console.ReadLine()!);

        if (!employees.ContainsKey(id))
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Employee emp = employees[id];

        Console.Write("Enter new name (leave blank to keep same): ");
        string newName = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newName))
            emp.Name = newName;

        Console.Write("Enter new department (leave blank to keep same): ");
        string newDept = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newDept))
            emp.Department = newDept;

        Console.WriteLine("Employee updated successfully!");
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter Employee Id: ");
        int id = int.Parse(Console.ReadLine()!);

        if (employees.Remove(id))
            Console.WriteLine("Employee deleted successfully!");
        else
            Console.WriteLine("Employee not found.");
    }
}


