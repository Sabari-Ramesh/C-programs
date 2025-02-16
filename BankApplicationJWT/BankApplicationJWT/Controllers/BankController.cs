using BankApplicationJWT.Bo;
using BankApplicationJWT.Data;
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

        public BankController(AppDbContext context,BankBo bankBo)
        {
            _context = context;
            _bankBo = bankBo;
        }

        [HttpPost("create-account")]
        public IActionResult CreateAccount([FromBody] Bank bank)
        {
            try
            {
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

            //var user = _context.Users.Find(bank.UserId);
            //if (user == null)
            //    return NotFound("User not found.");

            //// Check if the user already has a bank account
            //if (_context.Banks.Any(b => b.UserId == bank.UserId))
            //    return BadRequest("User already has a bank account.");

            //// Add the new bank account to the database
            //_context.Banks.Add(bank);
            //_context.SaveChanges();

            //return Ok(new { Message = "Bank account created successfully." });
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
                return Ok(resBank);
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

                //// Find the user's bank account by AccountNumber
                //var account = _context.Banks.FirstOrDefault(b => b.AccountNumber == accountNumber);
                //if (account == null)
                //    return NotFound("Account not found.");

                //// Ensure the account belongs to the authenticated user
                //if (account.UserId != userIdFromToken)
                //    return Forbid("You are not authorized to deposit into this account.");

                //// Update the account balance
                //account.Amount += amount;
                //_context.SaveChanges();

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

                //// Find the user's bank account by AccountNumber
                //var account = _context.Banks.FirstOrDefault(b => b.AccountNumber == accountNumber);
                //if (account == null)
                //    return NotFound("Account not found.");

                //// Ensure the account belongs to the authenticated user
                //if (account.UserId != userIdFromToken)
                //    return Forbid("You are not authorized to withdraw from this account.");

                //// Check if the user has sufficient balance
                //if (account.Amount < amount)
                //    return BadRequest("Insufficient balance.");

                //// Update the account balance
                //account.Amount -= amount;
                //_context.SaveChanges();

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

                //// Find the sender's bank account by AccountNumber
                //var senderAccount = _context.Banks.FirstOrDefault(b => b.AccountNumber == senderAccountNumber);
                //if (senderAccount == null)
                //    return NotFound("Sender account not found.");

                //// Ensure the sender's account belongs to the authenticated user
                //if (senderAccount.UserId != senderIdFromToken)
                //    return Forbid("You are not authorized to transfer funds from this account.");

                //// Find the recipient's bank account by AccountNumber
                //var recipientAccount = _context.Banks.FirstOrDefault(b => b.AccountNumber == recipientAccountNumber);
                //if (recipientAccount == null)
                //    return NotFound("Recipient account not found.");

                //// Check if the sender has sufficient balance
                //if (senderAccount.Amount < amount)
                //    return BadRequest("Insufficient balance.");

                //// Perform the transfer
                //senderAccount.Amount -= amount;
                //recipientAccount.Amount += amount;
                //_context.SaveChanges();

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


