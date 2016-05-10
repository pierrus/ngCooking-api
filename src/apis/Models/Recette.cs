using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Models
{
    public class Recette
    {
        private List<String> _ingredients;

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("isAvailable")]
        public Boolean IsAvailable { get; set; }

        [JsonProperty("picture")]
        public String Picture { get; set; }

        [JsonIgnore]
        public ICollection<IngredientRecette> IngredientsRecettes { get; set; }

        [JsonProperty("ingredients")]
        public List<String> Ingredients
        {
            get
            {
                if (_ingredients != null)
                    return _ingredients;
                else if (IngredientsRecettes != null)
                    return IngredientsRecettes.Select(ir => ir.Ingredient.Id).ToList();
                else
                    return null;
            }
            set
            {
                _ingredients = value;
            }
        }

        [JsonProperty("preparation")]
        public String Preparation { get; set; }

        [JsonProperty("comments")]
        public List<Commentaire> Commentaires { get; set; }

        [JsonProperty("creatorId")]
        public Int32 CreatorId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
