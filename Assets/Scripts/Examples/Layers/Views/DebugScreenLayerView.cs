using System.Collections.Generic;
using Rayleigh.Sklo.LayerViews;
using Rayleigh.Sklo.Screens;
using Sklo.Examples.Elements;
using UnityEngine;

namespace Sklo.Examples.Layers.Views
{
    [DisallowMultipleComponent]
    public sealed class DebugScreenLayerView : ScreenLayerView<DebugScreenLayerView, DebugScreenLayer>
    {
        [SerializeField]
        private Transform _contentContainer;
        
        [SerializeField]
        private DebugScreenItem _debugScreenItemPrefab;

        [SerializeField]
        private Transform _listContainer;

        private readonly List<DebugScreenItem> _items = new();
        
        public void UpdateList(IEnumerable<BaseScreen> screens)
        {
            for (var i = 0; i < _items.Count; i++)
                Destroy(_items[i].gameObject);
            _items.Clear();
            foreach (var screen in screens)
            {
                var item = Instantiate(_debugScreenItemPrefab, _listContainer, false);
                item.Init(screen);
                _items.Add(item);
            }
        }

        public void ToggleContent() => _contentContainer.gameObject.SetActive(!_contentContainer.gameObject.activeSelf);
    }
}