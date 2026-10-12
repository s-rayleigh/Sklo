using Rayleigh.Sklo.LayerViews;

namespace Sklo.Examples.Layers.Views
{
    public sealed class MainScreenLayerView : ScreenLayerView<MainScreenLayerView, MainScreenLayer>
    {
        public void OpenSettingsScreen() => Layer.OpenSettingsScreen();
        public void OpenQuitConfirmationScreen() => Layer.ShowQuitConfirmationScreen();
    }
}