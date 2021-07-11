using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Dish")]
    [Index(nameof(DishName), Name = "I_Dish_DishName")]
    public partial class Dish
    {
        public Dish()
        {
            AllergenInDishes = new HashSet<AllergenInDish>();
            DishTags = new HashSet<DishTag>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        [Column("DishID")]
        public int DishId { get; set; }
        [Required]
        [StringLength(255)]
        public string DishName { get; set; }
        [Required]
        [StringLength(255)]
        public string DishDescription { get; set; }
        public int Restaurant { get; set; }
        public int DishStatus { get; set; }
        [Required]
        [StringLength(255)]
        public string DishPicture { get; set; }

        [ForeignKey(nameof(DishStatus))]
        [InverseProperty(nameof(ObjectStatus.Dishes))]
        public virtual ObjectStatus DishStatusNavigation { get; set; }
        [ForeignKey(nameof(Restaurant))]
        [InverseProperty("Dishes")]
        public virtual Restaurant RestaurantNavigation { get; set; }
        [InverseProperty(nameof(AllergenInDish.Dish))]
        public virtual ICollection<AllergenInDish> AllergenInDishes { get; set; }
        [InverseProperty(nameof(DishTag.Dish))]
        public virtual ICollection<DishTag> DishTags { get; set; }
        [InverseProperty(nameof(Review.DishNavigation))]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
