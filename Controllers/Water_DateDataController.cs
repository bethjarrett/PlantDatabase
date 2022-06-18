using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PlantDatabase.Models;

namespace PlantDatabase.Controllers
{
    public class Water_DateDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// returns all water dates in database
        /// </summary>
        /// <returns>
        /// all water dates in db
        /// </returns>
        /// <example>
        /// GET: api/Water_DateData/ListWater_Dates
        /// </example>
        [ResponseType(typeof(Water_DateDto))]
        [HttpGet]
        public IEnumerable<Water_DateDto> ListWater_Dates()
        {
            List<Water_Date> Water_Dates = db.Water_Dates.ToList();
            List<Water_DateDto> Water_DateDtos = new List<Water_DateDto>();
            Water_Dates.ForEach(w => Water_DateDtos.Add(new Water_DateDto()
            {
                water_id = w.water_id,
                water_day = w.water_day,
                plant_id = w.Plant.plant_id,
                plant_name = w.Plant.plant_name

            }));
            return Water_DateDtos;
        }

        [ResponseType(typeof(Water_DateDto))]
        [HttpGet]
        public IHttpActionResult ListWater_DatesForPlant(int id)
        {
            List<Water_Date> Water_Dates = db.Water_Dates.Where(a => a.plant_id == id).ToList();
            List<Water_DateDto> Water_DateDtos = new List<Water_DateDto>();

            Water_Dates.ForEach(w => Water_DateDtos.Add(new Water_DateDto()
            {
                water_id = w.water_id,
                water_day = w.water_day,
                plant_id = w.Plant.plant_id,
                plant_name = w.Plant.plant_name
            }));

            return Ok(Water_DateDtos);
        }

        /// <summary>
        /// returns all water dates in database
        /// </summary>
        /// <returns>
        /// water date in database matching water date ID primary key
        /// </returns>
        /// <param name="id">primary key of water date</param>
        /// <example>
        /// GET: api/Water_DateData/FindWater_Date/5
        /// </example>
        [ResponseType(typeof(Water_DateDto))]
        [HttpGet]
        public IHttpActionResult FindWater_Date(int id)
        {
            Water_Date Water_Date = db.Water_Dates.Find(id);
            Water_DateDto Water_DateDto = new Water_DateDto()
            {
                water_id = Water_Date.water_id,
                water_day = Water_Date.water_day,
                plant_id = Water_Date.Plant.plant_id,
                plant_name = Water_Date.Plant.plant_name,
            };
            if (Water_Date == null)
            {
                return NotFound();
            }

            return Ok(Water_DateDto);
        }
        /// <summary>
        /// receives water date data, updates based on input
        /// </summary>
        /// <param name="id">water date id</param>
        /// <example>
        /// POST: api/Water_DateData/UpdateWater_Date/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWater_Date(int id, Water_Date water_date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != water_date.water_id)
            {
                return BadRequest();
            }
            db.Entry(water_date).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Water_DateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// adds water date to database
        /// </summary>
        /// <param name="water_date">JSON FORM DATA of water date</param>
        /// <returns>
        /// water date ID and plant data
        /// </returns>
        /// <example>
        /// POST: api/Water_DateData/AddWater_Date
        /// </example>
        [ResponseType(typeof(Water_Date))]
        [HttpPost]
        public IHttpActionResult AddWater_Date(Water_Date water_date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Water_Dates.Add(water_date);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = water_date.water_id }, water_date);
        }
        /// <summary>
        /// deletes water date from database by its ID
        /// </summary>
        /// <param name="id">primary key of water date</param>
        /// <example>
        /// POST: api/Water_DateData/DeleteWater_Date/5
        /// </example>
        [ResponseType(typeof(Water_Date))]
        [HttpPost]
        public IHttpActionResult DeleteWater_Date(int id)
        {
            Water_Date water_date = db.Water_Dates.Find(id);
            if (water_date == null)
            {
                return NotFound();
            }

            db.Water_Dates.Remove(water_date);
            db.SaveChanges();

            return Ok(water_date);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool Water_DateExists(int id)
        {
            return db.Water_Dates.Count(e => e.water_id == id) > 0;
        }
    }
}