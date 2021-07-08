using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Review")]
    public partial class Review
    {
        [Key]
        [Column("ReviewID")]
        public int ReviewId { get; set; }
        [Column(TypeName = "date")]
        public DateTime PostDate { get; set; }
        public int Dish { get; set; }
        [Required]
        [StringLength(50)]
        public string ReviewTitle { get; set; }
        [Required]
        [StringLength(255)]
        public string ReviewBody { get; set; }
        public int Rating { get; set; }
        public int Reviewer { get; set; }
        public int ReviewStatus { get; set; }
        [Required]
        [StringLength(255)]
        public string ReviewPicture { get; set; }
        public bool IsLiked { get; set; }

        [ForeignKey(nameof(Dish))]
        [InverseProperty("Reviews")]
        public virtual Dish DishNavigation { get; set; }
        [ForeignKey(nameof(ReviewStatus))]
        [InverseProperty(nameof(ObjectStatus.Reviews))]
        public virtual ObjectStatus ReviewStatusNavigation { get; set; }
        [ForeignKey(nameof(Reviewer))]
        [InverseProperty(nameof(Account.Reviews))]
        public virtual Account ReviewerNavigation { get; set; }
    }
}
