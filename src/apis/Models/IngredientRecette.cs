using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Models
{
    public class IngredientRecette
    {
        public String IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        public String RecetteId { get; set; }

        public Recette Recette { get; set; }
    }
}
