using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class AlbumDTO
    {
        public string Album_Type { get; set; }

        //The artists of the album.
        //Each artist object includes a link in href to more detailed 
        //information about the artist.
        public ArtistDTO[] Artists { get; set; }

        //A link to the Web API endpoint providing full details of the album.
        public string Href { get; set; }

        //The Spotify ID for the album.
        public string ID { get; set; }

        //The cover art for the album in various sizes, widest first.
        public ImageDTO[] Images { get; set; }

        //The name of the album. In case of an album takedown, 
        //the value may be an empty string.
        public string Name { get; set; }

        //The date the album was first released, for example 1981. 
        //Depending on the precision, it might be shown as 1981-12 or 1981-12-15.
        public string Release_Date { get; set; }

        public string Release_Date_Precision { get; set; }

        //not part of the Spotify Album object, filled using Release_Date property
        public string Release_Year { get; set; }
    }
}
