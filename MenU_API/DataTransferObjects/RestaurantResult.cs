using MenU_BL.Models;
using System.Collections.Generic;

namespace MenU_API.DataTransferObjects
{
    public class RestaurantResult
    {
        public Restaurant Restaurant { get; set; }
        public List<DishDTO> Dishes { get; set; }
    }
}
