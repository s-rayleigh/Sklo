using Rayleigh.Sklo;

namespace Sklo.Examples.Layers.Views
{
    public sealed class SettingsLayerView : ScreenLayerView<SettingsLayerView, SettingsLayer>
    {
        public void Back() => Layer.ToggleBack();
    }
}