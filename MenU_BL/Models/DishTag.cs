using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("DishTag")]
    public partial class DishTag
    {
        [Key]
        [Column("DishID")]
        public int DishId { get; set; }
        [Key]
        [Column("TagID")]
        public int TagId { get; set; }

        [ForeignKey(nameof(DishId))]
        [InverseProperty("DishTags")]
        public virtual Dish Dish { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("DishTags")]
        public virtual Tag Tag { get; set; }
    }
}
