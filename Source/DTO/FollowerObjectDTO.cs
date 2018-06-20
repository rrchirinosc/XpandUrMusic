using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    // Spotify follower object
    public class FollowerObjectDTO
    {
        public string Href { get; set; }

        public int Total { get; set; }
    }
}
