namespace Rayleigh.Sklo.Layers
{
    /// <summary>
    /// Implemented by a layer that is added to a screen with arguments of type <typeparamref name="TArgs"/>.
    /// Compositor overloads that accept arguments use it as a constraint.
    /// </summary>
    /// <remarks>
    /// A layer that implements this interface must be added with arguments.
    /// Avoid implementing it for two argument types where one converts to the other
    /// (e.g. a base type and its derived type): when both match, the runtime picks one of them.
    /// </remarks>
    /// <typeparam name="TArgs">Type of the arguments the layer expects.</typeparam>
    public interface ILayerWithArgs<in TArgs>
    {
        /// <summary>
        /// Called by the screen manager to pass the arguments the screen added the layer with.
        /// Do not call it yourself.
        /// </summary>
        /// <remarks>
        /// Called after the layer is initialized (its view is created, its display type is set
        /// and <c>BeforeLayerShow</c> handlers are invoked), and before the layer is shown or transitioned.
        /// A transitional layer receives the arguments of the next screen again before its transition.
        /// </remarks>
        /// <param name="args">Arguments the screen added the layer with.</param>
        void ApplyArgs(TArgs args);
    }
}
