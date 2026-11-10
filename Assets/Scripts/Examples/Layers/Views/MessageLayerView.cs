using Rayleigh.Sklo.LayerViews;
using Sklo.Examples.Data;
using TMPro;
using UnityEngine;

namespace Sklo.Examples.Layers.Views
{
    public sealed class MessageLayerView : ScreenLayerView<MessageLayerView, MessageScreenLayer>
    {
        [SerializeField]
        private TMP_Text _messageText;

        [Header("Colors")]
        
        [SerializeField]
        private Color _normalColor = Color.black;

        [SerializeField]
        private Color _warningColor = Color.yellow;

        [SerializeField]
        private Color _errorColor = Color.red;
        
        public void SetMessage(MessageData data)
        {
            _messageText.text = data.Text;
            _messageText.color = data.Type switch
            {
                MessageType.Warning => _warningColor,
                MessageType.Error => _errorColor,
                _ => _normalColor,
            };
        }

        public void Close() => Layer.Close();
    }
}