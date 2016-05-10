using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class ImportController : Controller
    {
        Models.NgContext _context;
        UserManager<Models.User> _userManager;

        public ImportController(Models.NgContext context, UserManager<Models.User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IEnumerable<Models.Categorie>> Get()
        {
            Newtonsoft.Json.JsonSerializer jsonSer = new Newtonsoft.Json.JsonSerializer();

            #region Users

            List<Models.User> users;

            using (var str = new System.IO.FileStream("Json/communaute.json", System.IO.FileMode.Open))
            {
                var sr = new System.IO.StreamReader(str);
                var reader = new Newtonsoft.Json.JsonTextReader(sr);

                users = jsonSer.Deserialize<List<Models.User>>(reader);

                foreach (var user in users)
                {
                    user.JsonId = user.Id;
                    user.Id = default(int);

                    if (!_context.Users.Any(u => u.Email == user.Email))
                    {
                        user.UserName = user.Email;
                        var result = await _userManager.CreateAsync(user, user.Password);
                    }
                }
            }

            _context.SaveChanges();

            #endregion

            #region categories

            using (var str = new System.IO.FileStream("Json/categories.json", System.IO.FileMode.Open))
            {
                var sr = new System.IO.StreamReader(str);
                var reader = new Newtonsoft.Json.JsonTextReader(sr);

                List<Models.Categorie> categories = jsonSer.Deserialize<List<Models.Categorie>>(reader);

                foreach (var cat in categories)
                {
                    if (!_context.Categories.Any(c => c.Id == cat.Id))
                        _context.Categories.Add(cat);
                }
            }

            _context.SaveChanges();

            #endregion

            #region Ingredients

            using (var str = new System.IO.FileStream("Json/ingredients.json", System.IO.FileMode.Open))
            {
                var sr = new System.IO.StreamReader(str);
                var reader = new Newtonsoft.Json.JsonTextReader(sr);

                List<Models.Ingredient> ingredients = jsonSer.Deserialize<List<Models.Ingredient>>(reader);

                foreach (var ingredient in ingredients)
                {
                    if (!_context.Categories.Any(c => c.Id == ingredient.CategoryId))
                        _context.Categories.Add(new Models.Categorie { Id = ingredient.CategoryId.ToLower(), NameToDisplay = ingredient.CategoryId });

                    if (!_context.Ingredients.Any(ig => ig.Id == ingredient.Id))
                        _context.Ingredients.Add(ingredient);
                }
            }

            _context.SaveChanges();

            #endregion

            #region Recettes

            using (var str = new System.IO.FileStream("Json/recettes.json", System.IO.FileMode.Open))
            {
                var sr = new System.IO.StreamReader(str);
                var reader = new Newtonsoft.Json.JsonTextReader(sr);

                List<Models.Recette> recettes = jsonSer.Deserialize<List<Models.Recette>>(reader);

                foreach (var recette in recettes)
                {
                    if (!_context.Recettes.Any(r => r.Id == recette.Id))
                    {
                        Models.User creator = users.Where(u => u.JsonId == recette.CreatorId).FirstOrDefault();
                        recette.CreatorId = creator.Id;
                        _context.Recettes.Add(recette);
                    }

                    foreach (String ingredientId in recette.Ingredients)
                    {
                        if (!_context.Ingredients.Any(ig => ig.Id == ingredientId))
                            _context.Ingredients.Add(new Models.Ingredient { Calories = 100, Id = ingredientId, IsAvailable = true, Name = ingredientId, Picture = "pomme-de-terre.jpg", CategoryId = "vegetable" });

                        if (!_context.IngredientsRecettes.Any(igr => igr.IngredientId == ingredientId && igr.RecetteId == recette.Id))
                            _context.IngredientsRecettes.Add(new Models.IngredientRecette { IngredientId = ingredientId, RecetteId = recette.Id });
                    }

                    if (recette.Commentaires != null)
                        foreach (Models.Commentaire comment in recette.Commentaires)
                        {
                            Models.User creator = users.Where(u => u.JsonId == comment.UserId).FirstOrDefault();
                            comment.UserId = creator.Id;

                            if (!_context.Commentaires.Any(c => c.UserId == comment.UserId && c.RecetteId == recette.Id))
                            {
                                comment.RecetteId = recette.Id;

                                _context.Commentaires.Add(comment);
                            }
                        }
                }
            }

            _context.SaveChanges();

            #endregion
                        

            return _context.Ingredients.Select<Models.Ingredient, Models.Categorie>(i => i.Category);
        }
    }
}
