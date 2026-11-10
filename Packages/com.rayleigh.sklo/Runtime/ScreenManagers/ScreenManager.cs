using System;
using Rayleigh.Sklo.LayerViews.Providers;
using Rayleigh.Sklo.Screens;
using UnityEngine;
using Screen = Rayleigh.Sklo.Screens.Screen;

namespace Rayleigh.Sklo.ScreenManagers
{
    public class ScreenManager : BaseScreenManager
    {
        public ScreenManager(ILayerViewProvider layerViewProvider, RectTransform viewsParent)
            : base(layerViewProvider, viewsParent)
        {
        }

        #region Main

        public T ToggleMain<T>() where T : Screen, new()
        {
            var screen = new T();
            ToggleMain(screen);
            return screen;
        }

        public T ToggleMain<T, TArgs>(TArgs args) where T : Screen, IScreenWithArgs<TArgs>, new()
        {
            var screen = CreateScreen<T, TArgs>(args);
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

        public T ShowModal<T>() where T : Screen, new()
        {
            var screen = new T();
            ShowModal(screen);
            return screen;
        }

        public T ShowModal<T, TArgs>(TArgs args) where T : Screen, IScreenWithArgs<TArgs>, new()
        {
            var screen = CreateScreen<T, TArgs>(args);
            ShowModal(screen);
            return screen;
        }

        public new void HideModal() => base.HideModal();
        
        #endregion Modal

        #region Overlay
        
        public T ShowOverlay<T>() where T : Screen, new()
        {
            var screen = new T();
            ShowOverlay(screen);
            return screen;
        }

        public T ShowOverlay<T, TArgs>(TArgs args) where T : Screen, IScreenWithArgs<TArgs>, new()
        {
            var screen = CreateScreen<T, TArgs>(args);
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