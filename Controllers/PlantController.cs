using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PlantDatabase.Models;
using PlantDatabase.Models.ViewModels;
using System.Web.Script.Serialization;

namespace PlantDatabase.Controllers
{
    public class PlantController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlantController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44338/api/");
        }
        /// <summary>
        /// grabs authentication cookie sent to this controller
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Plant/List
        // objective: communicate with plant data api to retrieve list of plants
        public ActionResult List()
        {
            string url = "plantdata/listplants";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PlantDto> plants = response.Content.ReadAsAsync<IEnumerable<PlantDto>>().Result;

            return View(plants);
        }

        // GET: Plant/Details/5
        // objective: communicate with plant data api to retrieve one plant
        public ActionResult Details(int id)
        {
            DetailsPlant ViewModel = new DetailsPlant();   
            string url = "plantdata/findplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlantDto selectedplant = response.Content.ReadAsAsync<PlantDto>().Result;
            ViewModel.selectedplant = selectedplant;
            
            url = "water_datedata/listwater_datesforplant/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<Water_DateDto> RelatedWater_Date = response.Content.ReadAsAsync<IEnumerable<Water_DateDto>>().Result;
            ViewModel.RelatedWater_Date = RelatedWater_Date;

            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }
        // GET: Plant/New
        public ActionResult New()
        {
            return View();
        }
        // POST: Plant/Create
        // objective: add new plant into system using api
        [HttpPost]
        public ActionResult Create(Plant plant)
        {
            string url = "plantdata/addplant";
            string jsonpayload = jss.Serialize(plant);
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
        // GET: Plant/Edit/5
        // objective: get existing selected plant data to edit
        public ActionResult Edit(int id)
        {
            string url = "plantdata/findplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlantDto selectedplant = response.Content.ReadAsAsync<PlantDto>().Result;
            
            return View(selectedplant);
        }
        // POST: Plant/Update/5
        // objective: update values for selected plant
        [HttpPost]
        public ActionResult Update(int id, Plant plant, HttpPostedFileBase PlantPic)
        {
            string url = "plantdata/updateplant/" + id;
            string jsonpayload = jss.Serialize(plant);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode && PlantPic != null)
            {
                url = "PlantData/UploadPlantPic/" + id;
                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(PlantPic.InputStream);
                requestcontent.Add(imagecontent, "PlantPic", PlantPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Plant/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "plantdata/findplant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlantDto selectedplant = response.Content.ReadAsAsync<PlantDto>().Result;
            return View(selectedplant);
        }
        // POST: Plant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "plantdata/deleteplant/" + id;
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
