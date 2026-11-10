namespace Rayleigh.Sklo.Screens
{
    /// <summary>
    /// Marks a screen that is shown with arguments of type <typeparamref name="TArgs"/>.
    /// Screen manager overloads that accept arguments use it as a constraint.
    /// </summary>
    /// <remarks>
    /// Do not implement it directly, inherit <see cref="Screen{TArgs}"/> or an async screen with arguments instead.
    /// </remarks>
    /// <typeparam name="TArgs">Type of the arguments the screen expects.</typeparam>
    public interface IScreenWithArgs<TArgs>
    {
    }
}
