using EncryptionDemo.Repository;
using EncryptionDemo.Service;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        //Constructor

        public UserController(UserService userService) { 
           this.userService = userService;
        }


        //Add User
        // Add User
        [HttpPost]
        public async Task<IActionResult> AddUser([FromQuery] string name, [FromQuery] string sensitiveInformation)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(sensitiveInformation))
            {
                return BadRequest(new { Message = "Invalid input. Name and sensitive information are required." });
            }

            try
            {
                // Call the service to add the user
                await userService.addUser(name, sensitiveInformation);

                // Return success response
                return Ok(new { Message = "User Created Successfully!!!" });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDecryptedData(int id)
        {
            var decryptedData = await userService.GetDecryptedDataAsync(id);
            return Ok(new { DecryptedData = decryptedData });
        }
    }
}

