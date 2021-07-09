using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;

namespace MenU_API.DataTransferObjects
{
    public class AccountDTO
    {
        public AccountDTO(Account acc)
        {
            this.AccountId = acc.AccountId;
            this.AccountStatus = acc.AccountStatus;
            this.AccountType = acc.AccountType;
            this.DateOfBirth = acc.DateOfBirth;
            this.Email = acc.Email;
            this.FirstName = acc.FirstName;
            this.Iterations = acc.Iterations;
            this.LastName = acc.LastName;
            this.Pass = acc.Pass;
            this.ProfilePicture = acc.ProfilePicture;
            this.Salt = acc.Salt;
            this.Username = acc.Username;
            
        }
        public AccountDTO() { }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AccountType { get; set; }
        public string ProfilePicture { get; set; }
        public string Pass { get; set; }
        public int AccountStatus { get; set; }
        public string Salt { get; set; }
        public int Iterations { get; set; }
    }
}
