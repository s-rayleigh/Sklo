using System;

namespace Sklo.Examples.Layers
{
    public sealed class SettingsLayerView : ScreenLayerView<SettingsLayerView, SettingsLayer>
    {
        public event Action BackPressed;
        
        public void Back() => BackPressed?.Invoke();
    }
}