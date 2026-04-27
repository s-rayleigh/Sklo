using Rayleigh.Sklo;
using Sklo.Examples.Layers;

namespace Sklo.Examples.Screens
{
    public sealed class TestOverlayScreen : Screen
    {
        protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor)
        {
            compositor.AddLayer<BackgroundLayer>();
            compositor.AddLayer<TestOverlayLayer>();
        }
    }
}