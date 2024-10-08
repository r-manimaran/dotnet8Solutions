using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Upsert(T entity);
        Task<bool> Delete(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
    
}
