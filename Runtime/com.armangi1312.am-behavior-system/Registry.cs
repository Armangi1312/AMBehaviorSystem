using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem
{
    /// <summary>
    /// An object that registers class objects.
    /// </summary>
    /// <typeparam name="TBase">The filtering type for the objects to be registered.</typeparam>
    [Serializable]
    public class Registry<TBase> : IRegistry<TBase>, IReadOnlyRegistry<TBase>
    {
        [field: SerializeReference] public List<TBase> SerializedObjects { get; protected set; } = new();

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>The registered class object</returns>
        public T Get<T>() where T : TBase
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i] is T item) return item;
            }
            throw new KeyNotFoundException($"{typeof(T).Name} is not registered.");
        }

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <returns>The registered class object</returns>
        public object Get(Type type)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() == type) return SerializedObjects[i];
            }
            throw new KeyNotFoundException($"{type.Name} is not registered.");
        }

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="value">The registered class object</param>
        /// <returns>Whether the object can be returned</returns>
        public bool TryGet<T>(out T value) where T : TBase
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i] is T item)
                {
                    value = item;
                    return true;
                }
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Returns the registered class object.
        /// </summary>
        /// <param name="type">The data type of the object</param>
        /// <param name="value">The registered class object</param>
        /// <returns>Whether the object can be returned</returns>
        public bool TryGet(Type type, out object value)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() == type)
                {
                    value = SerializedObjects[i];
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Checks whether an object of the specified type is registered.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        public bool Contains<T>() where T : TBase
        {
            return Contains(typeof(T));
        }

        /// <summary>
        /// Checks whether an object of the specified type is registered.
        /// </summary>
        /// <param name="type">The type to check</param>
        public bool Contains(Type type)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() == type) return true;
            }
            return false;
        }

        /// <summary>
        /// Registers a new object.
        /// </summary>
        /// <typeparam name="T">The type of the object to register.</typeparam>
        /// <param name="item">The object to register.</param>
        /// <returns>Whether registration is possible.</returns>
        public bool Register<T>(T item) where T : TBase
        {
            if (item == null || Contains(typeof(T))) return false;
            SerializedObjects.Add(item);
            return true;
        }

        /// <summary>
        /// Registers a new object.
        /// </summary>
        /// <param name="item">The object to register.</param>
        /// <returns>Whether registration is possible.</returns>
        public bool Register(object item)
        {
            if (item == null || item is not TBase baseItem) return false;
            if (Contains(item.GetType())) return false;
            SerializedObjects.Add(baseItem);
            return true;
        }

        /// <summary>
        /// Unregisters a specific object.
        /// </summary>
        /// <typeparam name="T">The type of the object to unregister.</typeparam>
        /// <param name="item">The object to unregister.</param>
        /// <returns>Whether unregistration is possible.</returns>
        public bool Unregister<T>(out T item) where T : TBase
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i] is not T typed) continue;
                item = typed;
                SerializedObjects.RemoveAt(i);
                return true;
            }
            item = default;
            return false;
        }

        /// <summary>
        /// Unregisters a specific object.
        /// </summary>
        /// <typeparam name="T">The type of the object to unregister.</typeparam>
        /// <param name="item">The object to unregister.</param>
        /// <returns>Whether unregistration is possible.</returns>
        public bool Unregister(Type type, out object item)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() != type) continue;
                item = SerializedObjects[i];
                SerializedObjects.RemoveAt(i);
                return true;
            }
            item = null;
            return false;
        }
    }
}