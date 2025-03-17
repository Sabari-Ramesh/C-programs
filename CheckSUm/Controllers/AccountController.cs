using CheckSUm.Data;
using CheckSUm.Entity;
using CheckSUm.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckSUm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService service;
        public AccountController(AccountService service)
        {
            this.service = service;
        }

        [HttpPost("/addaccount")]
        public async Task<IActionResult> AddAccount([FromQuery] string name, [FromQuery] string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest(new { Message = "Invalid input. Name and sensitive information are required." });
            }
            try
            {
                AccountDetails details = new AccountDetails()
                {
                    name = name,
                    accountNumber = accountNumber
                };

                AccountDetails resAccount = await service.AccountDetails(details);

                return Ok(new { resAccount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
            }
        }


        //Find Account by Id

        [HttpGet("retriveaccountbyid/{id}")]
        public async Task<IActionResult> FindAccountById(int id) {
            if (id == null || id <= 0) {
                return BadRequest(new { Message = "Invalid Input id , And the input must be not null !!!" });
            }
            AccountDetails details = await service.FindAccountById(id);
            return Ok(new { details });
        }


        //Add Account ModTen Approach
        [HttpPost("/addmodtenaccount")]
        public async Task<IActionResult> ModTenAccountNumber([FromQuery] string name, [FromQuery] string accountNumber) {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest(new { Message = "Invalid input. Name and sensitive information are required." });
            }
            try
            {
                // Call the service to add the user
                AccountDetails details = new AccountDetails()
                {
                    name = name,
                    accountNumber = accountNumber
                };

                AccountDetails resAccount = await service.ModTenAddAccountNumber(details);

                // Return success response
                return Ok(new { resAccount });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
            }
        }


        //Find Account By Id In ModTen Method
        [HttpGet("modTenFindAccount/{id}")]
        public async Task<IActionResult> ModTenFindAccountById(int id) {
            if (id == null || id <= 0)
            {
                return BadRequest(new { Message = "Invalid Input id , And the input must be not null !!!" });
            }
            AccountDetails details = await service.ModTenFindAccountById(id);
            return Ok(new { details });
        }

        //RandomAccountNumberCeation
        [HttpPost("/accountNnumber")]
        public async Task<IActionResult> RandomAccountCreation() {
            string res = await service.RandomAccountNumber();
            return Ok(new { res });
        }

        //Account Creation As per requirement
        [HttpPost("/accountcreationasperrequirements")]
        public async Task<IActionResult> AccountCreationRequirements([FromQuery] string sectorId, [FromQuery] string currNo) {
            if (sectorId.Length != 2) {
                return BadRequest("Sector length should be Two");
            }
            if (currNo.Length != 3)
            {
                return BadRequest("Currency Length should be Two");
            }
            AccountDetails responseAccountNum = await service.AccountNumberRequirements(sectorId, currNo);
            return Ok(responseAccountNum);
        }


        //Verify Account Number
        [HttpGet("verifyaccountnumber")]
        public async Task<IActionResult> VerifyAccountNumber([FromQuery] string accountNumber) {
            Console.WriteLine($"{ accountNumber} {accountNumber.Length}");
            if ( accountNumber.Length!=13) {
                return BadRequest("Ivalid Account Number");
            }
            string result = await service.ValidateAccountNumber(accountNumber);
            return Ok(result);
        } 
    }
}
