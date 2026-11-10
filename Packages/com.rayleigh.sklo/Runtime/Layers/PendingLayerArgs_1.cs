using UnityEngine.Pool;

namespace Rayleigh.Sklo.Layers
{
    internal sealed class PendingLayerArgs<TArgs> : PendingLayerArgs
    {
        private ILayerWithArgs<TArgs> _layer;
        private TArgs _args;

        public static PendingLayerArgs<TArgs> Get(ILayerWithArgs<TArgs> layer, TArgs args)
        {
            var pending = GenericPool<PendingLayerArgs<TArgs>>.Get();
            pending._layer = layer;
            pending._args = args;
            return pending;
        }

        public override void Apply()
        {
            try
            {
                _layer.ApplyArgs(_args);
            }
            finally
            {
                _layer = null;
                _args = default;
                GenericPool<PendingLayerArgs<TArgs>>.Release(this);
            }
        }
    }
}
