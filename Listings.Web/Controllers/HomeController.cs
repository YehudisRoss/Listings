using Listings.Data;
using Listings.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Listings.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Listings;Integrated Security=True";
        public IActionResult Index()
        {
            var db = new ListingsDb(_connectionString);
            var vm = new ListingsViewModel();
            vm.Listings = db.GetListings();

            List<int> ids = HttpContext.Session.Get<List<int>>("Ids");
            if (ids != null)
            {
                vm.Ids = ids;
            }
            return View(vm);
        }

        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Listing listing)
        {
            ListingsDb db = new ListingsDb(_connectionString);
            listing.DateCreated = DateTime.Now;
            db.AddListing(listing);

            List<int> ids = HttpContext.Session.Get<List<int>>("Ids");
            if (ids == null)
            {
                ids = new List<int>();
            }

            ids.Add(listing.Id);
            HttpContext.Session.Set("Ids", ids);
            return Redirect("/Home/index");
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            ListingsDb db = new ListingsDb(_connectionString);
            db.DeleteListing(id);
            return Redirect("/Home/Index");
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) :
            JsonConvert.DeserializeObject<T>(value);
        }
    }
}

