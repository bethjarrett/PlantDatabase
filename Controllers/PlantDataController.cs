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
    public class PlantDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PlantData/ListPlants
        [HttpGet]
        public IEnumerable<PlantDto> ListPlants()
        {
            List<Plant> Plants = db.Plants.ToList();
            List<PlantDto> PlantDtos = new List<PlantDto>();

            Plants.ForEach(p => PlantDtos.Add(new PlantDto()
            {
                plant_id = p.plant_id,
                plant_name = p.plant_name,
                plant_humidity = p.plant_humidity,
                plant_light = p.plant_light,
                plant_water = p.plant_water,
            }));

            return PlantDtos;
        }

        // GET: api/PlantData/FindPlant/5
        [ResponseType(typeof(PlantDto))]
        [HttpGet]
        public IHttpActionResult FindPlant(int id)
        {
            Plant Plant = db.Plants.Find(id);
            PlantDto PlantDto = new PlantDto()
            {
                plant_id = Plant.plant_id,
                plant_name = Plant.plant_name,
                plant_humidity = Plant.plant_humidity,
                plant_light = Plant.plant_light,
                plant_water = Plant.plant_water,
            };
            if (Plant == null)
            {
                return NotFound();
            }
            return Ok(PlantDto);
        }

        // POST: api/PlantData/UpdatePlant/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePlant(int id, Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != plant.plant_id)
            {
                return BadRequest();
            }
            db.Entry(plant).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
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

        // POST: api/PlantData/AddPlant
        [ResponseType(typeof(Plant))]
        [HttpPost]
        public IHttpActionResult AddPlant(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Plants.Add(plant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = plant.plant_id }, plant);
        }

        // POST: api/PlantData/DeletePlant/5
        [ResponseType(typeof(Plant))]
        [HttpPost]
        public IHttpActionResult DeletePlant(int id)
        {
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return NotFound();
            }

            db.Plants.Remove(plant);
            db.SaveChanges();

            return Ok(plant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlantExists(int id)
        {
            return db.Plants.Count(e => e.plant_id == id) > 0;
        }
    }
}