using System;
using Rayleigh.Sklo;

namespace Sklo.Examples.Layers.Views
{
    public sealed class SettingsLayerView : ScreenLayerView<SettingsLayerView, SettingsLayer>
    {
        public event Action BackPressed;
        
        public void Back() => BackPressed?.Invoke();
    }
}