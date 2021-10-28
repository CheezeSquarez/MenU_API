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
        public string GetSalt(string username) => this.Accounts.FirstOrDefault(x => x.Username == username).Salt;
        public int GetIterations(string username) => this.Accounts.FirstOrDefault(x => x.Username == username).Iterations;
        public bool Exists(string username, string email) => this.Accounts.Any(x => x.Email == email || x.Username == username);
        public bool Exists(string username) => this.Accounts.Any(x => x.Username == username);
        public bool SaltExists(string salt) => this.Accounts.Any(x => x.Salt == salt);
        public bool TokenExists(string token) => this.AccountAuthTokens.Any(x => x.AuthToken == token);
        public Account Login(string username, string hashedPass) => this.Accounts.FirstOrDefault(x => x.Username == username && x.Pass == hashedPass);
        public Account Login(string token) => this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token).Account;
        public Restaurant FindRestaurantById(int id) => this.Restaurants.FirstOrDefault(x => x.RestaurantId == id);
        public List<Restaurant> FindRestaurantWithLetters(string letters) => this.Restaurants.Where(x => x.RestaurantName.Contains(letters)).ToList();
        public List<Review> GetUserReviews(int id) => this.Reviews.Where(x => x.Reviewer == id).ToList();
        public List<Review> GetRestaurantReviews(int id) => this.Reviews.Where(x => x.DishNavigation.Restaurant == id).ToList();
        public List<Review> GetDishReviews(int id) => this.Reviews.Where(x => x.Dish == id).ToList();
        public List<Restaurant> GetPendingRestraurants() => this.Restaurants.Where(x => x.RestaurantStatus == this.ObjectStatuses.FirstOrDefault(y => y.StatusName == "Pending").StatusId).ToList();
        public List<Restaurant> GetOwnerRestaurants(int ownerId) => this.Restaurants.Where(x => x.OwnerId == ownerId).ToList();
        
    }
}