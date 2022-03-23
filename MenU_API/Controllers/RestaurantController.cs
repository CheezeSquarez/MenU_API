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


        [Route("GetRestaurants")]
        [HttpGet]
        public List<Restaurant> GetRestaurants([FromQuery] int id)
        {
            try
            {
                List<Restaurant> restaurants = context.GetOwnerRestaurants(id);
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
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
        public Restaurant FindRestaurantById([FromQuery] int resId)
        {
            try
            {
                Restaurant r = context.FindRestaurantById(resId);
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

        [Route("StampDish")]
        [HttpGet]
        public int StampDish()
        {
            try
            {
                Dish d = new Dish() { DishName = "", DishDescription = "", Restaurant = 0, DishTags = new List<DishTag>() };
                context.AddDish(d);
                Response.StatusCode = (int) System.Net.HttpStatusCode.OK;
                return d.DishId;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return -1;
            }
        }

        [Route("AddDish")]
        [HttpPost]
        public bool AddDish([FromBody] Dish d)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if(userDTO != null)
                {
                    if (context.UpdateDish(d))
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
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
            return false;
        }

        [Route("StampRestaurant")]
        [HttpGet]
        public int StampRestaurant()
        {
            try
            {
                Restaurant r = new Restaurant() { RestaurantName = "", StreetName = "", OwnerId = 0, City = "", StreetNumber = "" };
                context.AddRestaurant(r);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return r.RestaurantId;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return -1;
            }
        }

        [Route("AddRestaurant")]
        [HttpPost]
        public bool AddRestaurant([FromBody] RestaurantDTO r)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                //if (userDTO != null && userDTO.AccountType == 2 && userDTO.AccountId == r.Restaurant.OwnerId)
                //{

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
                        }   
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    
                    }
                    else
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    }
                    return false;
                //}
                //else
                //{
                //    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                //}
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return false;
        }

        [Route("EditRestaurant")]
        [HttpPost]
        public Restaurant EditRestaurant([FromBody] RestaurantDTO restaurant)
        {
            try
            {
                Restaurant update = context.

            }
            catch
            {

            }
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
