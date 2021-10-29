using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("AccountAuthToken")]
    public partial class AccountAuthToken
    {
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Key]
        [StringLength(255)]
        public string AuthToken { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountAuthTokens")]
        public virtual Account Account { get; set; }
    }
}
