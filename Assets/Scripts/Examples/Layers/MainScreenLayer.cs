using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Sklo.Examples.Layers.Views;
using Sklo.Examples.Screens;

namespace Sklo.Examples.Layers
{
    public sealed class MainScreenLayer : ScreenLayer<MainScreenLayer, MainScreenLayerView>, IScreenManagerHolderLayer 
    {
        public ScreenManager ScreenManager { get; set; }

        public void OpenSettingsScreen() => ScreenManager.ToggleMain<SettingsScreen>();
        public void ShowQuitConfirmationScreen() => ScreenManager.ShowModal<QuitConfirmationScreen>();
    }
}