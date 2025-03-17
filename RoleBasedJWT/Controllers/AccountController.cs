using Microsoft.AspNetCore.Mvc;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Service;

namespace RoleBasedJWT.Controllers
{
    public class AccountController :ControllerBase
    {
        private readonly AccountService accountService;

        public AccountController(AccountService accountService) { 
            this.accountService = accountService;
        }

        //Add Account in DataBase
        [HttpPost("/addaccount")]
        public async Task<IActionResult> AddAccount([FromBody] AccountDto accountDto)
        {
            if (accountDto == null) {
                return BadRequest("Enter Valid request");
            }
            AccountDto resAccount = await accountService.AddAccount(accountDto);
            return Ok(resAccount);
        }
    }
}
