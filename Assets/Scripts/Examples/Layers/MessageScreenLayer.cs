using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.ScreenManagers;
using Sklo.Examples.Data;
using Sklo.Examples.Layers.Views;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class MessageScreenLayer : ScreenLayer<MessageScreenLayer, MessageLayerView>,
        ILayerWithArgs<MessageData>, IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }

        public void ApplyArgs(MessageData args) => View.SetMessage(args);
        
        public void Close()
        {
            if (DisplayType is DisplayType.Modal)
                ScreenManager.HideModal();
            else
                Debug.Log($"Cannot hide, unsupported display type {DisplayType}.", View);
        }
    }
}