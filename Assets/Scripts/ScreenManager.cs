using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sklo
{
    [DisallowMultipleComponent]
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField]
        private ScreenLayerView[] _screenLayers;

        private LayerManager _layerManager;
        private Stack<BaseScreen> _historyStack;
        private List<BaseScreen> _overlayScreens;
        
        public BaseScreen MainScreen { get; private set; }

        public bool MainDisplayed => MainScreen is not null;
        
        public BaseScreen ModalScreen { get; private set; }

        public bool ModalDisplayed => ModalScreen is not null;

        public IReadOnlyCollection<BaseScreen> OverlayScreens => _overlayScreens;
        
        private void Awake()
        {
            _layerManager = new LayerManager();
            _historyStack = new Stack<BaseScreen>();
            _overlayScreens = new List<BaseScreen>();
        }
        
        #region Main
        
        public T ToggleMain<T>() where T : Screen, new()
        {
            var screen = new T();
            ToggleMain(screen);
            return screen;
        }
        
        public TScreen ToggleMain<TScreen, TParameter>(TParameter parameter)
            where TScreen : ParametricScreen<TParameter>, new()
            where TParameter : unmanaged
        {
            var screen = new TScreen();
            screen.SetParameter(parameter);
            ToggleMain(screen);
            return screen;
        }

        private void ToggleMain(BaseScreen screen)
        {
            // TODO
            if (screen.Displayed)
                throw new Exception();
            
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot switch the main screen while a modal screen is displayed.");

            if (MainDisplayed)
            {
                // Do nothing if trying to display the same screen type.
                if (MainScreen.GetType() == screen.GetType())
                    return;
                
                // Deal with the history stack.
                if(_historyStack.Count > 0 && _historyStack.Peek().GetType() == screen.GetType())
                    // Returned to the screen we have been before.
                    _historyStack.Pop();
                else if(MainDisplayed && !MainScreen.ExcludeFromHistory)
                    // Store the screen so we can return to it by calling the corresponding method.
                    _historyStack.Push(MainScreen);
                
                // TODO: recycle layers of the current screen and put it in the history.
                
                MainScreen.NotifyHidden();
            }
            
            var compositor = new ScreenCompositor(_layerManager);
            // TODO: prevent re-creation of existing layers (pool and check already shown).
            screen.Compose(compositor);
            
            MainScreen = screen;
            screen.NotifyShown();
            throw new NotImplementedException();
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

        public void ShowModal<TScreen, TParameter>(TParameter parameter)
            where TScreen : ParametricScreen<TParameter>, new()
            where TParameter : unmanaged
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

        public void ShowOverlay<TScreen, TParameter>(TParameter parameter)
            where TScreen : ParametricScreen<TParameter>, new()
            where TParameter : unmanaged
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