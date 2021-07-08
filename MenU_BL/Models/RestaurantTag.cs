using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("RestaurantTag")]
    public partial class RestaurantTag
    {
        [Key]
        [Column("TagID")]
        public int TagId { get; set; }
        [Key]
        [Column("RestaurantID")]
        public int RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        [InverseProperty("RestaurantTags")]
        public virtual Restaurant Restaurant { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("RestaurantTags")]
        public virtual Tag Tag { get; set; }
    }
}
