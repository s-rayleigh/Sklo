using Rayleigh.Sklo.LayerViews.Providers;
using UnityEngine;

namespace Rayleigh.Sklo.ScreenManagers.Hosts
{
    public class ScreenManagerHost : ScreenManagerHost<ScreenManager>
    {
        protected override ScreenManager CreateManager(ILayerViewProvider viewProvider, RectTransform viewsParent) =>
            new(viewProvider, viewsParent);
    }
}