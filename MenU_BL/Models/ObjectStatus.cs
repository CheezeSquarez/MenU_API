using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("ObjectStatus")]
    public partial class ObjectStatus
    {
        public ObjectStatus()
        {
            Accounts = new HashSet<Account>();
            Dishes = new HashSet<Dish>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        [Column("StatusID")]
        public int StatusId { get; set; }
        [Required]
        [StringLength(255)]
        public string StatusName { get; set; }

        [InverseProperty(nameof(Account.AccountStatusNavigation))]
        public virtual ICollection<Account> Accounts { get; set; }
        [InverseProperty(nameof(Dish.DishStatusNavigation))]
        public virtual ICollection<Dish> Dishes { get; set; }
        [InverseProperty(nameof(Review.ReviewStatusNavigation))]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
