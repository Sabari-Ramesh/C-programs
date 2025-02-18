using DemoApplication.Data;
using DemoApplication.Model;
using DemoApplication.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TeacherController:ControllerBase
    {
        private readonly ApplicationContext context;

        public TeacherController(ApplicationContext context)
        {
            this.context = context;
        }
        //Get All Teacher
        [HttpGet]
        public IActionResult getAllTeacher() { 
            List<Teacher> teacherList = context.Teachers.Include(c=>c.Courses).ToList();
            if (teacherList == null && teacherList.Count > 0) {
                return BadRequest("No Teacher Found");
            }
            List<TeacherDto> teacherDtoList = new List<TeacherDto>();

            foreach(var teacher in teacherList) { 
                TeacherDto teacherDto = new TeacherDto();
                teacherDto.teacherId = teacher.teacherId; 
                teacherDto.teacherName = teacher.teacherName;
                teacherDto.courses = teacher.Courses.Select(c => new CoursesDto
                {
                    CourseID = c.courseId,
                    CourseName = c.courseName
                }).ToList();
                teacherDtoList.Add(teacherDto);
            }
            return Ok(teacherDtoList);
        }

        //Add Teacher 
        [HttpPost]
        public IActionResult addTeacher(TeacherDto teacherDto) {

            if (teacherDto == null) {
                return BadRequest("Error in Add Teacher!!!");
            }
            var selectedCourse = context.Courses.Where(c => teacherDto.courses.Select(sc => sc.CourseName).Contains(c.courseName)).ToList();
            Teacher teacher = new Teacher()
            {
                teacherName = teacherDto.teacherName,
                Courses = selectedCourse
            };
            context.Teachers.Add(teacher);
            context.SaveChanges();
            return Ok(new {
                id = teacher.teacherId,
                Name = teacher.teacherName,
            Courses = teacher.Courses.Select(c => new { c.courseId, c.courseName }).ToList()
            });
        }

    }
}
