using System;

namespace Rayleigh.Sklo.Layers
{
    /// <summary>
    /// Caches whether a layer type requires arguments, so the check runs once per layer type.
    /// </summary>
    internal static class LayerArgsInfo<T> where T : BaseScreenLayer
    {
        public static readonly bool RequiresArgs = GetRequiresArgs();

        private static bool GetRequiresArgs()
        {
            foreach (var type in typeof(T).GetInterfaces())
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILayerWithArgs<>))
                    return true;
            }

            return false;
        }
    }
}
