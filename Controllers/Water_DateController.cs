using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using PlantDatabase.Models;
using System.Web.Script.Serialization;
using PlantDatabase.Models.ViewModels;

namespace PlantDatabase.Controllers
{
    public class Water_DateController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static Water_DateController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44338/api/");
        }

        // GET: Water_Date/List
        // objective: communicate with water_date data api to retrieve list of water_dates
        public ActionResult List()
        {
            string url = "water_datedata/listwater_dates";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Water_DateDto> water_dates = response.Content.ReadAsAsync<IEnumerable<Water_DateDto>>().Result;

            return View(water_dates);
        }

        // GET: Water_Date/Details/5
        // objective: communicate with water_date data api to retrieve one water_date
        public ActionResult Details(int id)
        {
            string url = "water_datedata/findwater_date/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Water_DateDto selectedwater_date = response.Content.ReadAsAsync<Water_DateDto>().Result;


            return View(selectedwater_date);
        }

        // GET: Water_Date/New
        public ActionResult New()
        {
            string url = "plantdata/listplants";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PlantDto> PlantOptions = response.Content.ReadAsAsync<IEnumerable<PlantDto>>().Result;

            return View(PlantOptions);
        }

        // POST: Water_Date/Create
        // objective: add new water_date into system using api
        [HttpPost]
        public ActionResult Create(Water_Date water_date)
        {
            string url = "water_datedata/addwater_date";

            string jsonpayload = jss.Serialize(water_date);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Water_Date/Edit/5
        // objective: get existing selected water_date data to edit
        public ActionResult Edit(int id)
        {
            UpdateWater_Date ViewModel = new UpdateWater_Date();
            
            string url = "water_datedata/findwater_date/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Water_DateDto selectedwater_date = response.Content.ReadAsAsync<Water_DateDto>().Result;
            ViewModel.selectedwater_date = selectedwater_date;

            url = "plantdata/listplants/";
            response = client.GetAsync(url).Result;
            IEnumerable<PlantDto> PlantOptions = response.Content.ReadAsAsync<IEnumerable<PlantDto>>().Result;
            ViewModel.PlantOptions = PlantOptions;

            return View(ViewModel);
        }

        // POST: Water_Date/Update/5
        [HttpPost]
        public ActionResult Update(int id, Water_Date water_date)
        {
            string url = "water_datedata/updatewater_date/" + id;
            string jsonpayload = jss.Serialize(water_date);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Water_Date/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "water_datedata/findwater_date/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Water_DateDto selectedwater_date = response.Content.ReadAsAsync<Water_DateDto>().Result;
            return View(selectedwater_date);
        }

        // POST: Water_Date/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "water_datedata/deletewater_date/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
