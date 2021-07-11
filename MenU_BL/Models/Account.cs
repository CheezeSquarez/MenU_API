using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MenU_BL.Models
{
    [Table("Account")]
    [Index(nameof(DateOfBirth), Name = "I_Account_DateOfBirth")]
    [Index(nameof(FirstName), Name = "I_Account_FirstName")]
    [Index(nameof(LastName), Name = "I_Account_LastName")]
    [Index(nameof(Email), Name = "UK_Account_Email", IsUnique = true)]
    [Index(nameof(Username), Name = "UK_Account_Username", IsUnique = true)]
    public partial class Account
    {
        public Account()
        {
            AccountAuthTokens = new HashSet<AccountAuthToken>();
            AccountTags = new HashSet<AccountTag>();
            Restaurants = new HashSet<Restaurant>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public int AccountType { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfilePicture { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        public int AccountStatus { get; set; }
        [Required]
        [StringLength(255)]
        public string Salt { get; set; }
        public int Iterations { get; set; }

        [ForeignKey(nameof(AccountStatus))]
        [InverseProperty(nameof(ObjectStatus.Accounts))]
        public virtual ObjectStatus AccountStatusNavigation { get; set; }
        [ForeignKey(nameof(AccountType))]
        [InverseProperty("Accounts")]
        public virtual AccountType AccountTypeNavigation { get; set; }
        [InverseProperty(nameof(AccountAuthToken.Account))]
        public virtual ICollection<AccountAuthToken> AccountAuthTokens { get; set; }
        [InverseProperty(nameof(AccountTag.Account))]
        public virtual ICollection<AccountTag> AccountTags { get; set; }
        [InverseProperty(nameof(Restaurant.Owner))]
        public virtual ICollection<Restaurant> Restaurants { get; set; }
        [InverseProperty(nameof(Review.ReviewerNavigation))]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
