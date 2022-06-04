using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantDatabase.Models.ViewModels
{
    public class DetailsPlant
    {
        public PlantDto selectedplant { get; set; }

        public IEnumerable<Water_DateDto> RelatedWater_Date { get; set; }

    }
}