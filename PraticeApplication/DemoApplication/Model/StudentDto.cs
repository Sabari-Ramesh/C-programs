using DemoApplication.Controllers;
using DemoApplication.Model.Entity;

namespace DemoApplication.Model
{
    public class StudentDto
    {
        public int studentId { get; set; }
        public string studentName { get; set; }

        public IEnumerable<CoursesDto> courses { get; set; }
    }
}
