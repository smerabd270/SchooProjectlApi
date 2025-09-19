namespace SchooProjectlApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Student"; // Admin, Teacher, Student

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();   // if student
        public ICollection<Course> Courses { get; set; } = new List<Course>(); // if teacher
    }
}
