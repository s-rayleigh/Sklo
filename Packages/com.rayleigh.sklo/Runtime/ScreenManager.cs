using System;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo
{
    public class ScreenManager : BaseScreenManager
    {
        #region Main
        
        public T ToggleMain<T>(Action<T> init = null) where T : Screen, new()
        {
            var screen = new T();
            init?.Invoke(screen);
            ToggleMain(screen);
            return screen;
        }

        public Screen ToggleBack()
        {
            if (!TryPeekInHistory(out var baseScreen))
                return null;
            var screen = (Screen)baseScreen;
            base.ToggleMain(screen);
            return screen;
        }
        
        public void HideMain()
        {
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot hide the main screen while a modal screen is displayed.");
            if (!MainDisplayed)
                return;
            base.ToggleMain(null);
        }
        
        #endregion Main

        #region Modal

        public T ShowModal<T>(Action<T> init = null) where T : Screen, new()
        {
            var screen = new T();
            init?.Invoke(screen);
            ShowModal(screen);
            return screen;
        }

        public new void HideModal() => base.HideModal();
        
        #endregion Modal

        #region Overlay
        
        public T ShowOverlay<T>(Action<T> init = null) where T : Screen, new()
        {
            var screen = new T();
            init?.Invoke(screen);
            ShowOverlay(screen);
            return screen;
        }

        public new void HideOverlay(BaseScreen screen) => base.HideOverlay(screen);

        public void HideAllOverlay()
        {
            for (var i = OverlayScreens.Count - 1; i >= 0; i--)
                HideOverlay(OverlayScreens[i]);
        }
        
        #endregion Overlay
    }
}