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
    public class HomeCookieController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Listings;Integrated Security=True";

        public IActionResult IndexCookie()
        {
            var db = new ListingsDb(_connectionString);
            var vm = new ListingsViewModel();
            vm.Listings = db.GetListings();

            List<int> ids = new List<int>();
            if(Request.Cookies["ids"] != null)
            {
                string idString = Request.Cookies["ids"];
                foreach (string num in idString.Split(","))
                    {
                    ids.Add(int.Parse(num));
                }
            }

            vm.Ids = ids;
            return View(vm);
        }
        public IActionResult NewAdCookie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAdCookie(Listing listing)
        {
            ListingsDb db = new ListingsDb(_connectionString);
            listing.DateCreated = DateTime.Now;
            db.AddListing(listing);

            String ids = Request.Cookies["ids"];
            if(ids != null)
            {
                ids += ",";
            }
            ids += $"{listing.Id}";
            Response.Cookies.Append("ids", ids);
            return Redirect("/HomeCookie/indexCookie");
        }
        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            ListingsDb db = new ListingsDb(_connectionString);
            db.DeleteListing(id);
            return Redirect("/HomeCookie/IndexCookie");
        }
    }
 
}

