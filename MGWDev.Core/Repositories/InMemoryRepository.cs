using MGWDev.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Repositories
{
    /// <summary>
    /// Simple mock implementation of IEntityRepository
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="U">Identity type</typeparam>
    public class InMemoryRepository<T, U> : IEntityRepository<T, U> where T : class
    {
        public string OrderByField { get; set; } = "Id";
        public bool OrderAscending { get; set; } = true;
        public List<T> Entities { get; protected set; }
        protected Func<U, Func<T, bool>> IdentityQuery { get; set; }
        public InMemoryRepository(List<T> entities, Func<U, Func<T, bool>> identityQuery)
        {
            Entities = entities ?? new List<T>();
            IdentityQuery = identityQuery;
        }
        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public virtual void Commit()
        {
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        public T GetById(U Id)
        {
            return Entities.FirstOrDefault(IdentityQuery(Id));
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0)
        {
            return Entities.AsQueryable().Where(query).OrderBy(OrderByField, OrderAscending).Skip(skip).Take(top);
        }

        public void Update(T entity)
        {
        }
    }
}
