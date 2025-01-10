using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Sklo
{
    [DisallowMultipleComponent]
    public abstract class BaseScreenManager : MonoBehaviour
    {
        protected struct BeginToggleResult : IDisposable
        {
            [MaybeNull]
            public BaseScreen ScreenToShow;
            [MaybeNull]
            public BaseScreen ScreenToHide;
            [MaybeNull]
            public HashSet<BaseScreenLayer> LayersToShow;
            [MaybeNull]
            public HashSet<BaseScreenLayer> LayersToHide;
            
            public IEnumerable<BaseScreenLayer> GetTransitionalLayers()
            {
                if (LayersToShow is null || LayersToHide is null)
                    return Enumerable.Empty<BaseScreenLayer>();
                // NOTE: we are comparing by an object reference,
                // and only transitional layers can move from the one screen to another,
                // so it is safe to assume that transitional layers are the ones that in the both
                // collections: layers to hide and layers to show.
                return LayersToHide.Intersect(LayersToShow);
            }

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

        [SerializeField]
        private Transform _viewsParent;

        private LayerManager _layerManager;
        private Stack<BaseScreen> _historyStack;
        private List<BaseScreen> _overlayScreens;
        private bool _toggleMainBegun;
        private Transform _mainParent;
        private Transform _modalParent;
        private Transform _overlayParent;
        
        [MaybeNull]
        public BaseScreen MainScreen { get; private set; }

        public bool MainDisplayed => MainScreen is not null;
        
        public BaseScreen ModalScreen { get; private set; }

        public bool ModalDisplayed => ModalScreen is not null;
        
        public IReadOnlyCollection<BaseScreen> OverlayScreens => _overlayScreens;
        
        protected virtual void Awake()
        {
            _layerManager = new LayerManager(_screenLayers);
            _historyStack = new Stack<BaseScreen>();
            _overlayScreens = new List<BaseScreen>();

            var parent = _viewsParent ?? transform;
            _mainParent = CreateScreenParentObj("MainScreen", parent);
            _modalParent = CreateScreenParentObj("ModalScreen", parent);
            _overlayParent = CreateScreenParentObj("OverlayScreens", parent);
        }

        private static RectTransform CreateScreenParentObj(string name, Transform parent)
        {
            var rectTransform = new GameObject(name).AddComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            return rectTransform;
        }

        protected bool BeginToggleMain([MaybeNull] BaseScreen screen, out BeginToggleResult result)
        {
            // TODO: text of the exception.
            if (screen is not null && screen.Displayed)
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

            MainScreen = screen;
            result.ScreenToShow = screen;
            
            if (screen is null)
                return true;
            
            result.LayersToShow = HashSetPool<BaseScreenLayer>.Get();
            screen.Compose(_layerManager, result.LayersToShow, _mainParent);
            foreach (var layer in result.LayersToShow)
                screen.Layers.Add(layer.GetType(), layer);

            return true;
        }

        protected void EndToggleMain(BeginToggleResult beginToggleResult)
        {
            _toggleMainBegun = false;
            _layerManager.ReleaseUnused();
            beginToggleResult.Dispose();
        }
        
        protected void ToggleMain([MaybeNull] Screen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            
            ((Screen)result.ScreenToHide)?.NotifyHidden();
            ((Screen)result.ScreenToShow)?.NotifyShown();
            
            var transitionalLayers = result.GetTransitionalLayers();
            if (result.LayersToHide is not null)
            {
                foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                    ((ScreenLayer)layer).NotifyHidden();
            }
            foreach (var layer in transitionalLayers)
                ((ScreenLayer)layer).NotifyTransition();
            if (result.LayersToShow is not null)
            {
                foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                    ((ScreenLayer)layer).NotifyShown();
            }
            // TODO: optimize.
            
            EndToggleMain(result);
        }

        protected bool TryPopFromHistory([NotNullWhen(true)] out BaseScreen screen) => _historyStack.TryPop(out screen);
    }
}