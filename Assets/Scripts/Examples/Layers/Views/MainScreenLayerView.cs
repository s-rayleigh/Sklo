using System;
using Rayleigh.Sklo;

namespace Sklo.Examples.Layers.Views
{
    public sealed class MainScreenLayerView : ScreenLayerView<MainScreenLayerView, MainScreenLayer>
    {
        public event Action OpenSettingsPressed;
        public event Action OpenQuitConfirmationPressed;
        
        public void OpenSettingsScreen() => OpenSettingsPressed?.Invoke();

        public void OpenQuitConfirmationScreen() => OpenQuitConfirmationPressed?.Invoke();
    }
}