namespace Rayleigh.Sklo.Screens
{
    internal interface IScreenArgsReceiver<in TArgs> : IScreenArgsReceiver
    {
        void SetArgs(TArgs args);
    }
}
