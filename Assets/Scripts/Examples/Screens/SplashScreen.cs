using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.Screens;
using Sklo.Examples.Layers;

namespace Sklo.Examples.Screens
{
    public sealed class SplashScreen : Screen
    {
        public override bool ExcludeFromHistory => true;

        protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor)
        {
            compositor.AddLayer<BackgroundLayer>();
            compositor.AddLayer<SplashLayer>();
        }
    }
}