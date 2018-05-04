using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class RecommendationsResponseDTO
    {
        //An array of track object (simplified) ordered 
        //according to the parameters supplied.
        public TracksDTO[] Tracks { get; set; }

        //An array of recommendation seed objects.
        public RecommendationSeedDTO[] Seeds { get; set; }        
    }
}
