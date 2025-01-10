using UnityEngine;

namespace Sklo.Examples.Layers
{
    public class SplashLayer : ScreenLayer<SplashLayer, SplashLayerView>
    {
        protected override void OnHide()
        {
            base.OnHide();
            Debug.Log("splash layer hide");
        }

        protected override void OnTransition()
        {
            base.OnTransition();
            Debug.Log("splash layer transition");
        }

        protected override void OnShow()
        {
            base.OnShow();
            Debug.Log("splash layer show");
        }
    }
}