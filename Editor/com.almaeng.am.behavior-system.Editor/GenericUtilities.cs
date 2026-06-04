using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMBehaviorSystem.Editor
{
    internal static class GenericUtilities
    {
        private static readonly Dictionary<Type, Type[]> inheritedTypesCache = new();
        private static Assembly[] cachedAssemblies;

        public static Type[] CollectInheritedTypes(Type baseType)
        {
            if (baseType == null)
                return Array.Empty<Type>();

            if (inheritedTypesCache.TryGetValue(baseType, out Type[] cached))
                return cached;

            List<Type> result = new();
            cachedAssemblies ??= AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in cachedAssemblies)
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch { continue; }

                foreach (Type type in types)
                {
                    if (type.IsAbstract || type.IsInterface || type.ContainsGenericParameters)
                        continue;

                    if (baseType.IsAssignableFrom(type))
                        result.Add(type);
                }
            }

            Type[] arr = result.ToArray();
            inheritedTypesCache[baseType] = arr;
            return arr;
        }

        public static Type[] ResolveElementTypes(Type type)
        {
            if (type != null && type.IsGenericType)
            {
                return type.GetGenericArguments();
            }

            return Array.Empty<Type>();
        }

        public static Type[] ResolveInheritedElementTypes(Type type)
        {
            Type current = type;

            while (current != null && current != typeof(object))
            {
                if (current.IsGenericType)
                {
                    return current.GetGenericArguments();
                }

                current = current.BaseType;
            }

            return Array.Empty<Type>();
        }

        public static bool TryResolveElementTypes(Type type, out Type[] elementTypes)
        {
            if (type != null && type.IsGenericType)
            {
                elementTypes = type.GetGenericArguments();
                return true;
            }

            elementTypes = Array.Empty<Type>();
            return false;
        }

        public static bool TryResolveInheritedElementTypes(Type type, out Type[] elementTypes)
        {
            Type current = type;

            while (current != null && current != typeof(object))
            {
                if (current.IsGenericType)
                {
                    elementTypes = current.GetGenericArguments();
                    return true;
                }

                current = current.BaseType;
            }

            elementTypes = Array.Empty<Type>();
            return false;
        }
    }
}