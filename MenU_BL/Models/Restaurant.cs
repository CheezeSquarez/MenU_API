using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Restaurant")]
    [Index(nameof(City), Name = "I_Restaurant_City")]
    [Index(nameof(RestaurantName), Name = "I_Restaurant_RestaurantName")]
    [Index(nameof(StreetName), Name = "I_Restaurant_StreetName")]
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
            RestaurantTags = new HashSet<RestaurantTag>();
        }

        [Key]
        [Column("RestaurantID")]
        public int RestaurantId { get; set; }
        [Required]
        [StringLength(255)]
        public string RestaurantName { get; set; }
        [Required]
        [StringLength(255)]
        public string StreetName { get; set; }
        [Column("OwnerID")]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(255)]
        public string City { get; set; }
        [Required]
        [StringLength(255)]
        public string RestaurantPicture { get; set; }
        [Required]
        [StringLength(255)]
        public string StreetNumber { get; set; }
        public int RestaurantStatus { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Account.Restaurants))]
        public virtual Account Owner { get; set; }
        [ForeignKey(nameof(RestaurantStatus))]
        [InverseProperty(nameof(ObjectStatus.Restaurants))]
        public virtual ObjectStatus RestaurantStatusNavigation { get; set; }
        [InverseProperty(nameof(Dish.RestaurantNavigation))]
        public virtual ICollection<Dish> Dishes { get; set; }
        [InverseProperty(nameof(RestaurantTag.Restaurant))]
        public virtual ICollection<RestaurantTag> RestaurantTags { get; set; }
    }
}
