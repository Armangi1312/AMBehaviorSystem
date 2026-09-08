using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace AMBehaviorSystem.Editor.Utilities
{
    public static class GenericUtilities
    {
        private static readonly Dictionary<Type, Type[]> inheritedTypesCache = new();

        public static Type[] CollectInheritedTypes(Type baseType)
        {
            if(baseType == null)
                return Array.Empty<Type>();

            if(inheritedTypesCache.TryGetValue(baseType, out Type[] cached))
                return cached;

            Type[] array = TypeCache.GetTypesDerivedFrom(baseType)
                                    .Where(type => !type.IsAbstract && !type.IsInterface && !type.ContainsGenericParameters)
                                    .ToArray();

            inheritedTypesCache[baseType] = array;
            return array;
        }

        public static Type[] GetElementTypes(Type type)
        {
            if(type != null && type.IsGenericType)
                return type.GetGenericArguments();

            return Array.Empty<Type>();
        }

        public static Type[] GetInheritedElementTypes(Type type)
        {
            Type current = type;

            while(current != null && current != typeof(object))
            {
                if(current.IsGenericType)
                    return current.GetGenericArguments();

                current = current.BaseType;
            }

            return Array.Empty<Type>();
        }

        public static bool TryGetElementTypes(Type type, out Type[] elementTypes)
        {
            if(type != null && type.IsGenericType)
            {
                elementTypes = type.GetGenericArguments();
                return true;
            }

            elementTypes = Array.Empty<Type>();
            return false;
        }

        public static bool TryGetInheritedElementTypes(Type type, out Type[] elementTypes)
        {
            Type current = type;

            while(current != null && current != typeof(object))
            {
                if(current.IsGenericType)
                {
                    elementTypes = current.GetGenericArguments();
                    return true;
                }

                current = current.BaseType;
            }

            elementTypes = Array.Empty<Type>();
            return false;
        }

        public static List<Type> CollectTypeChain(Type type)
        {
            List<Type> chain = new();
            Type current = type;

            while(current != null && current != typeof(object))
            {
                chain.Insert(0, current);
                current = current.BaseType;
            }

            return chain;
        }

        public static Type GetInheritedGenericType(Type type, int index)
        {
            Type[] types = GetInheritedElementTypes(type);

            return types.Length > index ? types[index] : null;
        }

        public static bool TryGetInheritedGenericType(Type type, int index, out Type outType)
        {
            outType = null;

            if(!TryGetInheritedElementTypes(type, out Type[] types))
                return false;

            if(types.Length <= index)
                return false;

            outType = types[index];
            return outType != null;
        }
    }
}