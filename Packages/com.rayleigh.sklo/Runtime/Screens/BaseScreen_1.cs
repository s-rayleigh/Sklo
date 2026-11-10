using System.Collections.Generic;
using Rayleigh.Sklo.Layers;
using UnityEngine;

namespace Rayleigh.Sklo.Screens
{
    public abstract class BaseScreen<TLayer> : BaseScreen where TLayer : BaseScreenLayer
    {
        internal override void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers,
            List<PendingLayerArgs> pendingLayerArgs, RectTransform parent, ulong opId)
        {
            base.Compose(layerManager, layers, pendingLayerArgs, parent, opId);
            OnCompose(new ScreenCompositor<TLayer>(layerManager, layers, pendingLayerArgs, parent, opId));
        }

        protected abstract void OnCompose(ScreenCompositor<TLayer> compositor);
    }
}