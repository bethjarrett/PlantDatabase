using System;
using System.IO;
using System.Web;
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
        /// <summary>
        /// returns all plants in database
        /// </summary>
        /// <returns>
        /// all plants in db, including their water dates
        /// </returns>
        /// <example>
        /// GET: api/PlantData/ListPlants
        /// </example>
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
                planthaspic = p.planthaspic,
                PicExtension = p.PicExtension,
            }));
            return PlantDtos;
        }
        /// <summary>
        /// returns all plants in database
        /// </summary>
        /// <returns>
        /// plant in database matching plant ID primary key
        /// </returns>
        /// <param name="id">primary key of plant</param>
        /// <example>
        /// GET: api/PlantData/FindPlant/5
        /// </example>
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
                planthaspic = Plant.planthaspic,
                PicExtension= Plant.PicExtension,
            };
            if (Plant == null)
            {
                return NotFound();
            }
            return Ok(PlantDto);
        }

        /// <summary>
        /// receives plant picture data, uploads it, updates plant's HasPic
        /// </summary>
        /// <param name="id">plant id</param>
        /// <example>
        /// POST: api/PlantData/UpdatePlantPic/3
        /// </example>
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
            db.Entry(plant).Property(a => a.planthaspic).IsModified = false;
            db.Entry(plant).Property(a => a.PicExtension).IsModified = false;
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
        [HttpPost]
        public IHttpActionResult UploadPlantPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                int numfiles = HttpContext.Current.Request.Files.Count;
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var plantPic = HttpContext.Current.Request.Files[0];
                    if (plantPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(plantPic.FileName).Substring(1);
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                string fn = id + "." + extension;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Plants/"), fn);
                                plantPic.SaveAs(path);
                                haspic = true;
                                picextension = extension;
                                Plant Selectedplant = db.Plants.Find(id);
                                Selectedplant.planthaspic = haspic;
                                Selectedplant.PicExtension = extension;
                                db.Entry(Selectedplant).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                return BadRequest();
                            }
                        }
                    }
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// adds plant to database
        /// </summary>
        /// <param name="plant">JSON FORM DATA of plant</param>
        /// <returns>
        /// plant ID and plant data
        /// </returns>
        /// <example>
        /// POST: api/PlantData/AddPlant
        /// </example>
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
        /// <summary>
        /// deletes plant from database by its ID
        /// </summary>
        /// <param name="id">primary key of plant</param>
        /// <example>
        /// POST: api/PlantData/DeletePlant/5
        /// </example>
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