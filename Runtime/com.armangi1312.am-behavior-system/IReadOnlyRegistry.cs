using System;

namespace AMBehaviorSystem
{
    /// <summary>
    /// Interface for registering class objects.
    /// (Read-only)
    /// </summary>
    public interface IReadOnlyRegistry
    {
        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <param name="type">The data type of the object</param>
        /// <returns>The registered class object</returns>
        object Get(Type type);

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <param name="type">The data type of the object</param>
        /// <param name="value">The registered class object</param>
        /// <returns>Whether the object can be returned</returns>
        bool TryGet(Type type, out object value);

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <param name="type">The data type of the object</param>
        /// <returns>The registered class object</returns>
        bool Contains(Type type);
    }

    /// <summary>
    /// Interface for registering class objects.
    /// (Read-only)
    /// </summary>
    /// <typeparam name="TBase">The base type for registered objects.</typeparam>
    public interface IReadOnlyRegistry<TBase> : IReadOnlyRegistry
    {
        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>The registered class object</returns>
        T Get<T>() where T : TBase;

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="value">The registered class object</param>
        /// <returns>Whether the object can be returned</returns>
        bool TryGet<T>(out T value) where T : TBase;

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="value">The registered class object</param>
        /// <returns>Whether the object can be returned</returns>
        bool Contains<T>() where T : TBase;
    }
}