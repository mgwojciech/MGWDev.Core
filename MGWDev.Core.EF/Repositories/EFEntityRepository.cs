﻿using MGWDev.Core.Model;
using MGWDev.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MGWDev.Core.Utilities;

namespace MGWDev.Core.EF.Repositories
{
    public class EFEntityRepository<T, U> : IEntityRepository<T, U> where T : class, IEntityWithId<U>, new()
    {
        public string OrderByField { get; set; } = "Id";
        public bool OrderAscending { get; set; } = true;
        protected DbSet<T> Set { get; set; }
        protected DbContext Context { get; set; }
        protected Func<U, Func<T, bool>> IdentityQuery { get; set; }
        public EFEntityRepository(DbContext context, Func<U, Func<T, bool>> identityQuery)
        {
            IdentityQuery = identityQuery;
            Context = context;
            Set = context.Set<T>();
        }
        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            Set.Remove(entity);
        }

        public T GetById(U Id)
        {
            return Set.FirstOrDefault(IdentityQuery(Id));
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0)
        {
            return Set.Where(query).OrderBy(OrderByField, OrderAscending).Skip(skip).Take(top);
        }

        public void Update(T entity)
        {
        }
    }
}
