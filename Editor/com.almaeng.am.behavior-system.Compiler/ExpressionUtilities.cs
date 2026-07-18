using Codice.CM.SEIDInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AMBehaviorSystem.Compiler
{
    internal static class ExpressionUtilities
    {
        private static readonly Dictionary<string, Type> ResolvedTypeCache = new();

        public static Type ParseType(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            if (ResolvedTypeCache.TryGetValue(name, out Type cachedType))
                return cachedType;

            Type resolvedType = ResolveType(name);

            ResolvedTypeCache.Add(name, resolvedType);

            return resolvedType;
        }

        private static Type ResolveType(string name)
        {
            Type directType = Type.GetType(name);
            if (directType != null) return directType;

            return FindTypeInLoadedAssemblies(name);
        }

        private static Type FindTypeInLoadedAssemblies(string name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type foundType = assembly.GetTypes()
                    .FirstOrDefault(type => type.FullName == name);

                if (foundType != null) return foundType;
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<Type, int> NumericPriorityMap = new Dictionary<Type, int>()
        {
            [typeof(sbyte)]     = 0,
            [typeof(byte)]      = 0,
            [typeof(short)]     = 1,
            [typeof(ushort)]    = 1,
            [typeof(int)]       = 2,
            [typeof(uint)]      = 3,
            [typeof(long)]      = 4,
            [typeof(ulong)]     = 5,
            [typeof(float)]     = 6,
            [typeof(double)]    = 7,
            [typeof(Vector2)]   = 8,
            [typeof(Vector3)]   = 9,
            [typeof(Vector4)]   = 10
        };


        private static readonly IReadOnlyList<Type> NumberList = new List<Type>()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4)
        };

        private static readonly IReadOnlyList<Type> PrimitiveNumberList = new List<Type>()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double)
        };

        private static readonly IReadOnlyList<Type> VectorList = new List<Type>()
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4)
        };

        public static string GetCastedExpression(Type originalType, Type castedType, string expression)
        {
            if(PrimitiveNumberList.Contains(originalType) && VectorList.Contains(castedType))
                return $"new {castedType.Name}({expression}, 0f)";

            if (PrimitiveNumberList.Contains(castedType) && VectorList.Contains(originalType))
                return $"({castedType.Name})({expression}.x)";

            return $"({castedType.Name})({expression})";
        }

        public static Type GetCastingType(params Type[] types)
        {
            if (types == null || types.Length == 0)
                return typeof(object);

            List<Type> nonNumericTypes = new();
            foreach (Type type in types)
            {
                if (!NumericPriorityMap.ContainsKey(type))
                    nonNumericTypes.Add(type);
            }

            if (nonNumericTypes.Count > 0)
            {
                Type mostDerivedType = types[0];

                for (int i = 1; i < types.Length; i++)
                {
                    Type currentType = types[i];
                    if (mostDerivedType.IsAssignableFrom(currentType))
                    {
                        mostDerivedType = currentType;
                        continue;
                    }

                    if (!currentType.IsAssignableFrom(mostDerivedType))
                        return typeof(object);
                }

                return mostDerivedType;
            }

            int highestPriority = -1;
            Type highestType = typeof(object);

            foreach (Type type in types)
            {
                int priority = NumericPriorityMap[type];
                if (priority <= highestPriority) continue;

                highestPriority = priority;
                highestType = type;
            }
            return highestType;
        }

        public static string FormatTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return "(null)";

            int spaceIndex = fullTypeName.IndexOf(' ');
            if (spaceIndex >= 0)
                fullTypeName = fullTypeName[(spaceIndex + 1)..];

            int dotIndex = fullTypeName.LastIndexOf('.');
            return dotIndex >= 0 ? fullTypeName[(dotIndex + 1)..] : fullTypeName;
        }

        public static string ToFieldName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return typeName;
            return char.ToLower(typeName[0]) + typeName[1..];
        }

        public static Type ResolveTypeFromName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].Name == typeName)
                        return types[j];
                }
            }

            return null;
        }

        public static Type ResolvePathType(Type rootType, string path)
        {
            if (rootType == null || string.IsNullOrEmpty(path)) return typeof(object);

            string[] segments = path.Split('.');
            Type current = rootType;

            for (int i = 0; i < segments.Length; i++)
            {
                FieldInfo field = current.GetField(segments[i],
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (field != null)
                {
                    current = field.FieldType;
                    continue;
                }

                PropertyInfo property = current.GetProperty(segments[i],
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (property != null)
                {
                    current = property.PropertyType;
                    continue;
                }

                return typeof(object);
            }

            return current;
        }

        public static Type ResolveComponentPathType(string type, string path)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(path)) return typeof(object);

            Type componentType = ResolveTypeFromName(type);
            if (componentType == null) return typeof(object);

            return ResolvePathType(componentType, path);
        }
    }
}
