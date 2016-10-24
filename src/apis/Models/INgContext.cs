using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Models
{
    public interface INgContext
    {
        DbSet<Ingredient> Ingredients { get; set; }

        DbSet<Recette> Recettes { get; set; }

        DbSet<Categorie> Categories { get; set; }

        DbSet<Commentaire> Commentaires { get; set; }

        DbSet<IngredientRecette> IngredientsRecettes { get; set; }

        int SaveChanges();
    }
}
