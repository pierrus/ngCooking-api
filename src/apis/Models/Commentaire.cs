using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Models
{
    public class Commentaire
    {
        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("member")]
        public User User { get; set; }

        [JsonProperty("userId")]
        public Int32 UserId { get; set; }

        [JsonIgnore]
        public Recette Recette { get; set; }

        [JsonProperty("recetteId")]
        public String RecetteId { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("mark")]
        public Int32 Mark { get; set; }

        [JsonProperty("comment")]
        public String Comment { get; set; }
    }
}
