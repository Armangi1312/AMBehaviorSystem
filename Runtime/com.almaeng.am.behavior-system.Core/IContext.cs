namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 런타임 가변 데이터를 저장하는 부모 인터페이스입니다.
    /// 구현하는 클래스는 런타임에 필요한 데이터를 저장하는 용도로 사용됩니다.
    /// 구현하는 클래스는 `[Serializable]` 어트리뷰트를 사용하여 직렬화가 가능해야 합니다.
    /// </summary>
    public interface IContext { }
}