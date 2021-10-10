using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenU_BL.Models
{
    [Serializable]
    public class UniqueKeyInUseException : Exception
    {
       
        public string Username { get; }

        public UniqueKeyInUseException() { }

        public UniqueKeyInUseException(string message)
            : base(message) { }

        public UniqueKeyInUseException(string message, Exception inner)
            : base(message, inner) { }

        public UniqueKeyInUseException(string message, string username)
            : this(message)
        {
            Username = username;
        }
        
    }
}
