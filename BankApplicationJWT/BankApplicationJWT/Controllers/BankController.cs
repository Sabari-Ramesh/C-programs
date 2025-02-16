using BankApplicationJWT.Bo;
using BankApplicationJWT.Data;
using BankApplicationJWT.DTO;
using BankApplicationJWT.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankApplicationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly BankBo _bankBo;
        private readonly BankDTO _bankDTO;

        public BankController(AppDbContext context,BankBo bankBo)
        {
            _context = context;
            _bankBo = bankBo;
        }

        [HttpPost("create-account")]
        public IActionResult CreateAccount([FromBody] BankDTO bankDTO)
        {
            try
            {
                Bank bank = new Bank();
                bank.AccountNumber = bankDTO.AccountNumber;
                bank.Amount = bankDTO.Amount;
                bank.UserId = bankDTO.UserId;
                // Extract UserId from JWT token
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Ensure the userId in the request matches the UserId from the token
                if (bank.UserId != userIdFromToken)
                    return Forbid("You are not authorized to create an account for another user.");

                // Check if the user exists

                string resmesg = _bankBo.addAccount(bank);
                return Ok(resmesg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("account/{userId}")]
        public IActionResult GetAccount(int userId)
        {
            try
            {
                // Extract UserId from JWT token
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Ensure the userId in the request matches the UserId from the token
                if (userId != userIdFromToken)
                    return Forbid("You are not authorized to view another user's account.");

                Bank resBank = _bankBo.findByUserId(userId);
                BankDTO bank = new BankDTO();
                bank.UserId=resBank.UserId;
                bank.Amount = resBank.Amount;
                bank.AccountNumber=resBank.AccountNumber;
                return Ok(bank);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("deposit")]
        public IActionResult Deposit(
          [FromQuery] string accountNumber,
          [FromQuery] decimal amount)
        {
            try
            {
                // Extract UserId from JWT token
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var newBalance = _bankBo.depositAmount(accountNumber, amount);
                return Ok(new { Message = "Amount deposited successfully.", NewBalance = newBalance });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("withdraw")]
        public IActionResult Withdraw(
           [FromQuery] string accountNumber,
           [FromQuery] decimal amount)
        {
            try
            {
                // Extract UserId from JWT token
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var balance = _bankBo.withdrawAmount(accountNumber, amount);
                return Ok(new { Message = "Amount withdrawn successfully.", NewBalance =balance});
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public IActionResult Transfer(
                    [FromQuery] string senderAccountNumber,
                    [FromQuery] string recipientAccountNumber,
                    [FromQuery] decimal amount)
        {
            try
            {
                // Extract UserId from JWT token
                var senderIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var balance = _bankBo.transferAmount(senderAccountNumber, recipientAccountNumber, amount);

                return Ok(new
                {
                    Message = "Funds transferred successfully.",
                    RemainingBalance = balance
                });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}


