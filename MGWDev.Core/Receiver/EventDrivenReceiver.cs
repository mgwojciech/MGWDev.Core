using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Receiver
{
    public class EventDrivenReceiver<T> : IEventReceiver<T>
    {
        public EventType Type { get; private set; }
        public event EventHandler<T> Handler;
        public EventDrivenReceiver(EventType type, EventHandler<T> handler = null)
        {
            Type = type;
            Handler = handler;
        }
        public void Process(T entity)
        {
            Handler?.Invoke(this, entity);
        }
    }
}
