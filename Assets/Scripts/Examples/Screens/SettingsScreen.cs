using Rayleigh.Sklo;
using Sklo.Examples.Layers;

namespace Sklo.Examples.Screens
{
    public sealed class SettingsScreen : Screen
    {
        protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor)
        {
            compositor.AddLayer<BackgroundLayer>();
            compositor.AddLayer<SettingsLayer>();
        }
    }
}