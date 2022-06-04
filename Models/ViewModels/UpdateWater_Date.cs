using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantDatabase.Models.ViewModels
{
    public class UpdateWater_Date
    {
        public Water_DateDto selectedwater_date { get; set; }

        public IEnumerable<PlantDto> PlantOptions { get; set; }
    }
}