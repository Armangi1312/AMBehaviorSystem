using AMBehaviorSystem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AMBehaviorSystem.Core.Utilities
{
    public static class ProcessorDependencyValidator
    {
        private static readonly Dictionary<Type, (Type[] Context, Type[] Setting, Type[] Processor)> cache = new();

        public static (Type[] Context, Type[] Setting, Type[] Processor) GetRequiredTypes(Type processorType)
        {
            if (cache.TryGetValue(processorType, out var cached))
                return cached;

            List<RequiredAttribute> attributes = new(processorType.GetCustomAttributes<RequiredAttribute>(true));

            HashSet<Type> settingList = new();
            HashSet<Type> contextList = new();
            HashSet<Type> processorList = new();

            foreach (RequiredAttribute attribute in attributes)
            {
                foreach (Type type in attribute.Types)
                {
                    bool isAssignableFromSetting = typeof(ISetting).IsAssignableFrom(type) && type != typeof(ISetting);
                    bool isAssignableFromContext = typeof(IContext).IsAssignableFrom(type) && type != typeof(IContext);
                    bool isAssignableFromProcessor = typeof(Processor).IsAssignableFrom(type) && type != typeof(Processor);

                    bool isValid = isAssignableFromSetting
                                 ^ isAssignableFromContext
                                 ^ isAssignableFromProcessor;

                    if (!isValid)
                        throw new InvalidOperationException($"{type.Name}은(는) IContext, ISetting, 또는 Processor의 하위 유형이 아닙니다.");

                    if (isAssignableFromContext)
                        contextList.Add(type);
                    else if (isAssignableFromSetting)
                        settingList.Add(type);
                    else if (isAssignableFromProcessor)
                        processorList.Add(type);
                }
            }

            var result = (contextList.ToArray(), settingList.ToArray(), processorList.ToArray());
            cache[processorType] = result;
            return result;
        }
    }
}