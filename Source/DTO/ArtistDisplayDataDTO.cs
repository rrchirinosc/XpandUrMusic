using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class ArtistDisplayDataDTO
    {
        //The Spotify ID for the artist
        public string ID { get; set; }

        //First fetch (largest) artist image.
        public ImageDTO Image { get; set; }

        //The name of the artist
        public string Name { get; set; }
    }
}
