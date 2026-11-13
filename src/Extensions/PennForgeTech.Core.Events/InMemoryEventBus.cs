using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Events
{
    public sealed class InMemoryEventBus : IEventPublisher
    {
        private readonly Dictionary<Type, List<object>> _listenersByEventType = new Dictionary<Type, List<object>>();
        private readonly object _lock = new object();

        public void Subscribe<TEvent>(IEventListener<TEvent> listener)
        {
            Guard.AgainstNull(listener, nameof(listener));

            lock (this._lock)
            {
                if (!this._listenersByEventType.TryGetValue(typeof(TEvent), out List<object>? listeners))
                {
                    listeners = new List<object>();
                    this._listenersByEventType[typeof(TEvent)] = listeners;
                }

                listeners.Add(listener);
            }
        }

        public async Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default)
        {
            List<object> listenersSnapshot = this.GetListenersSnapshot(typeof(TEvent));

            foreach (object listener in listenersSnapshot)
            {
                IEventListener<TEvent> typedListener = (IEventListener<TEvent>)listener;
                await typedListener.ListenAsync(eventData, cancellationToken);
            }
        }

        private List<object> GetListenersSnapshot(Type eventType)
        {
            lock (this._lock)
            {
                if (this._listenersByEventType.TryGetValue(eventType, out List<object>? listeners))
                {
                    return new List<object>(listeners);
                }

                return new List<object>();
            }
        }
    }
}
