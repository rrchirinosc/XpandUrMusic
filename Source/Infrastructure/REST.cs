using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using XpandUrMusic.DTO;

namespace XpandUrMusic.Infrastructure
{
    public class REST
    {
        static private HttpClient client = new HttpClient();

        public REST()
        {
        }

        // Fetch access token from Spotify
        public async Task<string> GetClientCredentialsAuthTokenAsync(string key)
        {
            string token = null;
            string Endpoint = "https://accounts.spotify.com/api/token";

            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials") });

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes(key));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Add authHeader without validation since it may look like more than one string
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Basic " + authHeader);

            HttpResponseMessage response = new HttpResponseMessage();
            response = await client.PostAsync(Endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;
                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                AuthenticationResponseDTO auth = (AuthenticationResponseDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(AuthenticationResponseDTO));

                token = auth.Access_Token;
            }

            return token;
        }

        // Search for an artist by name
        public async Task<ArtistsContainerDTO> GetArtistsAsync(string token, string artist)
        {
            ArtistsContainerDTO artists = null;
            int artistsCount = 20;
            string Endpoint = "https://api.spotify.com/v1/search?";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string value = "Bearer " + token;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", value);

            HttpResponseMessage response = new HttpResponseMessage();
            string uri = string.Format(Endpoint + 
                                        "q={0}" + "&type=artist&market=US&limit={1}&offset=0", artist, artistsCount);
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;

                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                artists = (ArtistsContainerDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(ArtistsContainerDTO));
            }
            
            return artists;
        }

        // Fetch recommendations based on given artists and/or genres
        public async Task<RecommendationsResponseDTO> GetRecommendationsAsync(string token, List<string> artistsIDs, List<string> genreIDs)
        {
            RecommendationsResponseDTO recommendations = null;
            int maxRecommendations = 10;
            int minPopularity = 50;
            string Endpoint = "https://api.spotify.com/v1/recommendations?";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string value = "Bearer " + token;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", value);

            HttpResponseMessage response = new HttpResponseMessage();
            string requestParameters = string.Format("limit={0}&min_popularity={1}", maxRecommendations, minPopularity);

            //according to the Spotify api only a max of 5 seed strings can be sent to retrieve recommendations
            //we give priority to artists over genre if received more than 5 from the user

            int maxSeeds = 5;

            //build artist IDs seed
            if (artistsIDs != null && artistsIDs.Count() > 0)
            {
                string seedArtists = "";
                
                foreach (string artistID in artistsIDs)
                {
                    if (seedArtists != "")
                        seedArtists += ",";
                    seedArtists += artistID;
                    if (--maxSeeds == 0)
                        break;
                }
                requestParameters += "&seed_artists=" + seedArtists;
            }

            //build artist IDs seed
            if (genreIDs != null && genreIDs.Count() > 0 && maxSeeds > 0)
            {
                string seedGenres = "";
                
                foreach (string genreID in genreIDs)
                {
                    if (seedGenres != "")
                        seedGenres += ",";
                    seedGenres += genreID;
                    if (--maxSeeds == 0)
                        break;
                }
                requestParameters += "&seed_genres=" + seedGenres;
            }

            string uri = string.Format(Endpoint + requestParameters);

            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;

                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                recommendations = (RecommendationsResponseDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(RecommendationsResponseDTO));
            }

            return recommendations;
        }


        // Get list of all available genres
        public async Task<GenresDTO> GetGenresAsync(string token)
        {
            GenresDTO genres = null;
            string Endpoint = "https://api.spotify.com/v1/recommendations/available-genre-seeds";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string value = "Bearer " + token;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", value);

            HttpResponseMessage response = new HttpResponseMessage();

            string uri = Endpoint;
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;

                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                genres = (GenresDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(GenresDTO));
            }

            return genres;
        }

        // Fetch given album and from there its release year
        public async Task<string> GetAlbumReleaseYearAsync(string token, string albumID)
        {
            string releaseYear = null;
            string Endpoint = "https://api.spotify.com/v1/albums/";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string value = "Bearer " + token;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", value);

            HttpResponseMessage response = new HttpResponseMessage();
            string requestParameters = albumID;

            string uri = Endpoint + requestParameters;
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;

                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                AlbumDTO album = (AlbumDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(AlbumDTO));

                // determine release year
                releaseYear = album.Release_Date.Substring(0, 4);
            }

            return releaseYear;
        }
    }
   
}    


