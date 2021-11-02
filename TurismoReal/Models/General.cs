using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TurismoReal.Models
{
    public class General
    {

        public class Retorno
        {
            public string Codigo { get; set; }
            public string Mensaje { get; set; }
        }
        public class Token
        {
            public string acces_token { get; set; }
            public RefreshToken refresh_token { get; set; }
        }

        
        public class RefreshToken
        {
            [JsonPropertyName("username")]
            public string UserName { get; set; }    // can be used for usage tracking
                                                    // can optionally include other metadata, such as user agent, ip address, device name, and so on

            [JsonPropertyName("tokenString")]
            public string TokenString { get; set; }

            [JsonPropertyName("expireAt")]
            public DateTime ExpireAt { get; set; }
        }

    }
}
