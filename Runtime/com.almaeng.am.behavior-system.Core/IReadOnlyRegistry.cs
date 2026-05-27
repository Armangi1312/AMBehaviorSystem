using System;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 클래스 객체를 등록하는 인터페이스입니다.
    /// (읽기 전용)
    /// </summary>
    public interface IReadOnlyRegistry
    {
        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <param name="type">객체의 자료형</param>
        /// <returns>등록된 클래스 객체</returns>
        object Get(Type type);

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <param name="type">객체의 자료형</param>
        /// <param name="value">등록된 클래스 객체</param>
        /// <returns>반환할 수 있는지 여부</returns>
        bool TryGet(Type type, out object value);
    }

    /// <summary>
    /// 클래스 객체를 등록하는 인터페이스입니다.
    /// (읽기 전용)
    /// </summary>
    public interface IReadOnlyRegistry<TBase> : IReadOnlyRegistry
    {
        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <returns>등록된 클래스 객체</returns>
        T Get<T>() where T : TBase;

        /// <summary>
        /// 등록된 클래스 객체를 반환합니다.        
        /// </summary>
        /// <typeparam name="T">객체의 타입</typeparam>
        /// <param name="value">등록된 클래스 객체</param>
        /// <returns>반환할 수 있는지 여부</returns>
        bool TryGet<T>(out T value) where T : TBase;
    }
}