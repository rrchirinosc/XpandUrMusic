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

        public async Task<string> GetClientCredentialsAuthTokenAsync(string key)
        {
            string token;

            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials") });

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes(key));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Add authHeader without validation since it may look like more than one string
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Basic " + authHeader);

            HttpResponseMessage response = new HttpResponseMessage();
            response = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            token = "No token";

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;
                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                AuthenticationResponseDTO auth = (AuthenticationResponseDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(AuthenticationResponseDTO));

                token = auth.Access_Token;
            }

            return token;
        }

        public async Task<string> GetArtistAsync(string token, string artist)
        {
            int artistsCount = 10;
            string ShowArtists = "";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string value = "Bearer " + token;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", value);

            HttpResponseMessage response = new HttpResponseMessage();
            string uri = string.Format("https://api.spotify.com/v1/search?" + 
                                        "q={0}" + "&type=artist&market=US&limit={1}&offset=0", artist, artistsCount);
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var httpContent = response.Content;

                StreamReader reader = new StreamReader(await httpContent.ReadAsStreamAsync(), Encoding.UTF8);
                ParentArtistsDTO artists = (ParentArtistsDTO)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(ParentArtistsDTO));

                string found = "Found Artists:";
                foreach(ArtistDTO a in artists.Artists.Items)
                {
                    found = string.Format(" {0},", a.Name);
                    ShowArtists += found;
                }
            }
            else
                ShowArtists = "No Artists found";

            return ShowArtists;
        }
    }
}
