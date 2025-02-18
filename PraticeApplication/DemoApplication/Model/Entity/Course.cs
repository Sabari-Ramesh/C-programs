using System.Text.Json.Serialization;

namespace DemoApplication.Model.Entity
{
    public class Course
    {
        public int courseId { get; set; }
        public string courseName { get; set; }

        public int teacherId { get; set; }
        public virtual Teacher teacher { get; set; }
         public virtual ICollection<Student> std { get; set; }

    }
}
