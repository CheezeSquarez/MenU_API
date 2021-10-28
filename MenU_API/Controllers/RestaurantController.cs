using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using MenU_API.DataTransferObjects;
using System.Net.Http;


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
    }
}
