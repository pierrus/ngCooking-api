using apis.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class CommentairesController
    {
        NgContext _context;

        public CommentairesController(NgContext context)
        {
            _context = context;
        }

        [HttpPost]
        public void Post(Commentaire comment)
        {
            _context.Commentaires.Add(comment);

            _context.SaveChanges();
        }
    }
}
