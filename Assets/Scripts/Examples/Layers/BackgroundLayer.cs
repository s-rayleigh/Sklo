using Rayleigh.Sklo;
using Sklo.Examples.Layers.Views;

namespace Sklo.Examples.Layers
{
    public sealed class BackgroundLayer : ScreenLayer<BackgroundLayer, BackgroundLayerView>
    {
        protected override void OnShow()
        {
            base.OnShow();
            View.UpdateColor(DisplayType);
        }
    }
}