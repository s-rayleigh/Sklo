using System;
using System.Collections.Generic;
using Rayleigh.Sklo.LayerViews;
using Sklo.Examples.Data;
using TMPro;
using UnityEngine;

namespace Sklo.Examples.Layers.Views
{
    public sealed class PopupMessagesLayerView : ScreenLayerView<PopupMessagesLayerView, PopupMessagesLayer>
    {
        [SerializeField]
        private TMP_InputField _messageField;

        [SerializeField]
        private TMP_Dropdown _typeDropdown;

        private void Awake()
        {
            _typeDropdown.ClearOptions();
            _typeDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(MessageType))));
        }

        public void ShowMessage()
        {
            var data = new MessageData
            {
                Type = (MessageType)_typeDropdown.value,
                Text = _messageField.text,
            };
            Layer.ShowMessage(data);
        }

        public void Back() => Layer.ToggleBack();
    }
}
