using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.ControllersModels
{
    public class User
    {
        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("firstname")]
        public String FirstName { get; set; }

        [JsonProperty("surname")]
        public String LastName { get; set; }

        [JsonProperty("level")]
        public Int32 Level { get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        [JsonProperty("birth")]
        public Int16 BirthYear { get; set; }

        [JsonProperty("bio")]
        public String Bio { get; set; }

        [JsonProperty("picture")]
        public String Picture { get; set; }
    }
}
