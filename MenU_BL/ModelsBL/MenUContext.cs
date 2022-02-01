using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public void AddRestaurant(Restaurant r)
        {
            this.Restaurants.Add(r);
            this.SaveChanges();
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
        public void AddDish(Dish d)
        {
            this.Dishes.Add(d);
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
        public Account Login(string username, string hashedPass) => this.Accounts.FirstOrDefault(x => x.Username == username && x.Pass == hashedPass);
        public Account Login(string token)
        {
            AccountAuthToken a = this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token);
            if (a != null)
                return a.Account;
            return null;
        }
        public Restaurant FindRestaurantById(int id) => this.Restaurants.FirstOrDefault(x => x.RestaurantId == id);
        public List<Restaurant> FindRestaurantWithLetters(string letters) => this.Restaurants.Where(x => x.RestaurantName.Contains(letters)).ToList();
        public List<Review> GetUserReviews(int id) => this.Reviews.Where(x => x.Reviewer == id).ToList();
        public List<Review> GetRestaurantReviews(int id) => this.Reviews.Where(x => x.DishNavigation.Restaurant == id).ToList();
        public List<Review> GetDishReviews(int id) => this.Reviews.Where(x => x.Dish == id).ToList();
        public List<Restaurant> GetPendingRestraurants() => this.Restaurants.Where(x => x.RestaurantStatus == this.ObjectStatuses.FirstOrDefault(y => y.StatusName == "Pending").StatusId).ToList();
        public List<Restaurant> GetOwnerRestaurants(int ownerId) => this.Restaurants.Where(x => x.OwnerId == ownerId).ToList();
        public List<Tag> GetAllTags() => this.Tags.ToList();
        public List<Allergen> GetAllAllergens() => this.Allergens.ToList();
        public Dish FindDishByRestaurant(int id) => this.Dishes.FirstOrDefault(x => x.Restaurant == id);
        public Dish FindDishByID(int id) => this.Dishes.FirstOrDefault(x => x.DishId == id);
        public bool UpdateDish(Dish d)
        {
            Dish update = FindDishByID(d.DishId);
            if (update != null)
            {
                update.DishStatus = d.DishStatus;
                update.DishPicture = d.DishPicture; 
                update.DishName = d.DishName; 
                update.DishDescription = d.DishDescription;
                update.AllergenInDishes = d.AllergenInDishes;
                update.Restaurant = d.Restaurant;
                return true;
            }
            return false;
        }
        public Restaurant FindRestaurantByOwner(int id) => Restaurants.FirstOrDefault(x => x.OwnerId == id);
        public Restaurant FindRestaurantByID(int id) => Restaurants.FirstOrDefault(x => x.RestaurantId == id);
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
                foreach (RestaurantTag rT in r.RestaurantTags)
                {
                    this.RestaurantTags.Add(new RestaurantTag() { RestaurantId = rT.RestaurantId, TagId = rT.TagId });
                }
                return true;
            }
            return false;
        }

        public bool AddDishes(List<Dish> dishes)
        {
            if(dishes != null)
            {
                foreach (Dish dish in dishes)
                {
                    foreach (DishTag dT in dish.DishTags)
                    {
                        this.DishTags.Add(new DishTag() { DishId = dT.DishId, TagId = dT.TagId, });
                    }
                    UpdateDish(dish);
                }
                return true;
            }
            return false;
        }

    }
}