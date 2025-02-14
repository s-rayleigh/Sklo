using System.Collections.Generic;
using UnityEngine;

namespace Rayleigh.Sklo
{
    public abstract class BaseScreen<TLayer> : BaseScreen where TLayer : BaseScreenLayer
    {
        internal override void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent)
        {
            base.Compose(layerManager, layers, parent);
            OnCompose(new ScreenCompositor<TLayer>(layerManager, layers, parent));
        }

        protected abstract void OnCompose(ScreenCompositor<TLayer> compositor);
    }
}