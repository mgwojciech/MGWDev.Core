using MGWDev.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.EF.Repositories
{
    public class EFIEntityWithIdRepository<T> : EFEntityRepository<T, int> where T : class, IEntityWithId, new()
    {
        public EFIEntityWithIdRepository(DbContext context) : base(context, id => (ent => ent.Id == id))
        {
        }
    }
}
