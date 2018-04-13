using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpandUrMusic.Infrastructure
{

    public class ApplicationOptions
    {
        public string ApplicationName { get; set; }
        public string SpotifyClientID { get; set; }
        public string SpotifySecret { get; set; }
    }
}
