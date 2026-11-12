using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Events;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class EventTests
    {
        [Fact]
        public async Task PublishAsync_DeliversTheEventToEverySubscribedListener()
        {
            InMemoryEventBus bus = new InMemoryEventBus();
            RecordingListener firstListener = new RecordingListener();
            RecordingListener secondListener = new RecordingListener();
            bus.Subscribe<string>(firstListener);
            bus.Subscribe<string>(secondListener);

            await bus.PublishAsync("hello");

            Assert.Equal(new[] { "hello" }, firstListener.ReceivedEvents);
            Assert.Equal(new[] { "hello" }, secondListener.ReceivedEvents);
        }

        [Fact]
        public async Task PublishAsync_IgnoresListenersOfOtherEventTypes()
        {
            InMemoryEventBus bus = new InMemoryEventBus();
            RecordingListener listener = new RecordingListener();
            bus.Subscribe<string>(listener);

            await bus.PublishAsync(42);

            Assert.Empty(listener.ReceivedEvents);
        }

        private sealed class RecordingListener : IEventListener<string>
        {
            public List<string> ReceivedEvents { get; } = new List<string>();

            public Task ListenAsync(string eventData, CancellationToken cancellationToken)
            {
                this.ReceivedEvents.Add(eventData);
                return Task.CompletedTask;
            }
        }
    }
}
