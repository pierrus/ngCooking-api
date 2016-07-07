using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using apis.Models;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        NgContext _context;

        public IngredientsController(NgContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/recettes
        [HttpGet]
        public IEnumerable<Ingredient> Get()
        {
            var ingredients = _context.Ingredients;

            return ingredients;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Ingredient Get(String id)
        {
            var ingredient = _context.Ingredients.Where(r => r.Id == id).FirstOrDefault();

            return ingredient;
        }
    }
}
