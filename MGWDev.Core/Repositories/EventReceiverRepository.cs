using MGWDev.Core.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Repositories
{
    public class EventReceiverRepository<T, U> : IEntityRepository<T, U> where T : class
    {
        public IList<IEventReceiver<T>> Receivers { get; protected set; }
        protected IEntityRepository<T,U> BaseRepo { get; set; }
        public EventReceiverRepository(IEntityRepository<T, U> baseRepo)
        {
            Receivers = new List<IEventReceiver<T>>();
            BaseRepo = baseRepo;
        }
        public void Add(T entity)
        {
            ProcessReceivers(entity, EventType.EntityAdding);
            BaseRepo.Add(entity);
            ProcessReceivers(entity, EventType.EntityAdded);
        }

        public virtual void Commit()
        {
            BaseRepo.Commit();
        }

        public void Delete(T entity)
        {
            ProcessReceivers(entity, EventType.EntityDeleting);
            BaseRepo.Delete(entity);
            ProcessReceivers(entity, EventType.EntityDeleted);
        }

        public T GetById(U Id)
        {
            return BaseRepo.GetById(Id);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0)
        {
            return BaseRepo.Query(query,top,skip);
        }

        public void Update(T entity)
        {
            ProcessReceivers(entity, EventType.EntityUpdeting);
            BaseRepo.Update(entity);
            ProcessReceivers(entity, EventType.EntityUpdated);
        }

        protected void ProcessReceivers(T entity, EventType type)
        {
            foreach (IEventReceiver<T> receiver in Receivers.Where(r => r.Type == type))
            {
                receiver.Process(entity);
            }
        }
    }
}
