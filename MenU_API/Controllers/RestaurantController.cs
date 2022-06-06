using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using MenU_API.DataTransferObjects;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MenU_API.Controllers
{
    [Route("restaurants")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        MenUContext context;
        public RestaurantController() { context = new MenUContext(); }


        [Route("GetOwnersRestaurants")]
        [HttpGet]
        public List<Restaurant> GetOwnersRestaurants([FromQuery] int ownerId)
        {
            try
            {
                List<Restaurant> restaurants = context.GetOwnerRestaurants(ownerId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return restaurants;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [Route("ChangeRestaurantStatus")]
        [HttpGet]
        public void ChangeRestaurantStatus([FromQuery] int statusId, [FromQuery] int resId)
        {
            try
            {
                context.UpdateRestaurantStatus(resId, statusId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
        }

        [Route("FindRestaurantById")]
        [HttpGet]
        public Restaurant FindRestaurantById([FromQuery] string resId)
        {
            try
            {
                int resIdInt = int.Parse(resId);
                Restaurant r = context.FindRestaurantById(resIdInt);
                if (r != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return r;
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
            }
            catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }
            return null;
        }

        [Route("FindRestaurantsByLetters")]
        [HttpGet]
        public List<Restaurant> FindRestaurantsByLetters(string letters)
        {
            try
            {
                List<Restaurant> restaurants = context.FindRestaurantWithLetters(letters);
                if(restaurants != null && restaurants.Count > 0)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return restaurants;
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
            }
            catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }
            return null;
        }

        [Route("GetAllTags")]
        [HttpGet]
        public List<Tag> GetAllTags()
        {
            try
            {
                List<Tag> tags = context.GetAllTags();
                if (tags.Count > 0)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return tags;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [Route("GetAllAllergens")]
        [HttpGet]
        public List<Allergen> GetAllAllergens()
        {
            try
            {
                List<Allergen> allergens = context.GetAllAllergens();
                if (allergens.Count > 0)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return allergens;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }


        [Route("AddDish")]
        [HttpPost]
        public int AddDish([FromBody] DishDTO d)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try 
            {
                if(userDTO != null)
                {
                    Dish addedDish = context.AddDish(d.Dish);
                    foreach (DishTag dt in d.Tags)
                    {
                        dt.DishId = addedDish.DishId;
                    }
                    context.AddAllDishTags(d.Tags);
                    foreach (AllergenInDish ad in d.AllergenInDishes)
                    {
                        ad.DishId = addedDish.DishId;
                    }
                    context.AddAllAllergensToDish(d.AllergenInDishes);

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return addedDish.DishId;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                
            }
            return 0;
        }


        [Route("AddRestaurant")]
        [HttpPost]
        public RestaurantResult AddRestaurant([FromBody] RestaurantDTO r)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if (userDTO != null && userDTO.AccountType == 2 && userDTO.AccountId == r.Restaurant.OwnerId)
                {

                    List<DishDTO> dishDTOs = r.Dishes.ToList();
                    List<Dish> dishes = new List<Dish>();

                    //foreach(DishDTO dish in dishDTOs)
                    //{
                    //    Dish d = dish.Dish;
                    //    d.DishTags = dish.Tags;
                    //    d.AllergenInDishes = dish.AllergenInDishes;
                    //    dishes.Add(d);
                    //}

                    //bool hasWorked = context.AddDishes(dishes);

                    Restaurant restaurant = r.Restaurant;
                    //restaurant.RestaurantTags = r.RestaurantTags.ToList();
                    //hasWorked = hasWorked && context.UpdateRestaurant(restaurant);

                    Restaurant added = context.AddRestaurant(restaurant);
                    if(added != null)
                    {
                        foreach (RestaurantTag rt in r.RestaurantTags)
                        {
                            rt.RestaurantId = added.RestaurantId;
                        }
                        context.AddAllRestaurantTags(r.RestaurantTags);

                        foreach (DishDTO d in dishDTOs)
                        {
                            d.Dish.Restaurant = added.RestaurantId;
                            Dish addedDish = context.AddDish(d.Dish);
                            foreach (DishTag dt in d.Tags)
                            {
                                dt.DishId = addedDish.DishId;
                            }
                            context.AddAllDishTags(d.Tags);
                            foreach (AllergenInDish ad in d.AllergenInDishes)
                            {
                                ad.DishId = addedDish.DishId;
                            }
                            context.AddAllAllergensToDish(d.AllergenInDishes);
                            d.Dish.DishId = addedDish.DishId;
                        }   

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return new RestaurantResult() { Restaurant = added, Dishes = dishDTOs};
                    }
                    else
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    }
                    return null;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return null;
        }

        [Route("UpdateRestaurant")]
        [HttpPost]
        public Restaurant EditRestaurant([FromBody] RestaurantDTO restaurant)
        {
            try
            {
                Restaurant update = context.EditRestaurant(restaurant.Restaurant, restaurant.RestaurantTags);
                if (update != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return update;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return null;
                }

            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("PostReview")]
        [HttpPost]
        public int PostReview([FromBody] Review r)
        {
            if(r != null)
            {
                try
                {
                    int id = context.AddReview(r);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return id;
                }
                catch(Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                //Bad Request
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            }
            return -1;
        }

        [Route("GetDishReviews")]
        [HttpGet]
        public List<Review> GetDishReviews([FromQuery] int id)
        {
            if (id > 0)
            {
                try
                {
                    List<Review> reviews = context.GetDishReviews(id);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return reviews;
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                //Bad Request
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            }
            return null;
        }

        [Route("GetDishById")]
        [HttpGet]
        public Dish GetDishById([FromQuery] int dishId)
        {
            try 
            {
                Dish d = context.GetDishById(dishId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return d;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [Route("GetRestaurantsByTag")]
        [HttpGet]
        public List<Restaurant> GetRestaurantsByTag([FromQuery] int tagId)
        {
            try
            {   //
                List<Restaurant> d = context.GetRestaurantsByTag(tagId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return d;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [Route("GetDishesByTag")]
        [HttpGet]
        public List<Dish> GetDishesByTag([FromQuery] int tagId)
        {
            try
            {   //
                List<Dish> d = context.GetDishesByTag(tagId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return d;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return null;
        }

        [Route("DeleteRestaurant")]
        [HttpGet]
        public bool DeleteRestaurant([FromQuery] int restaurantId)
        {
            try
            {
                context.DeleteRestaurant(restaurantId);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return false;
        }

        [Route("DeleteDish")]
        [HttpGet]
        public bool DeleteDish([FromQuery] int dishId)
        {
            try
            {
                context.DeleteDish(dishId);
                context.SaveChanges();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            return false;
        }

        [Route("Test")]
        [HttpGet]
        public Restaurant Test()
        {
            Restaurant r = context.Restaurants.FirstOrDefault(x => x.RestaurantId == 38);
            return r;
        }
    }
}
