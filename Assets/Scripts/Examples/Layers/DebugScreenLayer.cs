using System.Collections.Generic;
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
            View.UpdateList(GetScreens());
        }

        protected override void OnHide()
        {
            ScreenManager.AfterScreenShow -= OnScreenShow;
            ScreenManager.AfterScreenHide -= OnScreenHide;
            base.OnHide();
        }

        private void OnScreenShow(BaseScreen screen, DisplayType displayType) => View.UpdateList(GetScreens());

        private void OnScreenHide(BaseScreen screen, DisplayType displayType) => View.UpdateList(GetScreens());

        private IEnumerable<BaseScreen> GetScreens()
        {
            if (ScreenManager.MainScreen is not null)
                yield return ScreenManager.MainScreen;
            if (ScreenManager.ModalScreen is not null)
                yield return ScreenManager.ModalScreen;
            foreach (var overlayScreen in ScreenManager.OverlayScreens)
                yield return overlayScreen;
        }
    }
}