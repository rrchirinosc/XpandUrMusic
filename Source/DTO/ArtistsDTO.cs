using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class ArtistsDTO
    {
        public string Href { get; set; }

        public ArtistDTO[] Items { get; set; }

        public int Limit { get; set; }

        public string Next { get; set; }

        public int Offset { get; set; }

        public string Previous { get; set; }

        public int Total { get; set; }
    }
}
