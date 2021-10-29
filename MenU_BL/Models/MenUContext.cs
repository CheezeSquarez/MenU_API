using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MenU_BL.Models
{
    public partial class MenUContext : DbContext
    {
        public MenUContext()
        {
        }

        public MenUContext(DbContextOptions<MenUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountAuthToken> AccountAuthTokens { get; set; }
        public virtual DbSet<AccountTag> AccountTags { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Allergen> Allergens { get; set; }
        public virtual DbSet<AllergenInDish> AllergenInDishes { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishTag> DishTags { get; set; }
        public virtual DbSet<ObjectStatus> ObjectStatuses { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<RestaurantTag> RestaurantTags { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database=MenU; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.AccountType).HasDefaultValueSql("((1))");

                entity.Property(e => e.ProfilePicture).HasDefaultValueSql("('/imgs/default_user_pfp.png')");

                entity.HasOne(d => d.AccountStatusNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_AccountStatus");

                entity.HasOne(d => d.AccountTypeNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_AccountType");
            });

            modelBuilder.Entity<AccountAuthToken>(entity =>
            {
                entity.HasKey(e => e.AuthToken)
                    .HasName("PK_AccountAuthToken_AuthToken");

                entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountAuthTokens)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountAuthToken_AccountID");
            });

            modelBuilder.Entity<AccountTag>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.TagId })
                    .HasName("PK_AccountTag_AccountID_TagID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountTags)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTag_AccountID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.AccountTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTag_TagID");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_AccountType_TypeID");
            });

            modelBuilder.Entity<AllergenInDish>(entity =>
            {
                entity.HasKey(e => new { e.AllergenId, e.DishId })
                    .HasName("PK_AllergenInDish_AllergenID_DishID");

                entity.HasOne(d => d.Allergen)
                    .WithMany(p => p.AllergenInDishes)
                    .HasForeignKey(d => d.AllergenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AllergenInDish_AllergenID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.AllergenInDishes)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AllergenInDish_DishID");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.DishPicture).HasDefaultValueSql("('/imgs/default_dish.png')");

                entity.Property(e => e.DishStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DishStatusNavigation)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.DishStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_DishStatus");

                entity.HasOne(d => d.RestaurantNavigation)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.Restaurant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_Restaurant");
            });

            modelBuilder.Entity<DishTag>(entity =>
            {
                entity.HasKey(e => new { e.DishId, e.TagId })
                    .HasName("PK_DishTag_DishID_TagID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishTags)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DishTag_DishID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.DishTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_TagID");
            });

            modelBuilder.Entity<ObjectStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_Status_StatusID");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.RestaurantPicture).HasDefaultValueSql("('/imgs/default_restaurant.png')");

                entity.Property(e => e.RestaurantStatus).HasDefaultValueSql("((4))");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurant_Owner");

                entity.HasOne(d => d.RestaurantStatusNavigation)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.RestaurantStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurant_RestaurantStatus");
            });

            modelBuilder.Entity<RestaurantTag>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.RestaurantId })
                    .HasName("PK_RestaurantTag_TagID_RestaurantID");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantTags)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RestaurantTag_RestaurantID");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.RestaurantTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RestaurantTag_TagID");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.PostDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReviewPicture).HasDefaultValueSql("('/imgs/default_review_pfp.png')");

                entity.Property(e => e.ReviewStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DishNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Dish)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Dish");

                entity.HasOne(d => d.ReviewStatusNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ReviewStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_ReviewStatus");

                entity.HasOne(d => d.ReviewerNavigation)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Reviewer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Reviewer");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
