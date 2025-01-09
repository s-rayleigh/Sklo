using System;
using System.Linq;
using UnityEngine;

namespace Sklo
{
    [DisallowMultipleComponent]
    public class ScreenManager : BaseScreenManager
    {
        #region Main
        
        public T ToggleMain<T>() where T : Screen, new()
        {
            var screen = new T();
            ToggleMain(screen);
            return screen;
        }

        public Screen ToggleBack()
        {
            if (!TryPopFromHistory(out var baseScreen))
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

        public void ShowModal<T>() where T : Screen, new()
        {
            throw new NotImplementedException();
        }
        
        private void ShowModal(Screen screen)
        {
            if (ModalDisplayed)
            {
                throw new InvalidOperationException(
                    "Cannot show the modal screen while other modal screen is already displayed.");
            }
            
            throw new NotImplementedException();
        }
        
        #endregion Modal

        #region Overlay
        
        public void ShowOverlay<T>() where T : Screen, new()
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