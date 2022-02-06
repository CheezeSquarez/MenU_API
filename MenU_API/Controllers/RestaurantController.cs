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

        [Route("FindRestaurantByLetters")]
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
                if (userDTO != null && userDTO.AccountType == 2)
                {
                    List<DishDTO> dishDTOs = r.Dishes.ToList();
                    List<Dish> dishes = new List<Dish>();
                    foreach(DishDTO dish in dishDTOs)
                    {
                        Dish d = dish.Dish;
                        d.DishTags = dish.Tags;
                        d.AllergenInDishes = dish.AllergenInDishes;
                        dishes.Add(d);
                    }

                    bool hasWorked = context.AddDishes(dishes);

                    Restaurant restaurant = r.Restaurant;
                    restaurant.RestaurantTags = r.RestaurantTags.ToList();
                    hasWorked = hasWorked && context.UpdateRestaurant(restaurant);

                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return hasWorked;
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

        [Route("Test")]
        [HttpPost]
        public void Test([FromBody] Credentials str)
        {
            //string str = "{\"$id\":\"1\",\"RestaurantId\":29,\"RestaurantName\":\"gsasg\",\"StreetName\":\"sgsg\",\"OwnerId\":15,\"City\":\"afaf\",\"RestaurantPicture\":null,\"StreetNumber\":\"124\",\"RestaurantStatus\":0,\"Owner\":null,\"RestaurantStatusNavigation\":null,\"Dishes\":{\"$id\":\"2\",\"$values\":[{\"$id\":\"3\",\"DishId\":25,\"DishName\":\"sgsg\",\"DishDescription\":\"sggssg\",\"Restaurant\":29,\"DishStatus\":0,\"DishPicture\":null,\"DishStatusNavigation\":null,\"RestaurantNavigation\":null,\"AllergenInDishes\":{\"$id\":\"4\",\"$values\":[{\"$id\":\"5\",\"AllergenId\":9,\"DishId\":25,\"Allergen\":null,\"Dish\":null},{\"$id\":\"6\",\"AllergenId\":10,\"DishId\":25,\"Allergen\":null,\"Dish\":null},{\"$id\":\"7\",\"AllergenId\":12,\"DishId\":25,\"Allergen\":null,\"Dish\":null}]},\"DishTags\":{\"$id\":\"8\",\"$values\":[{\"$id\":\"9\",\"DishId\":25,\"TagId\":13,\"Dish\":null,\"Tag\":null},{\"$id\":\"10\",\"DishId\":25,\"TagId\":14,\"Dish\":null,\"Tag\":null}]},\"Reviews\":{\"$id\":\"11\",\"$values\":[]}}]},\"RestaurantTags\":{\"$id\":\"12\",\"$values\":[{\"$id\":\"13\",\"TagId\":9,\"RestaurantId\":29,\"Restaurant\":null,\"Tag\":null},{\"$id\":\"14\",\"TagId\":10,\"RestaurantId\":29,\"Restaurant\":null,\"Tag\":null}]}}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };

            try
            {
                Restaurant r = System.Text.Json.JsonSerializer.Deserialize<Restaurant>("", options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            

        }
    }
}
