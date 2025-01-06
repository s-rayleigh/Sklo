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

        private void ToggleMain(Screen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            ((Screen)result.ScreenToHide)?.NotifyHidden();
            ((Screen)result.ScreenToShow).NotifyShown();
            if (result.LayersToHide is null)
            {
                foreach (var layer in result.LayersToShow)
                    ((ScreenLayer)layer).NotifyShown();
            }
            else
            {
                // NOTE: we are comparing by an object reference,
                // and only transitional layers can move from the one screen to another,
                // so it is safe to assume that transitional layers are the ones that in the both
                // collections: layers to hide and layers to show.
                var transitionalLayers = result.LayersToHide.Intersect(result.LayersToShow);
                foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                    ((ScreenLayer)layer).NotifyHidden();
                foreach (var layer in transitionalLayers)
                    ((ScreenLayer)layer).NotifyTransition();
                foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                    ((ScreenLayer)layer).NotifyShown();
                // TODO: optimize.
            }

            EndToggleMain(result);
        }

        public void ToggleBack()
        {
            throw new NotImplementedException();
        }
        
        public void HideMain()
        {
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot hide the main screen while a modal screen is displayed.");
            throw new NotImplementedException();
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