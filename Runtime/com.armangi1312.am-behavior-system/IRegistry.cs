using System;

namespace AMBehaviorSystem
{
    /// <summary>
    /// An interface for registering class objects.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Registers a new object.
        /// </summary>
        /// <param name="item">The object to register.</param>
        /// <returns>Whether registration is possible.</returns>
        public bool Register(object item);

        /// <summary>
        /// Unregisters a specific object.
        /// </summary>
        /// <param name="type">The type of the object to unregister</param>
        /// <param name="item">The object to unregister</param>
        /// <returns>Whether unregistration is possible</returns>
        public bool Unregister(Type type, out object item);
    }

    /// <summary>
    /// An interface for registering class objects.
    /// </summary>
    public interface IRegistry<TBase> : IRegistry
    {
        /// <summary>
        /// Registers a new object.
        /// </summary>
        /// <typeparam name="T">The type of the object to register.</typeparam>
        /// <param name="item">The object to register.</param>
        /// <returns>Whether registration is possible.</returns>
        public bool Register<T>(T item) where T : TBase;

        /// <summary>
        /// Unregisters a specific object.
        /// </summary>
        /// <typeparam name="T">The type of the object to unregister.</typeparam>
        /// <param name="item">The object to unregister.</param>
        /// <returns>Whether unregistration is possible.</returns>
        public bool Unregister<T>(out T item) where T : TBase;
    }
}