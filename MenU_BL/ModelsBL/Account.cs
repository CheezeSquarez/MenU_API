using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace MenU_BL.Models
{
    partial class Account
    {
        public Account UpdateUser(List<string> info, MenUContext context)
        {
            
            if (info[0] != this.Username)
            {
                if (!context.Exists(info[0]))
                    this.Username = info[0];
                else
                    throw new UniqueKeyInUseException("Username is already taken", info[0]);
            }
            this.FirstName = info[1];
            this.LastName = info[2];

            context.SaveChanges();
            return this;
        }
        public void ChangePass(int iterations, string salt, string hash)
        {
            this.Iterations = iterations;
            this.Salt = salt;
            this.Pass = hash;
        }
    }
}
