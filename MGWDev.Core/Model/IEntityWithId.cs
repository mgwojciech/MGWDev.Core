using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Model
{
    public interface IEntityWithId<T>
    {
        T Id { get; set; }
    }
    public interface IEntityWithId : IEntityWithId<int>
    {

    }
}
