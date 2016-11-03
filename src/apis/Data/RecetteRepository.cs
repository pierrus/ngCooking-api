using apis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Data
{
    public class RecetteRepository : IRepository<Recette>
    {
        DbSet<Recette> _set = null;
        NgContext _ngContext;

        public RecetteRepository(NgContext ngContext)
        {
            _ngContext = ngContext;
            _set = ngContext.Set<Recette>();
        }

        public void Add(Recette entity)
        {
            _set.Add(entity);
            _ngContext.SaveChanges();
        }

        public void Delete(Recette entity)
        {
            _set.Remove(entity);
            _ngContext.SaveChanges();
        }

        public IQueryable<Recette> Get()
        {
            return _set.AsQueryable<Recette>()
                .Include(r => r.Commentaires)
                .ThenInclude(c => c.User)
                .Include(r => r.IngredientsRecettes)
                .ThenInclude(ir => ir.Ingredient);
        }

        public void Update(Recette entity)
        {
            _set.Update(entity);
            _ngContext.SaveChanges();
        }
    }
}
