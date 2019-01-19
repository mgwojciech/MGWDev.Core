using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Receiver
{
    public interface IEventReceiver<T>
    {
        EventType Type { get; }
        void Process(T entity);
    }
}
