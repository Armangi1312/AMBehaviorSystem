namespace AMBehaviorSystem
{
    /// <summary>
    /// A parent interface for storing runtime semi-immutable configuration data.
    /// Implementing classes are used to store configuration values.
    /// Implementing classes must be serializable using the '[Serializable]' attribute.
    /// </summary>
    public interface ISetting { }
}