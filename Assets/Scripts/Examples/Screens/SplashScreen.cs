using Rayleigh.Sklo;
using Sklo.Examples.Layers;
using UnityEngine;
using Screen = Rayleigh.Sklo.Screen;

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

        protected override void OnShow()
        {
            base.OnShow();
            Debug.Log("Splash screen shown");
        }

        protected override void OnHide()
        {
            base.OnHide();
            Debug.Log("Splash screen hidden");
        }
    }
}