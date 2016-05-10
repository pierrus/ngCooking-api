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
    public class CommunityController : Controller
    {
        Models.NgContext _context;

        public CommunityController(Models.NgContext context)
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
        public IEnumerable<ControllersModels.User> Get()
        {
            var users = _context.Users;

            return users.Select(u => GetUser(u)).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ControllersModels.User Get(Int32 id)
        {
            var user = _context.Users.Where(u => u.Id == id)
                .FirstOrDefault();

            return GetUser(user);
        }

        private ControllersModels.User GetUser(Models.User user)
        {
            return new ControllersModels.User
            {
                Bio = user.Bio,
                BirthYear = user.BirthYear,
                City = user.City,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Level = user.Level,
                Picture = user.Picture
            };
        }
    }
}
