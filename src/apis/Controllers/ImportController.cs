using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using apis.Models;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class ImportController : Controller
    {
        NgContext _context;
        UserManager<User> _userManager;

        public ImportController(NgContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IEnumerable<Categorie>> Get()
        {
            Newtonsoft.Json.JsonSerializer jsonSer = new Newtonsoft.Json.JsonSerializer();

            #region Users

            List<User> users;

            using (var str = new System.IO.FileStream("Json/communaute.json", System.IO.FileMode.Open))
            {
                var sr = new System.IO.StreamReader(str);
                var reader = new Newtonsoft.Json.JsonTextReader(sr);

                users = jsonSer.Deserialize<List<User>>(reader);

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

                List<Categorie> categories = jsonSer.Deserialize<List<Categorie>>(reader);

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

                List<Ingredient> ingredients = jsonSer.Deserialize<List<Ingredient>>(reader);

                foreach (var ingredient in ingredients)
                {
                    if (!_context.Categories.Any(c => c.Id == ingredient.CategoryId))
                        _context.Categories.Add(new Categorie { Id = ingredient.CategoryId.ToLower(), NameToDisplay = ingredient.CategoryId });

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

                List<Recette> recettes = jsonSer.Deserialize<List<Recette>>(reader);

                foreach (var recette in recettes)
                {
                    if (!_context.Recettes.Any(r => r.Id == recette.Id))
                    {
                        User creator = users.Where(u => u.JsonId == recette.CreatorId).FirstOrDefault();
                        recette.CreatorId = creator.Id;
                        _context.Recettes.Add(recette);
                    }

                    foreach (String ingredientId in recette.Ingredients)
                    {
                        if (!_context.Ingredients.Any(ig => ig.Id == ingredientId))
                            _context.Ingredients.Add(new Ingredient { Calories = 100, Id = ingredientId, IsAvailable = true, Name = ingredientId, Picture = "pomme-de-terre.jpg", CategoryId = "vegetable" });

                        if (!_context.IngredientsRecettes.Any(igr => igr.IngredientId == ingredientId && igr.RecetteId == recette.Id))
                            _context.IngredientsRecettes.Add(new IngredientRecette { IngredientId = ingredientId, RecetteId = recette.Id });
                    }

                    if (recette.Commentaires != null)
                        foreach (Commentaire comment in recette.Commentaires)
                        {
                            User creator = users.Where(u => u.JsonId == comment.UserId).FirstOrDefault();
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
                        

            return _context.Ingredients.Select<Ingredient, Categorie>(i => i.Category);
        }
    }
}
