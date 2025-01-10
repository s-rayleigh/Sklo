using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class BackgroundLayer : ScreenLayer<BackgroundLayer, BackgroundLayerView>
    {
        protected override void OnHide()
        {
            base.OnHide();
            Debug.Log("background hide");
        }

        protected override void OnShow()
        {
            base.OnShow();
            Debug.Log("background show");
        }

        protected override void OnTransition()
        {
            base.OnTransition();
            Debug.Log("background transition");
        }
    }
}