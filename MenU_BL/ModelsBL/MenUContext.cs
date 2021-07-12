using System;
using System.Collections.Generic;
using System.Text;
using MenU_BL.ModelsBL;
using System.Linq;

namespace MenU_BL.Models
{
    partial class MenUContext
    {
        public string GetSalt(string username) => this.Accounts.FirstOrDefault(x => x.Username == username).Salt;
        public int GetIterations(string username) => this.Accounts.FirstOrDefault(x => x.Username == username).Iterations;
        public Account Login(string username, string hashedPass) => this.Accounts.FirstOrDefault(x => x.Username == username && x.Pass == hashedPass);
        public Account Login(string token) => this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token).Account;
        public bool Exists(string username, string email) => this.Accounts.Any(x => x.Email == email && x.Username == username);
        public bool SaltExists(string salt) => this.Accounts.Any(x => x.Salt == salt);
        public bool TokenExists(string token) => this.AccountAuthTokens.Any(x => x.AuthToken == token);
        public void RemoveToken(string token)
        {
            AccountAuthToken toRemove = this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token);
            this.AccountAuthTokens.Remove(toRemove);
            this.SaveChanges();
        }
        public void SaveToken(int id, string token)
        {
            this.AccountAuthTokens.Add(new AccountAuthToken() { AccountId = id, AuthToken = token });
            this.SaveChanges();
        }
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
    }
}
