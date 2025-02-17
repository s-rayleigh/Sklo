using Rayleigh.Sklo;

namespace Sklo.Examples.Layers.Views
{
    public sealed class QuitConfirmationLayerView : ScreenLayerView<QuitConfirmationLayerView, QuitConfirmationLayer>
    {
        public void ConfirmQuit() => Layer.CloseApp();

        public void CancelQuit() => Layer.HideScreen();
    }
}