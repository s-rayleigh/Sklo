using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.ScreenManagers;
using Sklo.Examples.Data;
using Sklo.Examples.Layers.Views;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class PopupMessagesLayer : ScreenLayer<PopupMessagesLayer, PopupMessagesLayerView>,
        IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }

        public void ShowMessage(MessageData data)
        {
            if (string.IsNullOrWhiteSpace(data.Text))
            {
                Debug.Log("Cannot show an empty message.", View);
                return;
            }

            ScreenManager.ShowModal<MessageScreen, MessageData>(data);
        }

        public void ToggleBack() => ScreenManager.ToggleBack();
    }
}
