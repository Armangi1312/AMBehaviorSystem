using System;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 클래스 객체를 등록하는 인터페이스입니다.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register(object item);

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <param name="type">등록 해제할 객체의 타입</param>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public bool Unregister(Type type, out object item);
    }

    /// <summary>
    /// 클래스 객체를 등록하는 인터페이스입니다.
    /// </summary>
    public interface IRegistry<TBase> : IRegistry
    {
        /// <summary>
        /// 새로운 객체를 등록합니다.
        /// </summary>
        /// /// <typeparam name="T">등록할 객체의 타입</typeparam>
        /// <param name="item">등록할 객체</param>
        /// <returns>등록이 가능한지</returns>
        public bool Register<T>(T item) where T : TBase;

        /// <summary>
        /// 특정 객체를 등록 해제합니다.
        /// </summary>
        /// <typeparam name="T">등록 해제할 객체의 타입</typeparam>
        /// <param name="item">등록 해제된 객체</param>
        /// <returns>등록 해제가 가능한지</returns>
        public bool Unregister<T>(out T item) where T : TBase;
    }
}