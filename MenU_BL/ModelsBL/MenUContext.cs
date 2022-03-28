using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MenU_BL.Models
{
    partial class MenUContext
    {
        public void UpdateRestaurantStatus(int resId, int statusId)
        {
            this.Restaurants.FirstOrDefault(x => x.RestaurantId == resId).RestaurantStatus = statusId;
            this.SaveChanges();
        }
        public void SaveToken(int id, string token)
        {
            this.AccountAuthTokens.Add(new AccountAuthToken() { AccountId = id, AuthToken = token });
            this.SaveChanges();
        }
        public Restaurant AddRestaurant(Restaurant r)
        {
            this.Restaurants.Add(r);
            this.SaveChanges();

            Restaurant added = this.Restaurants.FirstOrDefault(x => x.RestaurantName == r.RestaurantName && x.City == r.City && x.StreetName == r.StreetName && x.OwnerId == r.OwnerId);
            return added;
        }
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
        public void AddReview(Review r)
        {
            this.Reviews.Add(r);
            this.SaveChanges();
        }
        public string GetSalt(string username)
        {
            Account a = this.Accounts.FirstOrDefault(x => x.Username == username);
            if (a != null)
                return a.Salt;
            return null;
        }
        public int GetIterations(string username)
        {
            Account a = this.Accounts.FirstOrDefault(x => x.Username == username);
            if (a != null)
                return a.Iterations;
            return -1;
        }
        public bool Exists(string username, string email) => this.Accounts.Any(x => x.Email == email || x.Username == username);
        public bool Exists(string username) => this.Accounts.Any(x => x.Username == username);
        public bool SaltExists(string salt) => this.Accounts.Any(x => x.Salt == salt);
        public bool TokenExists(string token) => this.AccountAuthTokens.Any(x => x.AuthToken == token);
        public Account Login(string username, string hashedPass) 
        {
            return this.Accounts
                 .Include(x => x.Restaurants)
                 .ThenInclude(r => r.Dishes).ThenInclude(a => a.DishTags)
                 .Include(b => b.Reviews)
                 .Where(x => x.Username == username && x.Pass == hashedPass).FirstOrDefault();
        }
        public Account Login(string token)
        {
            AccountAuthToken a = this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token);
            if (a != null)
            {
                Account acc = this.Accounts
                    .Include(x => x.Restaurants)
                    .ThenInclude(r => r.Dishes).ThenInclude(a => a.DishTags)
                    .Include(b => b.Reviews)
                    .Where(y => y.AccountId == a.AccountId).FirstOrDefault();
                return acc;
            }
            return null;
        }
        public Restaurant FindRestaurantById(int id)
        {
            return this.Restaurants
                .Include(x => x.RestaurantTags)
                .Include(y => y.Dishes).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes).ThenInclude(b => b.DishTags).Where(c => c.RestaurantId == id).FirstOrDefault();
        }
        public List<Restaurant> FindRestaurantWithLetters(string letters) => this.Restaurants.Where(x => x.RestaurantName.Contains(letters) && x.RestaurantId != 0).ToList();
        public List<Review> GetUserReviews(int id) => this.Reviews.Where(x => x.Reviewer == id).ToList();
        public List<Review> GetRestaurantReviews(int id) => this.Reviews.Where(x => x.DishNavigation.Restaurant == id).ToList();
        public List<Review> GetDishReviews(int id) => this.Reviews.Where(x => x.Dish == id).ToList();
        public List<Restaurant> GetPendingRestraurants() => this.Restaurants.Where(x => x.RestaurantStatus == this.ObjectStatuses.FirstOrDefault(y => y.StatusName == "Pending").StatusId).ToList();
        public List<Restaurant> GetOwnerRestaurants(int ownerId) => this.Restaurants.Where(x => x.OwnerId == ownerId).ToList();
        public List<Tag> GetAllTags() => this.Tags.ToList();
        public List<Allergen> GetAllAllergens() => this.Allergens.ToList();
        public Dish FindDishByRestaurant(int id) => this.Dishes.FirstOrDefault(x => x.Restaurant == id);
        public Dish FindDishByID(int id) =>
            this.Dishes.Include(a => a.Reviews).ThenInclude(b => b.ReviewerNavigation)
            .Include(c => c.RestaurantNavigation)
            .Include(d => d.AllergenInDishes)
            .Include(e => e.DishTags)
            .Where(x => x.DishId == id).FirstOrDefault();
        public bool UpdateDish(Dish d)
        {
            Dish update = FindDishByID(d.DishId);
            if (update != null)
            {
                update.DishStatus = d.DishStatus;
                update.DishPicture = d.DishPicture; 
                update.DishName = d.DishName; 
                update.DishDescription = d.DishDescription;
                update.Restaurant = d.Restaurant;
                return true;
            }
            return false;
        }
        public Restaurant FindRestaurantByOwner(int id) => Restaurants
            .Include(x => x.RestaurantTags)
                .Include(y => y.Dishes).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes).ThenInclude(b => b.DishTags).Where(x => x.OwnerId == id).FirstOrDefault();
        public Restaurant FindRestaurantByID(int id) => Restaurants
            .Include(x => x.RestaurantTags)
                .Include(y => y.Dishes).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes).ThenInclude(b => b.DishTags).Where(x => x.RestaurantId == id).FirstOrDefault();
        public bool UpdateRestaurant(Restaurant r)
        {
            Restaurant update = FindRestaurantByID(r.RestaurantId);
            if (update != null)
            {
                update.OwnerId = r.OwnerId;
                update.RestaurantName = r.RestaurantName;
                update.RestaurantPicture = r.RestaurantPicture;
                update.RestaurantStatus = r.RestaurantStatus;
                update.RestaurantTags = r.RestaurantTags;
                update.StreetName = r.StreetName;
                update.StreetNumber = r.StreetNumber;
                
                return true;
            }
            return false;
        }

        public Dish AddDish(Dish d)
        {
            this.Dishes.Add(d);
            this.SaveChanges();
            
            Dish dish = this.Dishes.FirstOrDefault(x => x.DishName == d.DishName && x.DishDescription == d.DishDescription && x.Restaurant == x.Restaurant);
            return dish;
        }
        public void AddAllRestaurantTags(List<RestaurantTag> restaurantTags)
        {
            foreach(RestaurantTag tag in restaurantTags)
                this.RestaurantTags.Add(tag);
            this.SaveChanges();
        }

        public void AddAllDishTags(List<DishTag> dishTags)
        {
            foreach (DishTag tag in dishTags)
                this.DishTags.Add(tag);
            this.SaveChanges();
        }

        public void AddAllAllergensToDish(List<AllergenInDish> allergensInDish)
        {
            foreach(AllergenInDish allergenInDish in allergensInDish)
                this.AllergenInDishes.Add(allergenInDish);
            this.SaveChanges();
        }

        public Restaurant EditRestaurant(Restaurant r, List<RestaurantTag> tags)
        {
            Restaurant restaurant = this.Restaurants.FirstOrDefault(x => x.RestaurantId == r.RestaurantId);
            restaurant.RestaurantName = r.RestaurantName;
            restaurant.StreetName = r.StreetName;
            restaurant.City = r.City;
            restaurant.StreetNumber = r.StreetNumber;
            List<RestaurantTag> rTs = this.RestaurantTags.Where(x => x.RestaurantId == r.RestaurantId).ToList();
            foreach (RestaurantTag t in rTs)
            {
                this.RestaurantTags.Remove(t);
            }
            foreach (RestaurantTag rt in tags)
            {
                this.RestaurantTags.Add(new RestaurantTag() { RestaurantId = rt.RestaurantId, TagId = rt.TagId });
            }
            this.SaveChanges();
            return this.Restaurants.FirstOrDefault(x => x.RestaurantId == r.RestaurantId);
        }

    }
}