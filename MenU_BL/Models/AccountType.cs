using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("AccountType")]
    public partial class AccountType
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [Column("TypeID")]
        public int TypeId { get; set; }
        [Required]
        [StringLength(255)]
        public string TypeName { get; set; }

        [InverseProperty(nameof(Account.AccountTypeNavigation))]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
