using System.Threading;
using System.Threading.Tasks;

namespace PennForgeTech.Core.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default);
    }
}
