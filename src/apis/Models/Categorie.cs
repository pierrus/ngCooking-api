using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace apis.Models
{
    public class Categorie
    {
        //[JsonProperty("id")]
        public String Id { get; set; }

        //[JsonProperty("id")]
        public String NameToDisplay { get; set; }
    }
}
