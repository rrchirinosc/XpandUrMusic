using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace XpandUrMusic.DTO
{
    // This is a partial Spotify simplified track object
    // Some properties are purposedly omitted for not being relevant
    // for the project at hand
    public class TrackDTO
    {
        //The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist. 
        public ArtistDTO[] Artists { get; set; }

        //A link to the Web API endpoint providing full details of the track.
        public string Href { get; set; }

        //The Spotify ID for the track.
        public string ID { get; set; }

        //The name of the track. 
        public string Name { get; set; }

        //A URL to a 30 second preview(MP3 format) of the track. null if not available.
        public string PreviewUrl { get; set; }

        //The number of the track. If an album has several discs, the track number is the number on the specified disc. 
        public int TrackNumber { get; set; }

        //The object type: "track".
        public string Type { get; set; }

        //The Spotify URI for the track.
        public string URI { get; set; }        
    }
}
