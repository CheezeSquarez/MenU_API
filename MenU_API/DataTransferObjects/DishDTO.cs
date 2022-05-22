using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;

namespace MenU_API.DataTransferObjects
{
    public class DishDTO
    {
        public DishDTO() { }

        public Dish Dish { get; set; }
        public List<DishTag> Tags { get; set; }
        public List<AllergenInDish> AllergenInDishes { get; set; }
        public FileInfo Img { get; set; }
    }
}
