using Rayleigh.Sklo;
using Sklo.Examples.Layers.Views;

namespace Sklo.Examples.Layers
{
    public sealed class DebugScreenLayer : ScreenLayer<DebugScreenLayer, DebugScreenLayerView>,
        IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }

        protected override void OnShow()
        {
            base.OnShow();
            ScreenManager.AfterScreenShow += OnScreenShow;
            ScreenManager.AfterScreenHide += OnScreenHide;
            View.UpdateList(ScreenManager.DisplayedScreens);
        }

        protected override void OnHide()
        {
            ScreenManager.AfterScreenShow -= OnScreenShow;
            ScreenManager.AfterScreenHide -= OnScreenHide;
            base.OnHide();
        }

        private void OnScreenShow(BaseScreen screen, DisplayType displayType) =>
            View.UpdateList(ScreenManager.DisplayedScreens);

        private void OnScreenHide(BaseScreen screen, DisplayType displayType) =>
            View.UpdateList(ScreenManager.DisplayedScreens);

        public void ToggleDebugInfo() => View.ToggleContent();
    }
}