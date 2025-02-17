using System;
using UnityEngine;

namespace Rayleigh.Sklo
{
    [DisallowMultipleComponent]
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
        
        public void ShowOverlay<T>(Action<T> init = null) where T : Screen, new()
        {
            throw new NotImplementedException();
        }
        
        private void ShowOverlay(Screen screen)
        {
            throw new NotImplementedException();
        }

        public void HideOverlay<T>() where T : BaseScreen
        {
            throw new NotImplementedException();
        }
        
        #endregion Overlay
    }
}