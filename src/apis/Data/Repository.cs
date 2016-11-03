using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Data
{
    public class Repository<TModel> : IRepository<TModel> where TModel:class
    {
        DbSet<TModel> _set = null;
        INgContext _ngContext;

        public Repository(INgContext ngContext)
        {
            _ngContext = ngContext;
            _set = ngContext.Set<TModel>();
        }

        public void Add(TModel entity)
        {
            _set.Add(entity);
            _ngContext.SaveChanges();
        }

        public void Delete(TModel entity)
        {
            _set.Remove(entity);
            _ngContext.SaveChanges();
        }

        public IQueryable<TModel> Get()
        {
            return _set.AsQueryable<TModel>();
        }

        public void Update(TModel entity)
        {
            _set.Update(entity);
            _ngContext.SaveChanges();
        }
    }
}
