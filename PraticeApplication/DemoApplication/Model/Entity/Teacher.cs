namespace DemoApplication.Model.Entity
{
    public class Teacher
    {
        public int teacherId { get; set; }
        public String teacherName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
