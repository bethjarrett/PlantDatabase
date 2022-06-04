using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantDatabase.Models
{
    public class Water_Date
    {
        [Key]
        public int water_id { get; set; }
        public DateTime water_day { get; set; }
        
        [ForeignKey("Plant")]
        public int plant_id { get; set; }
        public virtual Plant Plant { get; set; }
    }
    public class Water_DateDto
    {
        public int water_id { get; set; }
        public DateTime water_day { get; set; }
        public int plant_id { get; set; }
        public string plant_name { get; set; }

    }
}