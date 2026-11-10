using System.Threading;

namespace Rayleigh.Sklo.Async
{
    internal interface IAsyncLayer<TTask>
    {
        TTask NotifyShown(CancellationToken cancellationToken);
        TTask NotifyTransition(CancellationToken cancellationToken);
        TTask NotifyHidden(CancellationToken cancellationToken);
    }
}