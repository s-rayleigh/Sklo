namespace Rayleigh.Sklo.Layers
{
    /// <summary>
    /// Layer arguments recorded by the compositor and applied once the layer is initialized.
    /// </summary>
    internal abstract class PendingLayerArgs
    {
        /// <summary>
        /// Applies the arguments to the layer and returns this instance to the pool.
        /// </summary>
        public abstract void Apply();
    }
}
