using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class RecommendationSeedDTO
    {
        //A link to the full track or artist data for this seed. 
        //For tracks this will be a link to a Track Object. 
        //For artists a link to an Artist Object. 
        //For genre seeds, this value will be null.
        public string Href { get; set; }

        //The id used to select this seed. 
        //This will be the same as the string used in the seed_artists, 
        //seed_tracks or seed_genres parameter.
        public string ID { get; set; }

        //The entity type of this seed.One of artist, track or genre.
        public string Type { get; set; }

        //The Spotify api contains more keys, only added the above
        //for the sake of this app
    }
}
