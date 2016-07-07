using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using apis.Models;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class CommunityController : Controller
    {
        apis.Models.NgContext _context;

        public CommunityController(NgContext context)
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
        public IEnumerable<Models.User> Get()
        {
            var users = _context.Users;

            return users.Select(u => GetUser(u)).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Models.User Get(Int32 id)
        {
            var user = _context.Users.Where(u => u.Id == id)
                .FirstOrDefault();

            return GetUser(user);
        }

        private Models.User GetUser(User user)
        {
            return new Models.User
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
