using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            AccountTags = new HashSet<AccountTag>();
            DishTags = new HashSet<DishTag>();
            RestaurantTags = new HashSet<RestaurantTag>();
        }

        [Key]
        [Column("TagID")]
        public int TagId { get; set; }
        [Required]
        [StringLength(255)]
        public string TagName { get; set; }

        [InverseProperty(nameof(AccountTag.Tag))]
        public virtual ICollection<AccountTag> AccountTags { get; set; }
        [InverseProperty(nameof(DishTag.Tag))]
        public virtual ICollection<DishTag> DishTags { get; set; }
        [InverseProperty(nameof(RestaurantTag.Tag))]
        public virtual ICollection<RestaurantTag> RestaurantTags { get; set; }
    }
}
