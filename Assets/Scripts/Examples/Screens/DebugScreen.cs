using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.Screens;
using Sklo.Examples.Layers;

namespace Sklo.Examples.Screens
{
    public sealed class DebugScreen : Screen
    {
        protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor)
        {
            compositor.AddLayer<DebugScreenLayer>();
        }

        public void ToggleDebugInfo() => GetLayer<DebugScreenLayer>()?.ToggleDebugInfo();
    }
}