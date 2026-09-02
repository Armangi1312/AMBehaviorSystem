using System;

namespace AMBehaviorSystem.Attributes
{
    /// <summary>
    /// An attribute that specifies the elements required by the Processor class.
    /// The necessary Setting and Context objects are automatically added to the Controller.
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
