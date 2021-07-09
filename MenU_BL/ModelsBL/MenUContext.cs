using System;
using System.Collections.Generic;
using System.Text;
using MenU_BL.ModelsBL;
using System.Linq;

namespace MenU_BL.Models
{
    partial class MenUContext
    {
        public string GetSalt(int id) => this.Accounts.FirstOrDefault(x => x.AccountId == id).Salt;
        public Account Login(string username, string hashedPass) => this.Accounts.FirstOrDefault(x => x.Username == username && x.Pass == hashedPass);
        public Account Login(string token) => this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token).Account;
        public bool Exists(string username, string email) => this.Accounts.Any(x => x.Email == email && x.Username == username);
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
    }
}
