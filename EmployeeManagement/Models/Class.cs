namespace EmployeeManagement.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public bool IsDeleted { get; set; } = false;


        public ICollection<TeacherClassSubject> TeacherClassSubjects { get; set; }
    }

}
