using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AMBehaviorSystem.Editor.Node
{
    internal static class ReflectionPathUtility
    {
        internal const int DefaultMaxDepth = 2;

        internal static readonly HashSet<Type> LeafTypes = new()
        {
            typeof(int), typeof(float), typeof(double), typeof(bool),
            typeof(string), typeof(long), typeof(short), typeof(byte),
            typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte),
            typeof(decimal), typeof(char),
            typeof(Vector2), typeof(Vector3), typeof(Vector4),
            typeof(Quaternion), typeof(Color), typeof(Color32),
            typeof(Rect), typeof(Bounds), typeof(LayerMask),
            typeof(Matrix4x4), typeof(AnimationCurve),
        };

        internal static readonly HashSet<Type> RecursionBlockedTypes = new()
        {
            typeof(Transform), typeof(GameObject),
            typeof(Component), typeof(UnityEngine.Object),
            typeof(MonoBehaviour), typeof(Behaviour),
            typeof(ScriptableObject),
        };

        internal static void CollectPaths(Type type, string prefix, List<string> result, int depth, int maxDepth = DefaultMaxDepth, bool blockUnityRefs = false)
        {
            if (depth >= maxDepth) return;

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo property in type.GetProperties(flags))
            {
                if (!property.CanRead) continue;
                if (property.GetIndexParameters().Length > 0) continue;

                string path = BuildPath(prefix, property.Name);

                if (IsLeaf(property.PropertyType))
                    result.Add(path);
                else if (!ShouldBlock(property.PropertyType, blockUnityRefs))
                    CollectPaths(property.PropertyType, path, result, depth + 1, maxDepth, blockUnityRefs);
            }

            foreach (FieldInfo field in type.GetFields(flags))
            {
                string path = BuildPath(prefix, field.Name);

                if (IsLeaf(field.FieldType))
                    result.Add(path);
                else if (!ShouldBlock(field.FieldType, blockUnityRefs))
                    CollectPaths(field.FieldType, path, result, depth + 1, maxDepth, blockUnityRefs);
            }
        }

        internal static string BuildPath(string prefix, string name)
            => string.IsNullOrEmpty(prefix) ? name : $"{prefix}.{name}";

        internal static bool IsLeaf(Type type)
            => type.IsPrimitive || type.IsEnum || LeafTypes.Contains(type);

        private static bool ShouldBlock(Type type, bool blockUnityRefs)
        {
            if (IsSystemInternal(type)) return true;
            if (!blockUnityRefs) return false;
            if (RecursionBlockedTypes.Contains(type)) return true;
            if (type.IsSubclassOf(typeof(UnityEngine.Component))) return true;
            if (type.IsSubclassOf(typeof(UnityEngine.Object))) return true;
            return false;
        }

        private static bool IsSystemInternal(Type type)
        {
            if (type.Namespace == null) return false;
            return type.Namespace.StartsWith("System.Reflection")
                || type.Namespace.StartsWith("System.Runtime")
                || type.Namespace.StartsWith("System.Collections");
        }
    }
}
