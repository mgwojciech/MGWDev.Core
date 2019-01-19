using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Repositories
{
    /// <summary>
    /// Abstraction for data access layer
    /// </summary>
    /// <typeparam name="T">Returned type</typeparam>
    /// <typeparam name="U">Identity type</typeparam>
    public interface IEntityRepository<T, U> where T : class
    {
        T GetById(U Id);
        IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Commit();
    }
    public interface IEntityRepository<T> : IEntityRepository<T,int> where T : class
    {

    }
}
