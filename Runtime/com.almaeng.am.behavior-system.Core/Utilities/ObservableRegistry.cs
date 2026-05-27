using System;

namespace AMBehaviorSystem.Core.Utilities
{
    /// <summary>
    /// 이벤트 기능이 있는 레지스트리입니다.
    /// </summary>
    /// <typeparam name="TBase">등록할 객체의 필터링 타입</typeparam>
    [Serializable]
    public class ObservableRegistry<TBase> : Registry<TBase>
    {
        public event Action<Type, TBase> OnRegistered;
        public event Action<Type, TBase> OnUnregistered;

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <typeparam name="T">등록할 객체의 타입</typeparam>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public new bool Register<T>(T item) where T : TBase
        {
            var result = base.Register(item);
            if (result) OnRegistered?.Invoke(typeof(T), item);
            return result;
        }

        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public new bool Register(object item)
        {
            var result = base.Register(item);
            if (result && item is TBase baseItem) OnRegistered?.Invoke(item.GetType(), baseItem);
            return result;
        }

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <typeparam name="T">등록 해제할 객체의 타입</typeparam>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public new bool Unregister<T>(out T item) where T : TBase
        {
            var result = base.Unregister(out item);
            if (result) OnUnregistered?.Invoke(typeof(T), item);
            return result;
        }

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <param name="type">등록 해제할 객체의 타입</param>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public new bool Unregister(Type type, out object item)
        {
            var result = base.Unregister(type, out item);
            if (result) OnUnregistered?.Invoke(type, (TBase)item);
            return result;
        }
    }
}
