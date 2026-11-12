using System.Threading;
using System.Threading.Tasks;

namespace PennForgeTech.Core.Events
{
    public interface IEventListener<in TEvent>
    {
        Task ListenAsync(TEvent eventData, CancellationToken cancellationToken);
    }
}
