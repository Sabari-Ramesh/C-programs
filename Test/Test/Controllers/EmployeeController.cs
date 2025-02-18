using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Data;
using Test.Modal;
using Test.Modal.Entity;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //List all Employee in the database

        [HttpGet]
        public IActionResult getAllEmployee()
        {
            var allEmployees = dbContext.employees.ToList();
            return Ok(allEmployees);
        }

        //Add Employee

        [HttpPost]
        public IActionResult addEmployee(EmployeeDTO addemployeeDTO)
        {
            var EmployeeEntity = new Employee()
            {
                Name = addemployeeDTO.Name,
                email = addemployeeDTO.email,
                phoneNumber = addemployeeDTO.phoneNumber,
                salary = addemployeeDTO.salary,
            };
            dbContext.employees.Add(EmployeeEntity);
            dbContext.SaveChanges();
            return Ok();
        }

        //Find Employee By Id

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult getEmployeeById(Guid id) {
            var employee = dbContext.employees.Find(id);
            if (employee == null) {
                return NotFound(id);
            }
            return Ok(employee);
            }

        //Update the Employee data in the table

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult updateEmployee(Guid id, UpdateEmployeeDTO updateEmployeeDTO) { 
        var employee =dbContext.employees.Find(id);
            if (employee == null) {
                return NotFound(id);
            }
            employee.Name = updateEmployeeDTO.Name;
            employee.salary =updateEmployeeDTO.salary;
            employee.phoneNumber = updateEmployeeDTO.phoneNumber;   
            employee.email = updateEmployeeDTO.email;
            dbContext.SaveChanges();
            return Ok(employee);
        }


        //Delete the Record form the table
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult deleteEmployee(Guid id) {
            var employee = dbContext.employees.Find(id);
            if (employee == null) {
                return NotFound();
            }
            dbContext.employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
