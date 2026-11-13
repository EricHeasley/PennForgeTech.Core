using System.Threading;
using System.Threading.Tasks;

namespace PennForgeTech.Core.Abstractions
{
    public interface IEventListener<in TEvent>
    {
        Task ListenAsync(TEvent eventData, CancellationToken cancellationToken);
    }
}
