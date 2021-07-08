using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Allergen")]
    public partial class Allergen
    {
        public Allergen()
        {
            AllergenInDishes = new HashSet<AllergenInDish>();
        }

        [Key]
        [Column("AllergenID")]
        public int AllergenId { get; set; }
        [Required]
        [Column("Allergen")]
        [StringLength(255)]
        public string Allergen1 { get; set; }

        [InverseProperty(nameof(AllergenInDish.Allergen))]
        public virtual ICollection<AllergenInDish> AllergenInDishes { get; set; }
    }
}
