using RoleBasedJWT.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedJWT.Model.Dto
{
    public class AccountDto
    {
        public int AccountNo { get; set; }
        public int UserId { get; set; } 
        public decimal Balance { get; set; }
        public string BankName { get; set; }
    }
}
