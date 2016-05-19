using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using apis.Models;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class RecettesController : Controller
    {
        NgContext _context;

        public RecettesController(NgContext context)
        {
            _context = context;
        }

        // GET api/recettes/5
        [HttpGet]
        public async Task<dynamic> Get([FromQuery] String id)
        {
            if (String.IsNullOrEmpty(id))
            {
                var recettes = _context.Recettes
                .Include(r => r.Commentaires)
                .ThenInclude(c => c.User)
                .Include(r => r.IngredientsRecettes)
                .ThenInclude(ir => ir.Ingredient);

                await recettes.ForEachAsync(r => r.Calories = r.IngredientsRecettes.Sum(i => i.Ingredient.Calories));

                return recettes;
            }
            else
            {
                var recette = _context.Recettes.Where(r => r.Id == id)
                    .Include(r => r.Commentaires)
                    .ThenInclude(c => c.User)
                    .Include(r => r.IngredientsRecettes)
                    .ThenInclude(ir => ir.Ingredient)
                    .FirstOrDefault();

                recette.Calories = recette.IngredientsRecettes.Sum(i => i.Ingredient.Calories);


                return recette;
            }
        }

        [HttpPut]
        public void Put(Recette recipe)
        {
            recipe.Id = recipe.Name.Replace(" ", "-").ToLower();

            if (_context.Recettes.Any(r => r.Id == recipe.Id))
                recipe.Id = String.Format("{0}-{1}", recipe.Id, Guid.NewGuid());

            recipe.IsAvailable = true;

            _context.Recettes.Add(recipe);

            _context.SaveChanges();

            if (recipe.IngredientsRecettes == null)
                recipe.IngredientsRecettes = new List<IngredientRecette>();

            foreach (String ingredientId in recipe.Ingredients)
            {
                Ingredient ig = _context.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();

                if (ig != null)
                    recipe.IngredientsRecettes.Add(new IngredientRecette { IngredientId = ig.Id, RecetteId = recipe.Id });
            }

            _context.SaveChanges();
        }
    }
}
