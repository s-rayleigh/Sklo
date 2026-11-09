namespace Rayleigh.Sklo.Async
{
    internal interface IAsyncLayer<TTask>
    {
        TTask NotifyShown();
        TTask NotifyTransition();
        TTask NotifyHidden();
    }
}