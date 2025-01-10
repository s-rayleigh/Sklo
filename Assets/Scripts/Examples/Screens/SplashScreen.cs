using Sklo.Examples.Layers;
using UnityEngine;

namespace Sklo.Examples.Screens
{
    public sealed class SplashScreen : Screen
    {
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