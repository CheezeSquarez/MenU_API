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
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
    }
}
