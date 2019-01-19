using MGWDev.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Utilities
{
    /// <summary>
    /// Enumerated through entities in batches of provided size
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityBuffer<T> where T : class
    {
        protected IEntityRepository<T> Repository { get; set; }
        public Expression<Func<T, bool>> Query { get; set; }
        public int MaxDegree { get; set; } = Convert.ToInt32(ConfigurationSettings.AppSettings["MaxDegree"] ?? "4");
        public int NumberOfEntities { get; private set; }
        /// <summary>
        /// Creates new instance of entity buffer
        /// </summary>
        /// <param name="repo">IEntityRepository implementation used to access the data</param>
        /// <param name="query">Query applied to repository at each batch</param>
        /// <param name="numberOfEntities">Limit of total number of entities to process</param>
        public EntityBuffer(IEntityRepository<T> repo, Expression<Func<T, bool>> query, int numberOfEntities = int.MaxValue)
        {
            Repository = repo;
            NumberOfEntities = numberOfEntities;
            Query = query;
        }
        /// <summary>
        /// Enumerates through all items that meets Query condition.
        /// </summary>
        /// <param name="action">Method to call one each entity</param>
        /// <param name="batchSize">Size of single request to data source</param>
        /// <param name="deleteAfterAction">Optional: should You delete item after process</param>
        public void Enumerate(Action<T> action, int batchSize, bool deleteAfterAction = false)
        {
            IEnumerable<T> current = GetData(batchSize);
            int skipCount = 0;
            while (current.Count() > 0)
            {
                Parallel.ForEach(current, new ParallelOptions()
                {
                    MaxDegreeOfParallelism = MaxDegree
                }, entity =>
                {
                    if (skipCount > NumberOfEntities)
                        return;
                    action(entity);
                    if (deleteAfterAction)
                        Repository.Delete(entity);
                    skipCount++;
                });
                if (deleteAfterAction)
                {
                    Repository.Commit();
                    current = GetData(batchSize);
                }
                else
                    current = GetData(batchSize, skipCount);
            }
        }
        protected virtual IEnumerable<T> GetData(int batchSize, int skip = 0)
        {
            return Repository.Query(Query, batchSize, skip);
        }
    }
}
