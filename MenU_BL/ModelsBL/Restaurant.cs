using System;
using System.Collections.Generic;
using System.Text;

namespace MenU_BL.Models
{
    partial class Restaurant
    {
        public void UpdateStatus(int statusId) => this.RestaurantStatus = statusId;
            
    }
}
