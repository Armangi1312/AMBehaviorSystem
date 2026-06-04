using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 클래스 객체를 등록하는 객체입니다.
    /// </summary>
    /// <typeparam name="TBase">등록할 객체의 필터링 타입</typeparam>
    [Serializable]
    public class Registry<TBase> : IRegistry<TBase>, IReadOnlyRegistry<TBase>
    {
        [field: SerializeReference] public List<TBase> SerializedObjects { get; protected set; } = new();

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <returns>등록된 클래스 객체</returns>
        public T Get<T>() where T : TBase
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i] is T item) return item;
            }
            throw new KeyNotFoundException($"{typeof(T).Name} is not registered.");
        }

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <param name="type">객체의 자료형</param>
        /// <returns>등록된 클래스 객체</returns>
        public object Get(Type type)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() == type) return SerializedObjects[i];
            }
            throw new KeyNotFoundException($"{type.Name} is not registered.");
        }

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <param name="value">등록된 클래스 객체</param>
        /// <returns>반환할 수 있는지 여부</returns>
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
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <param name="type">객체의 자료형</param>
        /// <param name="value">등록된 클래스 객체</param>
        /// <returns>반환할 수 있는지 여부</returns>
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
        /// 지정된 타입의 객체가 등록되어 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="T">확인할 타입</typeparam>
        public bool Contains<T>() where T : TBase
        {
            return Contains(typeof(T));
        }

        /// <summary>
        /// 지정된 타입의 객체가 등록되어 있는지 확인합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        public bool Contains(Type type)
        {
            for (int i = 0; i < SerializedObjects.Count; i++)
            {
                if (SerializedObjects[i]?.GetType() == type) return true;
            }
            return false;
        }

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <typeparam name="T">등록할 객체의 타입</typeparam>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register<T>(T item) where T : TBase
        {
            if (item == null || Contains(typeof(T))) return false;
            SerializedObjects.Add(item);
            return true;
        }

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register(object item)
        {
            if (item == null || item is not TBase baseItem) return false;
            if (Contains(item.GetType())) return false;
            SerializedObjects.Add(baseItem);
            return true;
        }

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <typeparam name="T">등록 해제할 객체의 타입</typeparam>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
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
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <param name="type">등록 해제할 객체의 타입</param>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
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