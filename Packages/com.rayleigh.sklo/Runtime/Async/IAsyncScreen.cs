using System.Threading;

namespace Rayleigh.Sklo.Async
{
    internal interface IAsyncScreen<TTask>
    {
        TTask NotifyShown(CancellationToken cancellationToken);
        TTask NotifyHidden(CancellationToken cancellationToken);
    }
}