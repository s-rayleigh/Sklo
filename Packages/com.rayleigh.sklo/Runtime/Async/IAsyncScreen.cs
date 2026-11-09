namespace Rayleigh.Sklo.Async
{
    internal interface IAsyncScreen<TTask>
    {
        TTask NotifyShown();
        TTask NotifyHidden();
    }
}