using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        Models.NgContext _context;

        public IngredientsController(Models.NgContext context)
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
        public IEnumerable<Models.Ingredient> Get()
        {
            var ingredients = _context.Ingredients;

            return ingredients;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Models.Ingredient Get(String id)
        {
            var ingredient = _context.Ingredients.Where(r => r.Id == id).FirstOrDefault();

            return ingredient;
        }
    }
}
