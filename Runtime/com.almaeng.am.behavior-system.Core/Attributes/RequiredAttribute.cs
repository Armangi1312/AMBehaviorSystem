using System;

namespace AMBehaviorSystem.Core.Attributes
{
    /// <summary>
    /// Processor 클래스에 필요한 요소를 명시하는 어트리뷰트입니다.
    /// 필요한 Setting, Context 객체는 자동으로 Controller에 추가됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RequiredAttribute : Attribute
    {
        public Type[] Types { get; }

        public RequiredAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
