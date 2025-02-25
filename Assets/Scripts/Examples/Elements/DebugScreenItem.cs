using System.Linq;
using Rayleigh.Sklo;
using TMPro;
using UnityEngine;

namespace Sklo.Examples.Elements
{
    public sealed class DebugScreenItem : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _screenTypeText;
        
        [SerializeField]
        private TMP_Text _screenNameText;

        [SerializeField]
        private TMP_Text _screenListText;

        [SerializeField]
        private Color _mainScreenColor = Color.black;

        [SerializeField]
        private Color _modalScreenColor = Color.black;

        [SerializeField]
        private Color _overlayScreenColor = Color.black;

        public void Init(BaseScreen screen)
        {
            _screenNameText.text = screen.GetType().Name;
            _screenTypeText.text = screen.DisplayType.ToString();
            _screenTypeText.color = screen.DisplayType switch
            {
                DisplayType.Main => _mainScreenColor,
                DisplayType.Modal => _modalScreenColor,
                DisplayType.Overlay => _overlayScreenColor,
                _ => Color.black
            };
            var layerNames = screen.Layers.Select(l => l.GetType().Name).ToArray();
            var layerListText = string.Empty;
            for (var i = 0; i < layerNames.Length; i++)
            {
                layerListText += $"– {layerNames[i]}";
                if (i != layerNames.Length - 1)
                    layerListText += "\n";
            }
            _screenListText.text = layerListText;
        }
    }
}