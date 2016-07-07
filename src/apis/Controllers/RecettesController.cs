using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using apis.Models;
using Microsoft.AspNetCore.Hosting;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class RecettesController : Controller
    {
        NgContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public RecettesController(NgContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
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

        [HttpPost]
        public void Post(Recette recipe)
        {
            recipe.Id = recipe.Name.Replace(" ", "-").ToLower();

            if (_context.Recettes.Any(r => r.Id == recipe.Id))
                recipe.Id = String.Format("{0}-{1}", recipe.Id, Guid.NewGuid());

            
            byte[] imageBinary = new byte[Request.Form.Files["rawPicture"].Length];

            using (System.IO.Stream fileStream = Request.Form.Files["rawPicture"].OpenReadStream())
                fileStream.Read(imageBinary, 0, imageBinary.Length);

            recipe.IsAvailable = true;
            //Un nom est généré aléatoirement afin de ne pas avoir à se soucier des collisions avec d'autres images sur le file system
            //Gère uniquement les jpg
            recipe.Picture = String.Format("img/recettes/{0}.jpg", Guid.NewGuid().ToString());

            _context.Recettes.Add(recipe);

            _context.SaveChanges();

            if (recipe.IngredientsRecettes == null)
                recipe.IngredientsRecettes = new List<IngredientRecette>();

            //Pas propre
            if (recipe.Ingredients != null && recipe.Ingredients.Count > 0)
                recipe.Ingredients = recipe.Ingredients[0].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (String ingredientId in recipe.Ingredients)
            {
                Ingredient ig = _context.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();

                if (ig != null)
                    recipe.IngredientsRecettes.Add(new IngredientRecette { IngredientId = ig.Id, RecetteId = recipe.Id });
            }

            _context.SaveChanges();

            String pictureFullPath = String.Format("{0}/{1}", _appEnvironment.WebRootPath, recipe.Picture);

            using (System.IO.FileStream fs = new System.IO.FileStream(pictureFullPath, System.IO.FileMode.Create))
                fs.Write(imageBinary, 0, imageBinary.Length);
        }
    }
}
