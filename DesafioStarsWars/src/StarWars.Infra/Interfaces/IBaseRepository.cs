using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Infra.Interfaces
{
    public interface IBaseRepository<T> : IDisposable
        where T : class, new()
    {
        T Add(T model);
        Task<T> AddAsync(T model);
        
        T Find(Expression<Func<T, bool>> filter);
        Task<T> FindAsync(Expression<Func<T, bool>> filter);

        IEnumerable<T> All();
        IEnumerable<T> All(Expression<Func<T, bool>> filter);
        IEnumerable<T> AllOrderBy<Tkey>(Expression<Func<T, Tkey>> orderBy);
        Task<IList<T>> AllAsync();
        Task<IList<T>> AllAsync(Expression<Func<T, bool>> filter);
        Task<IList<T>> AllOrderByAsync<Tkey>(Expression<Func<T, Tkey>> orderBy);

        IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null);
        Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null);

        bool Delete(Expression<Func<T, bool>> filter);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter);

        ObjectId CreateObjectId(string value);
    }
}
