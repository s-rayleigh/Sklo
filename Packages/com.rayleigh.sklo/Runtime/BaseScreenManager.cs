using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Rayleigh.Sklo
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

        protected readonly struct BeginProcessResult : IDisposable
        {
            private readonly BaseScreen _screen;
            private readonly HashSet<BaseScreenLayer> _layersToProcess;

            public BaseScreen Screen => _screen;
            public HashSet<BaseScreenLayer> LayersToProcess => _layersToProcess;

            public BeginProcessResult(BaseScreen screen)
            {
                _screen = screen;
                _layersToProcess = HashSetPool<BaseScreenLayer>.Get();
            }

            public void Dispose() => HashSetPool<BaseScreenLayer>.Release(_layersToProcess);
        }
        
        [SerializeField]
        private ScreenLayerView[] _screenLayers;

        [SerializeField]
        private Transform _viewsParent;

        private LayerManager _layerManager;
        private Stack<BaseScreen> _historyStack;
        private List<BaseScreen> _overlayScreens;
        private bool _screenProcessBegun;
        private Transform _mainParent;
        private Transform _modalParent;
        private Transform _overlayParent;
        
        [MaybeNull]
        public BaseScreen MainScreen { get; private set; }

        public bool MainDisplayed => MainScreen is not null;
        
        public BaseScreen ModalScreen { get; private set; }

        public bool ModalDisplayed => ModalScreen is not null;
        
        public IReadOnlyList<BaseScreen> OverlayScreens => _overlayScreens;

        #region Events
        
        public event Action<BaseScreenLayer, DisplayType> BeforeLayerShow;
        
        public event Action<BaseScreenLayer, DisplayType> BeforeLayerHide;

        public event Action<BaseScreen, DisplayType> BeforeScreenShow;

        public event Action<BaseScreen, DisplayType> AfterScreenShow; 

        public event Action<BaseScreen, DisplayType> BeforeScreenHide;

        public event Action<BaseScreen, DisplayType> AfterScreenHide; 
        
        #endregion Events
        
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
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            return rectTransform;
        }

        private Transform GetParentObj(DisplayType displayType) => displayType switch
        {
            DisplayType.Main => _mainParent,
            DisplayType.Modal => _modalParent,
            DisplayType.Overlay => _overlayParent,
            _ => throw new ArgumentOutOfRangeException(nameof(displayType), displayType, "Specify valid display type.")
        };

        private void BeginShowScreenInternal(BaseScreen screen, HashSet<BaseScreenLayer> layersToShow,
            DisplayType displayType)
        {
            var parentObj = GetParentObj(displayType);
            if (displayType is DisplayType.Overlay)
                parentObj = CreateScreenParentObj(screen.GetType().Name, parentObj);
            
            screen.Compose(_layerManager, layersToShow, parentObj);

            foreach (var layer in layersToShow)
            {
                layer.DisplayType = displayType;
                screen.InternalLayers.Add(layer.GetType(), layer);
            }

            if (BeforeLayerShow is not null)
            {
                foreach (var layer in layersToShow)
                    BeforeLayerShow(layer, displayType);
            }
            
            BeforeScreenShow?.Invoke(screen, displayType);
        }

        private void EndProcessInternal(IDisposable beginResult)
        {
            _layerManager.ReleaseUnused();
            beginResult.Dispose();
        }

        private void BeginHideScreenInternal(BaseScreen screen, HashSet<BaseScreenLayer> layersToHide,
            DisplayType displayType)
        {
            foreach (var layer in screen.InternalLayers.Values)
            {
                // Return layers to the manager so it knows which transitional layers available for re-use.
                _layerManager.Return(layer);
                // Add the layer to the list of layers to hide.
                layersToHide.Add(layer);
            }
            screen.InternalLayers.Clear();
            screen.ParentContainer = null;

            if (BeforeLayerHide is not null)
            {
                foreach (var layer in layersToHide)
                    BeforeLayerHide(layer, displayType);
            }

            BeforeScreenHide?.Invoke(screen, displayType);
        }
        
        #region Main
        
        protected bool BeginToggleMain([MaybeNull] BaseScreen screen, out BeginToggleResult result)
        {
            if (screen is not null && screen.Displayed)
                throw new Exception("The screen is already displayed.");
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot toggle the main screen while a modal screen is displayed.");
            ThrowIfProcessBegun();

            _screenProcessBegun = true;
            result = new BeginToggleResult();
            
            if (MainDisplayed)
            {
                // Do nothing if trying to display the same screen type.
                if (MainScreen.SameAs(screen))
                {
                    _screenProcessBegun = false;
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
                
                BeginHideScreenInternal(result.ScreenToHide, result.LayersToHide, DisplayType.Main);
            }

            MainScreen = screen;
            result.ScreenToShow = screen;
            
            if (screen is null)
                return true;
            
            result.LayersToShow = HashSetPool<BaseScreenLayer>.Get();
            BeginShowScreenInternal(screen, result.LayersToShow, DisplayType.Main);
            
            return true;
        }

        protected void EndToggleMain(BeginToggleResult beginToggleResult)
        {
            if (beginToggleResult.ScreenToShow is not null)
                AfterScreenShow?.Invoke(beginToggleResult.ScreenToShow, DisplayType.Main);
            if (beginToggleResult.ScreenToHide is not null)
                AfterScreenHide?.Invoke(beginToggleResult.ScreenToHide, DisplayType.Main);
            _screenProcessBegun = false;
            EndProcessInternal(beginToggleResult);
        }

        protected void ToggleMain([MaybeNull] Screen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            
            ((Screen)result.ScreenToHide)?.NotifyHidden();
            ((Screen)result.ScreenToShow)?.NotifyShown(DisplayType.Main);
            
            // TODO: optimize.
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
            
            EndToggleMain(result);
        }

        protected bool TryPeekInHistory([NotNullWhen(true)] out BaseScreen screen) => _historyStack.TryPeek(out screen);
        
        #endregion Main
        
        #region Modal

        protected BeginProcessResult BeginShowModal(BaseScreen screen)
        {
            if (screen is null)
                throw new ArgumentNullException(nameof(screen));
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot show a modal screen while another is being displayed.");
            ThrowIfProcessBegun();

            _screenProcessBegun = true;
            ModalScreen = screen;
            
            var result = new BeginProcessResult(screen);
            BeginShowScreenInternal(screen, result.LayersToProcess, DisplayType.Modal);
            
            return result;
        }

        protected void EndShowModal(BeginProcessResult beginProcessResult)
        {
            if (beginProcessResult.Screen is not null)
                AfterScreenShow?.Invoke(beginProcessResult.Screen, DisplayType.Modal);
            _screenProcessBegun = false;
            EndProcessInternal(beginProcessResult);
        }

        protected void ShowModal([NotNull] Screen screen)
        {
            var result = BeginShowModal(screen);
            
            ((Screen)result.Screen).NotifyShown(DisplayType.Modal);
            foreach (var layer in result.LayersToProcess)
                ((ScreenLayer)layer).NotifyShown();
            
            EndShowModal(result);
        }

        protected BeginProcessResult BeginHideModal()
        {
            if (!ModalDisplayed)
                throw new InvalidOperationException("Can't hide a modal screen because it isn't currently displayed.");
            ThrowIfProcessBegun();

            _screenProcessBegun = true;
            
            var result = new BeginProcessResult(ModalScreen);
            BeginHideScreenInternal(result.Screen, result.LayersToProcess, DisplayType.Modal);
            return result;
        }

        protected void EndHideModal(BeginProcessResult beginProcessResult)
        {
            ModalScreen = null;
            if (beginProcessResult.Screen is not null)
                AfterScreenHide?.Invoke(beginProcessResult.Screen, DisplayType.Modal);
            _screenProcessBegun = false;
            EndProcessInternal(beginProcessResult);
        }

        protected void HideModal()
        {
            var result = BeginHideModal();
            
            ((Screen)result.Screen).NotifyHidden();
            foreach (var layer in result.LayersToProcess)
                ((ScreenLayer)layer).NotifyHidden();
            
            EndHideModal(result);
        }
        
        #endregion Modal
        
        #region Overlay

        protected BeginProcessResult BeginShowOverlay(BaseScreen screen)
        {
            if (screen is null)
                throw new ArgumentNullException(nameof(screen));
            if (screen.Displayed)
                throw new InvalidOperationException("Screen is already displayed.");
            
            _overlayScreens.Add(screen);
            
            var result = new BeginProcessResult(screen);
            BeginShowScreenInternal(screen, result.LayersToProcess, DisplayType.Overlay);
            
            // TODO: sorting.
            
            return result;
        }

        protected void EndShowOverlay(BeginProcessResult beginProcessResult)
        {
            if (beginProcessResult.Screen is not null)
                AfterScreenShow?.Invoke(beginProcessResult.Screen, DisplayType.Overlay);
            EndProcessInternal(beginProcessResult);
        }

        protected void ShowOverlay(BaseScreen screen)
        {
            var result = BeginShowOverlay(screen);
            
            ((Screen)result.Screen).NotifyShown(DisplayType.Overlay);
            foreach (var layer in result.LayersToProcess)
                ((ScreenLayer)layer).NotifyShown();
            
            EndShowOverlay(result);
        }
        
        protected BeginProcessResult BeginHideOverlay(BaseScreen screen)
        {
            if (screen is null)
                throw new ArgumentNullException(nameof(screen));
            if (!screen.Displayed)
                throw new InvalidOperationException("Screen is not displayed.");
            if (screen.DisplayType is not DisplayType.Overlay)
                throw new InvalidOperationException("Screen is not displayed in overlay mode.");
            var index = _overlayScreens.IndexOf(screen);
            if (index is -1)
                throw new InvalidOperationException("Screen is not controlled by this screen manager.");

            _overlayScreens.RemoveAt(index);

            var result = new BeginProcessResult(screen);
            BeginHideScreenInternal(screen, result.LayersToProcess, DisplayType.Overlay);

            return result;
        }
        
        protected void EndHideOverlay(BeginProcessResult beginProcessResult)
        {
            if (beginProcessResult.Screen is not null)
                AfterScreenHide?.Invoke(beginProcessResult.Screen, DisplayType.Overlay);
            // Destroy the parent container.
            Destroy(beginProcessResult.Screen.ParentContainer);
            EndProcessInternal(beginProcessResult);
        }

        protected void HideOverlay(BaseScreen screen)
        {
            var result = BeginHideOverlay(screen);
            
            ((Screen)result.Screen).NotifyHidden();
            foreach (var layer in result.LayersToProcess)
                ((ScreenLayer)layer).NotifyHidden();
            
            EndHideOverlay(result);
        }
        
        #endregion Overlay

        private void ThrowIfProcessBegun()
        {
            if (_screenProcessBegun)
                throw new InvalidOperationException("Cannot show a screen because another process is already begun.");
        }
    }
}