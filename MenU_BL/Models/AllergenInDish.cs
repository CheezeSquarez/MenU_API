using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("AllergenInDish")]
    public partial class AllergenInDish
    {
        [Key]
        [Column("AllergenID")]
        public int AllergenId { get; set; }
        [Key]
        [Column("DishID")]
        public int DishId { get; set; }

        [ForeignKey(nameof(AllergenId))]
        [InverseProperty("AllergenInDishes")]
        public virtual Allergen Allergen { get; set; }
        [ForeignKey(nameof(DishId))]
        [InverseProperty("AllergenInDishes")]
        public virtual Dish Dish { get; set; }
    }
}
