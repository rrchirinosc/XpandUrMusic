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
using XpandUrMusic.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace XpandUrMusic.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationOptions ApplicationOptions { get; set; }
        private REST rest;
        private IMemoryCache cache;

        public HomeController(
            IOptions<ApplicationOptions> applicationOptions,
            IMemoryCache memCache)            
        {
            ApplicationOptions = applicationOptions.Value;
            rest = new REST();
            cache = memCache;
        }

        public async Task <IActionResult> Index()
        {
            ErrorViewModel model = new();

            // get Spotify app token and store it in cache
            // TODO: handle token expiration/renewal
            await RetrieveToken();
            
            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<IActionResult> Recommendations(string artistsIDs, string genresIDs)
        {
            RecommendationsViewModel model = new();
            RecommendationsResponseDTO recommendations;
            recommendations = await GetRecommendations(artistsIDs, genresIDs);

            // fill release year for all recommended albums
            for (int i = 0; i < recommendations.Tracks.Length; i++)
            {
                recommendations.Tracks[i].Album.Release_Year =
                    await rest.GetAlbumReleaseYearAsync(await RetrieveToken(), recommendations.Tracks[i].Album.ID);
            }

            model.Recommendations = recommendations;
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> ArtistLookup(string artistName)
        {
            List<ArtistDisplayDataDTO> artistsList = new();
            ArtistsContainerDTO artists = await rest.GetArtistsAsync(await RetrieveToken(), artistName);

            if(artists != null)
            {
                foreach(ArtistDTO artist in artists.Artists.Items)
                {
                    ArtistDisplayDataDTO artistData = new();
                    artistData.ID = artist.ID;
                    artistData.Name = artist.Name;
                    artistData.Image = (artist.Images.Length > 0 ? artist.Images[0] : null);    //first or none
                    artistsList.Add(artistData);
                }
            }

            return Json(artistsList);
        }


        public async Task<RecommendationsResponseDTO> GetRecommendations(string artistsIDs, string genresIDs)
        {
            // double work here wanted a list of strings but got a string of comma separated items
            // so first to an array the passed as List
            // TODO: see if possible to receive a list of strings from js
            List<string> artists = artistsIDs != null ? artistsIDs.Split(',').ToList() : null;
            List<string> genres = genresIDs != null ? genresIDs.Split(',').ToList() : null;

            RecommendationsResponseDTO recommendations = await rest.GetRecommendationsAsync(await RetrieveToken(), artists, genres);

            return recommendations;
        }

        public async Task<JsonResult> GetGenres()
        {
            GenresDTO genresDTO = await RetrieveGenres();//rest.GetGenresAsync(await GetToken());

            string[] genres = genresDTO.Genres;
            return Json(genres);
        }

        public async Task<string> RetrieveToken()
        {
            string key = "Token";
            string obj;
            if (!cache.TryGetValue<string>(key, out obj))
            {
                obj = await rest.GetClientCredentialsAuthTokenAsync(ApplicationOptions.SpotifyClientKey);
                cache.Set<string>(key, obj);
            }
            return obj;
        }

        public async Task<GenresDTO> RetrieveGenres()
        {
            string key = "Genres";
            GenresDTO obj;
            if (!cache.TryGetValue<GenresDTO>(key, out obj))
            {
                obj = await rest.GetGenresAsync(await RetrieveToken());
                cache.Set<GenresDTO>(key, obj);
            }
            return obj;
        }
    }
}
