using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    // This is a partial Spotify simplified artist object
    // Some properties are purposedly omitted for not being relevant
    // for the project at hand
    public class ArtistDTO
    {
        //Known external URLs for this artist.
        public ExternalURLDTO ExternalURLs { get; set; }

        //Information about the followers of the artist
        public FollowerObjectDTO Followers { get; set; }

        //A list of the genres the artist is associated with.
        //For example: "Prog Rock" , "Post-Grunge". (If not yet classified, the array is empty.)
        public string[] Genres { get; set; }

        //A link to the Web API endpoint providing full details of the artist.
        public string Href { get; set; }

        //The Spotify ID for the artist.
        public string ID { get; set; }

        //Images of the artist in various sizes, widest first.
        public ImageDTO[] Images { get; set; }

        //The name of the artist
        public string Name { get; set; }

        public int Popularity { get; set; }

        //The object type: "artist"
        public string Type { get; set; }

        //The Spotify URI for the artist.
        public string URI { get; set; }
    }
}
