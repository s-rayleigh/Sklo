using System;
using System.Collections.Generic;
using Rayleigh.Sklo.Layers;
using UnityEngine;

namespace Rayleigh.Sklo
{
    public sealed class ScreenCompositor<TLayer> where TLayer : BaseScreenLayer
    {
        private readonly ulong _opId;
        private readonly LayerManager _layerManager;
        private readonly HashSet<BaseScreenLayer> _layers;
        private readonly List<PendingLayerArgs> _pendingArgs;
        private readonly RectTransform _parent;

        internal ScreenCompositor(LayerManager layerManager, HashSet<BaseScreenLayer> layers,
            List<PendingLayerArgs> pendingArgs, RectTransform parent, ulong opId)
        {
            _opId = opId;
            _layerManager = layerManager;
            _layers = layers;
            _pendingArgs = pendingArgs;
            _parent = parent;
        }

        public void AddLayer<T>(object key = null) where T : TLayer, new()
        {
            // Check before getting the layer, so a view isn't created for a layer that can't be added.
            if (LayerArgsInfo<T>.RequiresArgs)
            {
                throw new InvalidOperationException(
                    $"Layer '{typeof(T).Name}' requires arguments. Add it with an overload that accepts arguments.");
            }

            var layer = _layerManager.Get<T>(key, _opId, _parent);
            _layers.Add(layer);
        }

        /// <summary>
        /// Adds a layer with arguments. The arguments are applied once the layer is initialized,
        /// before it is shown or transitioned.
        /// </summary>
        public void AddLayer<T, TArgs>(TArgs args, object key = null) where T : TLayer, ILayerWithArgs<TArgs>, new()
        {
            var layer = _layerManager.Get<T>(key, _opId, _parent);
            _layers.Add(layer);
            _pendingArgs.Add(PendingLayerArgs<TArgs>.Get((T)layer, args));
        }
    }
}
