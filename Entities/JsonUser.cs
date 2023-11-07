using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
   
        public class JsonUser
    {
            public string status { get; set; }
        [JsonProperty("data")]

        public List<User> users { get; set; }
            public string message { get; set; }

        }
    
}
