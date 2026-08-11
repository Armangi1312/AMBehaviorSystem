using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;

namespace AMBehaviorSystem.Editor.Node.Data
{
    public static class ControllerRegistryScanner
    {
        private const BindingFlags MemberFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static List<TInterface> ExtractItems<TInterface>(Object controller) where TInterface : class
        {
            List<TInterface> result = new();
            if (controller == null) return result;

            Type controllerType = controller.GetType();

            foreach (PropertyInfo property in controllerType.GetProperties(MemberFlags))
            {
                if (!property.CanRead || !IsRegistryType(property.PropertyType)) continue;
                CollectFromRegistry(property.GetValue(controller), result);
            }

            foreach (FieldInfo field in controllerType.GetFields(MemberFlags))
            {
                if (!IsRegistryType(field.FieldType)) continue;
                CollectFromRegistry(field.GetValue(controller), result);
            }

            return result;
        }

        private static void CollectFromRegistry<TInterface>(object registry, List<TInterface> result) where TInterface : class
        {
            if (registry == null) return;

            PropertyInfo serializedObjectsProperty = registry.GetType().GetProperty("SerializedObjects", BindingFlags.Public | BindingFlags.Instance);

            if (serializedObjectsProperty == null) return;

            if (serializedObjectsProperty.GetValue(registry) is not IEnumerable enumerable) return;

            foreach (object item in enumerable)
                if (item is TInterface typedItem) result.Add(typedItem);
        }

        private static bool IsRegistryType(Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Registry<>)) return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}
