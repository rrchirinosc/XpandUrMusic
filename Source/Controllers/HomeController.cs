using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XpandUrMusic.Models;
using XpandUrMusic.Infrastructure;
using Microsoft.Extensions.Options;

namespace XpandUrMusic.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationOptions ApplicationOptions { get; set; }
        private string appID;
        private string appSecret;

        static void GetSettings()
        {
            
        }

        public  HomeController(
            IOptions<ApplicationOptions> applicationOptions)
        {
            ApplicationOptions = applicationOptions.Value;

            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
