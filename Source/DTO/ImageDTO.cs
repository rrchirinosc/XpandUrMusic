using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class ImageDTO
    {
        //The image height in pixels. If unknown: null or not returned.
        public int Height { get; set; }

        //The source URL of the image.
        public string URL { get; set; }

        //The image width in pixels. If unknown: null or not returned.
        public int Width { get; set; }
    }
}
