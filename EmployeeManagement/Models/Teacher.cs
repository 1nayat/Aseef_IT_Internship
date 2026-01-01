namespace EmployeeManagement.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }

        public ICollection<TeacherClassSubject> TeacherClassSubjects { get; set; }
    }

}
