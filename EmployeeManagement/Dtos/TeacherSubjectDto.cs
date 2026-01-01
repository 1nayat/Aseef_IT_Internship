namespace EmployeeManagement.Dtos
{
    public class TeacherSubjectDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public List<string> Subjects { get; set; }
    }

}
