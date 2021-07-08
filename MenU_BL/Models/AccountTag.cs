using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("AccountTag")]
    public partial class AccountTag
    {
        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Key]
        [Column("TagID")]
        public int TagId { get; set; }
        public int PickedNum { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("AccountTags")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("AccountTags")]
        public virtual Tag Tag { get; set; }
    }
}
