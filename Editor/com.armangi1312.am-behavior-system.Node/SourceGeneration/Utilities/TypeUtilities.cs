using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AMBehaviorSystem.Node.SourceGeneration.Utilities
{
    internal static class TypeUtilities
    {
        private static readonly Dictionary<Type, int> typePriorities = new()
        {
            [typeof(sbyte)] = 0,
            [typeof(byte)] = 0,
            [typeof(short)] = 1,
            [typeof(ushort)] = 1,
            [typeof(int)] = 2,
            [typeof(uint)] = 3,
            [typeof(long)] = 4,
            [typeof(ulong)] = 5,
            [typeof(float)] = 6,
            [typeof(double)] = 7,
            [typeof(Vector2)] = 8,
            [typeof(Vector3)] = 9,
            [typeof(Vector4)] = 10
        };

        private static readonly IReadOnlyCollection<Type> integerTypes = new HashSet<Type>()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong)
        };

        private static readonly IReadOnlyCollection<Type> floatTypes = new HashSet<Type>()
        {
            typeof(float),
            typeof(double)
        };

        private static readonly IReadOnlyCollection<Type> vectorTypes = new HashSet<Type>()
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4)
        };

        private static readonly IReadOnlyDictionary<Type, string> typeAliasMap = new Dictionary<Type, string>()
        {
            [typeof(sbyte)] = "sbyte",
            [typeof(byte)] = "byte",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
            [typeof(bool)] = "bool",
            [typeof(object)] = "object"
        };

        private const float DefaultVectorFillValue = 0f;

        public static Type GetCastingType(params Type[] types)
        {
            if (types == null || types.Length == 0)
                return typeof(object);

            Type highestType = null;
            int highestPriority = -1;

            foreach (Type type in types)
            {
                if (!typePriorities.TryGetValue(type, out int priority))
                    return typeof(object);

                if (priority > highestPriority)
                {
                    highestPriority = priority;
                    highestType = type;
                }
            }

            return highestType;
        }

        public static bool IsInteger(this Type type) => integerTypes.Contains(type);
        public static bool IsFloat(this Type type) => floatTypes.Contains(type);

        public static bool IsVector(this Type type) => vectorTypes.Contains(type);

        public static bool IsScalar(this Type type) => IsInteger(type) || IsFloat(type);
        public static bool IsNumeric(this Type type) => IsScalar(type) || IsVector(type);

        public static bool CanCast(this Type original, Type castedType)
        {
            if (original.IsAssignableFrom(castedType)) return true;
            if(original.IsNumeric() && castedType.IsNumeric()) return true;

            return false;
        }

        public static string GetCastedExpression(this Type originalType, Type castedType, string expression)
        {
            bool isOriginalTypeVector = originalType.IsVector();
            bool isCastedTypeVector = castedType.IsVector();

            if (!isOriginalTypeVector && isCastedTypeVector)
                return BuildScalarToVectorExpression(castedType, expression);

            if (isOriginalTypeVector && !isCastedTypeVector)
                return $"({GetTypeAlias(castedType)})({expression}.x)";

            if (isOriginalTypeVector && isCastedTypeVector)
                return BuildVectorToVectorExpression(originalType, castedType, expression);

            return $"({GetTypeAlias(castedType)})({expression})";
        }

        private static string BuildScalarToVectorExpression(Type castedType, string expression)
        {
            string scalarExpression = $"({expression})";

            if (castedType == typeof(Vector2))
                return $"new Vector2({scalarExpression}, {DefaultVectorFillValue}f)";

            if (castedType == typeof(Vector3))
                return $"new Vector3({scalarExpression}, {DefaultVectorFillValue}f, {DefaultVectorFillValue}f)";

            return $"new Vector4({scalarExpression}, {DefaultVectorFillValue}f, {DefaultVectorFillValue}f, {DefaultVectorFillValue}f)";
        }

        private static string BuildVectorToVectorExpression(Type originalType, Type castedType, string expression)
        {
            int originalComponentCount = GetVectorComponentCount(originalType);
            int castedComponentCount = GetVectorComponentCount(castedType);

            if (castedComponentCount <= originalComponentCount)
                return $"({GetTypeAlias(castedType)})({expression})";

            List<string> components = new(castedComponentCount);
            string[] axisNames = { "x", "y", "z", "w" };

            for (int i = 0; i < originalComponentCount; i++)
                components.Add($"{expression}.{axisNames[i]}");

            for (int i = originalComponentCount; i < castedComponentCount; i++)
                components.Add($"{DefaultVectorFillValue}f");

            return $"new {GetTypeAlias(castedType)}({string.Join(", ", components)})";
        }

        private static int GetVectorComponentCount(Type type)
        {
            if (type == typeof(Vector2)) return 2;
            if (type == typeof(Vector3)) return 3;
            return 4;
        }

        public static string GetTypeAlias(this Type type)
        {
            if (typeAliasMap.TryGetValue(type, out string name))
                return name;

            return type.Name;
        }
    }
}