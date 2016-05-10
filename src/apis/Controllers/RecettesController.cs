using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class RecettesController : Controller
    {
        Models.NgContext _context;

        public RecettesController(Models.NgContext context)
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
        public IEnumerable<Models.Recette> Get()
        {
            var recettes = _context.Recettes
                .Include(r => r.Commentaires)
                .Include(r => r.IngredientsRecettes)
                .ThenInclude(ir => ir.Ingredient);            

            return recettes;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Models.Recette Get(String id)
        {
            var recette = _context.Recettes.Where(r => r.Id == id)
                .Include(r => r.Commentaires)
                .Include(r => r.IngredientsRecettes)
                .FirstOrDefault();

            return recette;
        }

        [HttpPost]
        public void Post()
        {

        }
    }
}
