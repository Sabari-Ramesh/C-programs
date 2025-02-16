using BankApplicationJWT.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationJWT.DTO
{
    public class BankDTO
    {

        public String AccountNumber { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }
    }
}
