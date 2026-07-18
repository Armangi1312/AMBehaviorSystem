namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// A parent interface for storing runtime-variable data.
    /// Implementing classes are used to store data required at runtime.
    /// Implementing classes must be serializable using the `[Serializable]` attribute.
    /// </summary>
    public interface IContext { }
}