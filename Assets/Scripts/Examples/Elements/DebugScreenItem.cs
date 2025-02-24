using System.Linq;
using Rayleigh.Sklo;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sklo.Examples.Elements
{
    public sealed class DebugScreenItem : MonoBehaviour
    {
        [FormerlySerializedAs("_screenModeText")] [SerializeField]
        private TMP_Text _screenTypeText;
        
        [SerializeField]
        private TMP_Text _screenNameText;

        [SerializeField]
        private TMP_Text _screenListText;

        public void Init(BaseScreen screen)
        {
            _screenNameText.text = screen.GetType().Name;
            _screenTypeText.text = screen.DisplayType.ToString().First().ToString().ToUpperInvariant();
            var layerNames = screen.Layers.Select(l => l.GetType().Name).ToArray();
            var layerListText = string.Empty;
            for (var i = 0; i < layerNames.Length; i++)
            {
                layerListText += $"— {layerNames[i]}";
                if (i != layerNames.Length - 1)
                    layerListText += "\n";
            }
            _screenListText.text = layerListText;
        }
    }
}