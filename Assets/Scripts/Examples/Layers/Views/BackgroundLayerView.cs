using Rayleigh.Sklo;
using Rayleigh.Sklo.LayerViews;
using UnityEngine;
using UnityEngine.UI;

namespace Sklo.Examples.Layers.Views
{
    public sealed class BackgroundLayerView : ScreenLayerView<BackgroundLayerView, BackgroundLayer>
    {
        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private Color _mainBgColor = Color.black;

        [SerializeField]
        private Color _modalBgColor = Color.gray;

        [SerializeField]
        private Color _overlayBgColor = Color.clear;

        public void UpdateColor(DisplayType displayType) => _backgroundImage.color = displayType switch
        {
            DisplayType.Main => _mainBgColor,
            DisplayType.Modal => _modalBgColor,
            DisplayType.Overlay => _overlayBgColor,
            _ => _backgroundImage.color
        };
    }
}