using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Model
{
    public interface IEntityWithIdAndTitle<T> : IEntityWithId<T>
    {
        string Title { get; set; }
    }
    public interface IEntityWithIdAndTitle : IEntityWithIdAndTitle<int>,IEntityWithId
    {
    }

}
