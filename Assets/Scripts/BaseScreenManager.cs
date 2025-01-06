using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Pool;

namespace Sklo
{
    [DisallowMultipleComponent]
    public abstract class BaseScreenManager : MonoBehaviour
    {
        protected struct BeginToggleResult : IDisposable
        {
            public BaseScreen ScreenToShow;
            [MaybeNull]
            public BaseScreen ScreenToHide;
            public HashSet<BaseScreenLayer> LayersToShow;
            [MaybeNull]
            public HashSet<BaseScreenLayer> LayersToHide;
            
            public void Dispose()
            {
                if (LayersToHide is not null)
                    HashSetPool<BaseScreenLayer>.Release(LayersToHide);
                if (LayersToShow is not null)
                    HashSetPool<BaseScreenLayer>.Release(LayersToShow);
                LayersToHide = null;
                LayersToShow = null;
                ScreenToShow = null;
                ScreenToHide = null;
            }
        }
        
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

        private bool _toggleMainBegun;
        
        protected virtual void Awake()
        {
            _layerManager = new LayerManager(_screenLayers);
            _historyStack = new Stack<BaseScreen>();
            _overlayScreens = new List<BaseScreen>();
        }

        protected bool BeginToggleMain(BaseScreen screen, out BeginToggleResult result)
        {
            // TODO: text of the exception.
            if (screen.Displayed)
                throw new Exception("");
            
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot switch the main screen while a modal screen is displayed.");

            if (_toggleMainBegun)
            {
                throw new InvalidOperationException(
                    "Cannot begin toggling the main screen because toggle is already begun");
            }

            _toggleMainBegun = true;
            result = new BeginToggleResult();
            
            if (MainDisplayed)
            {
                // Do nothing if trying to display the same screen type.
                if (MainScreen.SameAs(screen))
                {
                    _toggleMainBegun = false;
                    result.Dispose();
                    return false;
                }

                // Deal with the history stack.
                if (_historyStack.Count > 0 && _historyStack.Peek().SameAs(screen))
                    // Returned to the screen we have been before.
                    _historyStack.Pop();
                else if (MainDisplayed && !MainScreen.ExcludeFromHistory)
                    // Store the screen so we can return to it by calling the corresponding method.
                    _historyStack.Push(MainScreen);
                
                result.ScreenToHide = MainScreen;
                result.LayersToHide = HashSetPool<BaseScreenLayer>.Get();
                
                foreach (var layer in MainScreen.Layers.Values)
                {
                    // Return layers to the manager so it knows which transitional layers available for re-use.
                    _layerManager.Return(layer);
                    // Add the layer to the list of layers to hide.
                    result.LayersToHide.Add(layer);
                }
                MainScreen.Layers.Clear();
            }

            result.LayersToShow = HashSetPool<BaseScreenLayer>.Get();
            var compositor = new ScreenCompositor(_layerManager, result.LayersToShow);
            screen.Compose(compositor);
            
            MainScreen = screen;
            result.ScreenToShow = screen;

            return true;
        }

        protected void EndToggleMain(BeginToggleResult beginToggleResult)
        {
            _toggleMainBegun = false;
            _layerManager.ReleaseUnused();
            beginToggleResult.Dispose();
        }
    }
}