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
    public class Registry<TBase> : IRegistry<TBase>, IReadOnlyRegistry<TBase>, ISerializationCallbackReceiver
    {
        private readonly Dictionary<Type, TBase> items = new();

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <returns>등록된 클래스 객체</returns>
        public T Get<T>() where T : TBase
        {
            return (T)items[typeof(T)];
        }

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <param name="type">객체의 자료형</param>
        /// <returns>등록된 클래스 객체</returns>
        public object Get(Type type)
        {
            return items[type];
        }

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.        
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <param name="value">등록된 클래스 객체</param>
        /// <returns>반환할 수 있는지 여부</returns>
        public bool TryGet<T>(out T value) where T : TBase
        {
            if (items.TryGetValue(typeof(T), out var item))
            {
                value = (T)item;
                return true;
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
            var result = items.TryGetValue(type, out var item);
            value = item;
            return result;
        }

        /// <summary>
        /// 지정된 타입의 객체가 등록되어 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="T">확인할 타입</typeparam>
        /// <returns></returns>
        public bool Contains<T>() where T : TBase
        {
            return items.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 지정된 타입의 객체가 등록되어 있는지 확인합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        /// <returns></returns>
        public bool Contains(Type type)
        {
            return items.ContainsKey(type);
        }

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// /// <typeparam name="T">등록할 객체의 타입</typeparam>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register<T>(T item) where T : TBase
        {
            if (item == null) return false;
            return items.TryAdd(typeof(T), item);
        }

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register(object item)
        {
            if (item == null || item is not TBase baseItem) return false;
            return items.TryAdd(item.GetType(), baseItem);
        }

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <typeparam name="T">등록 해제할 객체의 타입</typeparam>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public bool Unregister<T>(out T item) where T : TBase
        {
            var result = items.Remove(typeof(T), out var removed);
            item = result ? (T)removed : default;
            return result;
        }

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <param name="type">등록 해제할 객체의 타입</param>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public bool Unregister(Type type, out object item)
        {
            var result = items.Remove(type, out var removed);
            item = removed;
            return result;
        }

        [field: SerializeReference] public List<TBase> SerializedObjects { get; protected set; } = new();

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            for(int i = 0; i < SerializedObjects.Count; i++)
            {
                Register(SerializedObjects[i]);
            }
        }
    }
}