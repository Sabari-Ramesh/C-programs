using System.Text.Json.Serialization;

namespace DemoApplication.Model.Entity
{
    public class Student
    {
        public int Id { get; set; }
        public string studentName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
