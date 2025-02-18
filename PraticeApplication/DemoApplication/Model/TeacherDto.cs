namespace DemoApplication.Model
{
    public class TeacherDto
    {
        public int teacherId { get; set; }
        public String teacherName { get; set; }
        public IEnumerable<CoursesDto> courses { get; set; }
    }
}
