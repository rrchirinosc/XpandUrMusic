using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace XpandUrMusic.DTO
{
    public class AuthenticationResponseDTO
    {
        public string Access_Token { get; set; }

        public string Token_Type { get; set; }

        public int Expires_In { get; set; }

        public string Scope { get; set; }
    }
}
