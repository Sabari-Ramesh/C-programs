using EncryptionDemo.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService) { 
            _accountService = accountService;
        }

        [HttpPost("/addaccount")]
        public async Task<IActionResult> AddAccount([FromQuery] string name, [FromQuery] string accountNumber) {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest(new { Message = "Invalid input. Name and sensitive information are required." });
            }
            try
            {
                // Call the service to add the user
                await _accountService.AddAccount(name, accountNumber);

                // Return success response
                return Ok(new { Message = "Account Created Successfully!!!" });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
            }
        }
    }
}
