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
            r.RestaurantStatus = 1;
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
        public int AddReview(Review r)
        {
            this.Reviews.Add(r);
            this.SaveChanges();
            return r.ReviewId;
        }
        public Dish GetDishById(int id) => this.Dishes
            .Include(a => a.Reviews)
            .Include(b => b.AllergenInDishes).ThenInclude(c => c.Allergen)
            .Include(d => d.DishTags).ThenInclude(e => e.Tag)
            .FirstOrDefault(x => x.DishId == id && x.DishStatus == 1);
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
                 .Include(x => x.Restaurants.Where(a => a.RestaurantStatus == 1)).ThenInclude(z => z.RestaurantTags)
                 .Include(x => x.Restaurants.Where(a => a.RestaurantStatus == 1)).ThenInclude(r => r.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(a => a.DishTags)
                 .Include(b => b.Reviews)
                 .Where(x => x.Username == username && x.Pass == hashedPass).FirstOrDefault();
        }
        public Account Login(string token)
        {
            AccountAuthToken a = this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token);
            if (a != null)
            {
                Account acc = this.Accounts
                    .Include(x => x.Restaurants.Where(a => a.RestaurantStatus == 1)).ThenInclude(z => z.RestaurantTags)
                    .Include(x => x.Restaurants.Where(a => a.RestaurantStatus == 1))
                    .ThenInclude(r => r.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(a => a.DishTags)
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
                .Include(y => y.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(b => b.DishTags)
                .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(c => c.Reviews).Where(c => c.RestaurantId == id && c.RestaurantStatus == 1).FirstOrDefault();
        }
        public List<Restaurant> FindRestaurantWithLetters(string letters) => this.Restaurants.Where(x => x.RestaurantName.Contains(letters) && x.RestaurantId != 0 && x.RestaurantStatus == 1).ToList();
        public List<Review> GetUserReviews(int id) => this.Reviews.Include(x => x.DishNavigation).ThenInclude(a => a.RestaurantNavigation).Where(x => x.Reviewer == id).ToList();
        public List<Review> GetRestaurantReviews(int id) => this.Reviews.Where(x => x.DishNavigation.Restaurant == id).ToList();
        public List<Review> GetDishReviews(int id) => this.Reviews.Where(x => x.Dish == id).ToList();
        public List<Restaurant> GetPendingRestraurants() => this.Restaurants.Where(x => x.RestaurantStatus == this.ObjectStatuses.FirstOrDefault(y => y.StatusName == "Pending").StatusId).ToList();
        public List<Restaurant> GetOwnerRestaurants(int ownerId) => this.Restaurants.Where(x => x.OwnerId == ownerId && x.RestaurantStatus == 1)
            .Include(x => x.RestaurantTags)
            .Include(y => y.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(z => z.AllergenInDishes)
            .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(b => b.DishTags)
            .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(v => v.Reviews).ToList();
        public List<Tag> GetAllTags() => this.Tags.ToList();
        public List<Allergen> GetAllAllergens() => this.Allergens.ToList();
        public Dish FindDishByRestaurant(int id) => this.Dishes.FirstOrDefault(x => x.Restaurant == id && x.DishStatus == 1);
        public Dish FindDishByID(int id) =>
            this.Dishes.Include(a => a.Reviews).ThenInclude(b => b.ReviewerNavigation)
            .Include(c => c.RestaurantNavigation)
            .Include(d => d.AllergenInDishes)
            .Include(e => e.DishTags)
            .Where(x => x.DishId == id && x.DishStatus == 1).FirstOrDefault();
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
                .Include(y => y.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(b => b.DishTags).Where(x => x.OwnerId == id && x.RestaurantStatus == 1).FirstOrDefault();
        public Restaurant FindRestaurantByID(int id) => Restaurants
            .Include(x => x.RestaurantTags)
                .Include(y => y.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(z => z.AllergenInDishes)
                .Include(a => a.Dishes.Where(a => a.DishStatus == 1)).ThenInclude(b => b.DishTags).Where(x => x.RestaurantId == id && x.RestaurantStatus == 1).FirstOrDefault();
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
            foreach (RestaurantTag tag in restaurantTags)
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
            foreach (AllergenInDish allergenInDish in allergensInDish)
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
        public void RemoveToken(string token) => this.AccountAuthTokens.Remove(this.AccountAuthTokens.FirstOrDefault(x => x.AuthToken == token));

        public List<Restaurant> GetRestaurantsByTag(int tagId)
        {
            List<int> restaurantIds = this.RestaurantTags.Where(x => x.TagId == tagId).Select(a => a.RestaurantId).ToList();
            List<Restaurant> restaurants = new List<Restaurant>();
            foreach (int i in restaurantIds)
                restaurants.Add(this.Restaurants.FirstOrDefault(x => x.RestaurantId == i && x.RestaurantStatus == 1));
            return restaurants;
        }

        public List<Dish> GetDishesByTag(int tagId)
        {
            List<int> dishIds = this.DishTags.Where(x => x.TagId == tagId).Select(a => a.DishId).ToList();
            List<Dish> dishes = new List<Dish>();
            foreach (int i in dishIds)
                dishes.Add(this.Dishes.Include(x => x.RestaurantNavigation).FirstOrDefault(x => x.DishId == i && x.DishStatus == 1));
            return dishes;
        }

        public void DeleteRestaurant(int restaurantId)
        {
            Restaurant r = this.Restaurants.FirstOrDefault(x => x.RestaurantId == restaurantId);
            if(r != null)
            {
                this.UpdateRestaurantStatus(r.RestaurantId, 3);
                List<Dish> dishesInRestaurant = this.Dishes.Where(x => x.Restaurant == r.RestaurantId).ToList();
                foreach(Dish dish in dishesInRestaurant)
                {
                    this.DeleteDish(dish.DishId);
                }
            }
            SaveChanges();
        }

        public void DeleteDish(int dishId)
        {
            Dish dish = this.Dishes.FirstOrDefault(x => x.DishId == dishId);
            if (dish != null)
                dish.DishStatus = 3;
        }

    }
}