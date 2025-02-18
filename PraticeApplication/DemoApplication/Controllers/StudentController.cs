using DemoApplication.Data;
using DemoApplication.Model;
using DemoApplication.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DemoApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]

    //Find All Students from the Database
    public class StudentController:ControllerBase
    {
        private readonly ApplicationContext context;

        public StudentController(ApplicationContext context)
        {
           this.context = context;
        }

        //Find All students

        [HttpGet]
        public IActionResult GetAllStudents()
        
        {
            List<Student> students = context.Students
                .Include(c => c.Courses)
                .ToList();


            List<StudentDto> studentsList = new List<StudentDto>();

            if (students.Count == 0)
            {
                return NotFound("No students found.");
            }

            foreach (var student in students)
            {
                StudentDto studentDto = new StudentDto
                {
                    studentId = student.Id,
                    studentName = student.studentName,
                    courses = student.Courses.Select(c => new CoursesDto
                    {
                        CourseID = c.courseId,
                        CourseName = c.courseName
                    }).ToList()
                };

                studentsList.Add(studentDto);
            }
            return Ok(studentsList);
        }

        //add Students 
        [HttpPost]
        public IActionResult addStudent(StudentDto studentdto)
        {
            Student student = new Student();
            student.studentName = studentdto.studentName;


            //Linq Querry
            var selectedCourse = context.Courses.Where(c => studentdto.courses.Select(sc => sc.CourseName).Contains(c.courseName)).ToList();

            if (studentdto.courses.Count() == 0)
            {
                return BadRequest("No course Details Found");
            }
            //Add the student to the database
            student.Courses = selectedCourse;
            context.Students.Add(student);
            context.SaveChanges();
            return Ok(new
            {
                id = student.Id,
                studentName = student.studentName,
                courses = student.Courses.Select(c => new { c.courseId, c.courseName }).ToList()
            });
        }


        //Update Student 
        [HttpPut("update")]
        public IActionResult updateStudent([FromQuery] int id , [FromQuery] String name) {

            var student = context.Students.Include(s => s.Courses).FirstOrDefault(s => s.Id == id);
            if (student == null) {
                return BadRequest("Student Not Found in the DataBase!!!!");
            }
            student.studentName = name;
            context.Students.Update(student);
            context.SaveChanges();
            return Ok(new
            {
                id = student.Id,
                studentName = student.studentName,
                courses = student.Courses.Select(c => new { c.courseId, c.courseName }).ToList()
            });
        }


        //Delete Student
        [HttpDelete]
        public IActionResult deleteStudent([FromQuery] int id) {

            var student = context.Students.Find(id);
            if (id == null) {
                return BadRequest("Student Not Found in the DataBase");
                    }
            return Ok("Data SuccessFully Deleted.....");
        }

    }
}
