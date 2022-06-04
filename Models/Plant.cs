using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PlantDatabase.Models
{
    public class Plant
    {
        [Key]
        public int plant_id { get; set; }
        public string plant_name { get; set; }
        public int plant_water { get; set; }
        public int plant_humidity { get; set; }
        public int plant_light { get; set; }
    }
    public class PlantDto
    {
        public int plant_id { get; set; }
        public string plant_name { get; set; }
        public int plant_water { get; set; }
        public int plant_humidity { get; set; }
        public int plant_light { get; set; }
    }
}