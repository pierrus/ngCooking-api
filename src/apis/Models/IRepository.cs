using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Models
{
    public interface IRepository<TModel>
    {
        IQueryable<TModel> Get();
        void Add(TModel entity);
        void Update(TModel entity);
        void Delete(TModel entity);
    }
}
