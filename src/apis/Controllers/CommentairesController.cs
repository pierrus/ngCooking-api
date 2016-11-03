using apis.Data;
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
        IRepository<Commentaire> _commentairesRepository;

        public CommentairesController(IRepository<Commentaire> commentairesRepository)
        {
            _commentairesRepository = commentairesRepository;
        }

        [HttpPost]
        public void Post(Commentaire comment)
        {
            _commentairesRepository.Add(comment);
        }
    }
}
