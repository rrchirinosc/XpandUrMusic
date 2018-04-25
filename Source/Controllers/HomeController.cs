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
        private REST rest;
        static private string token;

        public HomeController(
            IOptions<ApplicationOptions> applicationOptions)
        {
            ApplicationOptions = applicationOptions.Value;
            rest = new REST();
        }

        public async Task <IActionResult> Index()
        {
            ErrorViewModel model = new ErrorViewModel();

            token = await rest.GetClientCredentialsAuthTokenAsync(ApplicationOptions.SpotifyClientKey);

            return View(model);
        }

        public async Task <IActionResult> About()
        {
            ErrorViewModel model = new ErrorViewModel();

            ViewData["Message"] = "Your application description page.";
            model.RequestId = await rest.GetArtistAsync(token, "Muse");
            return View(model);
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

        public async Task<string> ArtistLookup(string artistName)
        {
            return await rest.GetArtistAsync(token, artistName);
        }
    }
}
