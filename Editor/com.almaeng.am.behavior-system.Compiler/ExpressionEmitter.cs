//using AMBehaviorSystem.Node;
//using AMBehaviorSystem.Node.Data;
//using AMBehaviorSystem.Node.Math.Advanced;
//using AMBehaviorSystem.Node.Math.Basic;
//using AMBehaviorSystem.Node.Constants;
//using GraphProcessor;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;
//using UnityEngine;

//namespace AMBehaviorSystem.Compiler
//{
//    internal static class ExpressionEmitter
//    {
//        private delegate (string Expression, Type PortType) NodeEmitter(EmitContext context, NodeGraph graph, BaseNode node);

//        private static readonly Dictionary<Type, NodeEmitter> Factory = new()
//        {
//            [typeof(BooleanNode)] = static (_, _, n) =>
//            {
//                BooleanNode node = (BooleanNode)n;
//                return (node.Field.ToString().ToLower(), typeof(bool));
//            },
//            [typeof(IntegerNode)] = static (_, _, n) =>
//            {
//                IntegerNode node = (IntegerNode)n;
//                return ($"{node.Field}", typeof(int));
//            },
//            [typeof(FloatNode)] = static (_, _, n) =>
//            {
//                FloatNode node = (FloatNode)n;
//                return ($"{F(node.Field)}f", typeof(float));
//            },
//            [typeof(DoubleNode)] = static (_, _, n) =>
//            {
//                DoubleNode node = (DoubleNode)n;
//                return ($"{D(node.Field)}d", typeof(double));
//            },
//            [typeof(Vector2Node)] = static (_, _, n) =>
//            {
//                Vector2Node node = (Vector2Node)n;
//                return ($"new Vector2({F(node.Field.x)}f, {F(node.Field.y)}f)", typeof(UnityEngine.Vector2));
//            },
//            [typeof(Vector3Node)] = static (_, _, n) =>
//            {
//                Vector3Node node = (Vector3Node)n;
//                return ($"new Vector3({F(node.Field.x)}f, {F(node.Field.y)}f, {F(node.Field.z)}f)", typeof(UnityEngine.Vector3));
//            },
//            [typeof(Vector4Node)] = static (_, _, n) =>
//            {
//                Vector4Node node = (Vector4Node)n;
//                return ($"new Vector4({F(node.Field.x)}f, {F(node.Field.y)}f, {F(node.Field.z)}f, {F(node.Field.w)}f)", typeof(UnityEngine.Vector4));
//            },
//            [typeof(AddNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "(# + #)", nameof(AddNode.A), nameof(AddNode.B));
//            },
//            [typeof(SubtractNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "(# - #)", nameof(SubtractNode.A), nameof(SubtractNode.B));
//            },
//            [typeof(MultiplyNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "(# * #)", nameof(MultiplyNode.A), nameof(MultiplyNode.B));
//            },
//            [typeof(DivideNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "(# / #)", nameof(DivideNode.A), nameof(DivideNode.B));
//            },
//            [typeof(PowerNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Pow(#, #)", nameof(PowerNode.A), nameof(PowerNode.B));
//            },
//            [typeof(SquareRootNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Sqrt(#)", nameof(SquareRootNode.In));
//            },
//            [typeof(AbsNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Abs(#)", nameof(AbsNode.In));
//            },
//            [typeof(SinNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Sin(#)", nameof(SinNode.In));
//            },
//            [typeof(CosNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Cos(#)", nameof(CosNode.In));
//            },
//            [typeof(TanNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Tan(#)", nameof(TanNode.In));
//            },
//            [typeof(Atan2Node)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Atan2(#, #)", nameof(Atan2Node.Y), nameof(Atan2Node.X));
//            },
//            [typeof(CeilNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Ceil(#)", nameof(CeilNode.In));
//            },
//            [typeof(FloorNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Floor(#)", nameof(FloorNode.In));
//            },
//            [typeof(RoundNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Round(#)", nameof(RoundNode.In));
//            },
//            [typeof(MinNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Min(#, #)", nameof(MinNode.A), nameof(MinNode.B));
//            },
//            [typeof(MaxNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Max(#, #)", nameof(MaxNode.A), nameof(MaxNode.B));
//            },
//            [typeof(ModuloNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Modulo(#, #)", nameof(ModuloNode.A), nameof(ModuloNode.B));
//            },
//            [typeof(ClampNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Clamp(#, #, #)", nameof(ClampNode.In), nameof(ClampNode.Min), nameof(ClampNode.Max));
//            },
//            [typeof(RemapNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Remap(#, #, #, #, #)", nameof(RemapNode.Value), nameof(RemapNode.FromMin), nameof(RemapNode.FromMax), nameof(RemapNode.ToMin), nameof(RemapNode.ToMax));
//            },
//            [typeof(RandomNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Random(#, #)", nameof(RandomNode.Min), nameof(RandomNode.Max));
//            },
//            [typeof(ComparisonNode)] = static (context, graph, n) =>
//            {
//                ComparisonNode node = (ComparisonNode)n;
//                string op = node.ComparisonType switch
//                {
//                    ComparisonType.Equal => "==",
//                    ComparisonType.NotEqual => "!=",
//                    ComparisonType.Less => "<",
//                    ComparisonType.LessOrEqual => "<=",
//                    ComparisonType.Greater => ">",
//                    ComparisonType.GreaterOrEqual => ">=",
//                    _ => "=="
//                };
//                return Emit(context, graph, n, typeof(bool), $"(# {op} #)", nameof(ComparisonNode.A), nameof(ComparisonNode.B));
//            },
//            [typeof(LogicNode)] = static (context, graph, n) =>
//            {
//                LogicNode node = (LogicNode)n;
//                string op = node.LogicType switch
//                {
//                    LogicType.And => "&&",
//                    LogicType.Or => "||",
//                    LogicType.Xor => "^",
//                    _ => "&&"
//                };
//                return Emit(context, graph, n, typeof(bool), $"(# {op} #)", nameof(LogicNode.A), nameof(LogicNode.B));
//            },
//            [typeof(NotNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, typeof(bool), "(!#)", nameof(NotNode.In));
//            },
//            [typeof(DistanceNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, typeof(float), "MathUtilities.Distance(#, #)", nameof(DistanceNode.A), nameof(DistanceNode.B));
//            },
//            [typeof(DotNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Dot(#, #)", nameof(DotNode.A), nameof(DotNode.B));
//            },
//            [typeof(MagnitudeNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Magnitude(#)", nameof(MagnitudeNode.In));
//            },
//            [typeof(NormalizeNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Normalize(#)", nameof(NormalizeNode.In));
//            },
//            [typeof(LerpNode)] = static (context, graph, n) =>
//            {
//                return Emit(context, graph, n, "MathUtilities.Lerp(#, #, #)", nameof(LerpNode.A), nameof(LerpNode.B), nameof(LerpNode.T));
//            },
//            [typeof(SplitNode)] = static (context, graph, n) =>
//            {
//                SplitNode node = (SplitNode)n;
//                (string varName, Type portType) = EmitInputExpression(context, graph, node, nameof(SplitNode.In));
//                return (varName, portType);
//            },

//            [typeof(ContextDataNode)] = static (_, _, n) =>
//            {
//                ContextDataNode node = (ContextDataNode)n;
//                string typeName = FormatTypeName(node.Type);
//                string fieldName = ToFieldName(typeName);
//                Type rootType = ResolveTypeFromName(typeName);
//                Type portType = ResolvePathType(rootType, node.Path);
//                return ($"{fieldName}.{node.Path}", portType);
//            },
//            [typeof(SettingDataNode)] = static (_, _, n) =>
//            {
//                SettingDataNode node = (SettingDataNode)n;
//                string typeName = FormatTypeName(node.Type);
//                string fieldName = ToFieldName(typeName);
//                Type rootType = ResolveTypeFromName(typeName);
//                Type portType = ResolvePathType(rootType, node.Path);
//                return ($"{fieldName}.{node.Path}", portType);
//            },
//            [typeof(ComponentDataNode)] = static (_, _, n) =>
//            {
//                ComponentDataNode node = (ComponentDataNode)n;
//                string guid = node.GUID[..8].Replace("-", "");
//                string[] segments = node.Path.Split('.');
//                string componentTypeName = segments.Length >= 2 ? segments[0] : "Transform";
//                string componentFieldName = $"{char.ToLower(componentTypeName[0])}{componentTypeName[1..]}{guid}";
//                string memberPath = segments.Length >= 2 ? string.Join('.', segments[1..]) : node.Path;
//                Type portType = ResolveComponentPathType(node.Target?.Value, node.Path);
//                return ($"{componentFieldName}.{memberPath}", portType);
//            },
//        };

//        public static (string Expression, Type PortType) EmitExpression(EmitContext context, NodeGraph graph, BaseNode node)
//        {
//            if (Factory.TryGetValue(node.GetType(), out NodeEmitter emitter))
//                return emitter(context, graph, node);

//            return ("default", typeof(object));
//        }

//        public static (string Expression, Type PortType) EmitInputExpression(EmitContext context, NodeGraph graph, BaseNode node, string portFieldName)
//        {
//            (BaseNode sourceNode, string sourcePort) = GraphTraversal.GetInputNodeWithPort(graph, node, portFieldName);
//            if (sourceNode == null)
//                return ("default", typeof(object));

//            string varName = context.GetOrEmit(graph, sourceNode, sourcePort, out Type portType);
//            return (varName, portType);
//        }

//        public static (string Expression, Type PortType) Emit(EmitContext context, NodeGraph graph, BaseNode node, string template, params string[] ports)
//        {
//            string[] args = new string[ports.Length];
//            Type[] portTypes = new Type[ports.Length];

//            for (int i = 0; i < ports.Length; i++)
//            {
//                (args[i], portTypes[i]) = EmitInputExpression(context, graph, node, ports[i]);
//            }

//            Type resultType = ResolveResultType(portTypes);

//            for (int i = 0; i < args.Length; i++)
//            {
//                if (portTypes[i] != resultType && IsNumericPrimitive(portTypes[i]) && IsNumericPrimitive(resultType))
//                    args[i] = $"({ToCSharpTypeName(resultType)}){args[i]}";
//            }

//            return (Format(template, args), resultType);
//        }

//        public static (string Expression, Type PortType) Emit(EmitContext context, NodeGraph graph, BaseNode node, Type resultType, string template, params string[] ports)
//        {
//            string[] args = new string[ports.Length];
//            Type[] portTypes = new Type[ports.Length];

//            for (int i = 0; i < ports.Length; i++)
//                (args[i], portTypes[i]) = EmitInputExpression(context, graph, node, ports[i]);

//            Type dominantArgType = ResolveResultType(portTypes);

//            for (int i = 0; i < args.Length; i++)
//            {
//                if (portTypes[i] == dominantArgType) continue;

//                if (IsVectorPrimitive(portTypes[i]) && IsVectorPrimitive(dominantArgType))
//                    args[i] = $"({ToCSharpTypeName(dominantArgType)}){args[i]}";
//                else if (IsNumericPrimitive(portTypes[i]) && IsNumericPrimitive(dominantArgType))
//                    args[i] = $"({ToCSharpTypeName(dominantArgType)}){args[i]}";
//            }

//            return (Format(template, args), resultType);
//        }

//        public static string FormatTypeName(string fullTypeName)
//        {
//            if (string.IsNullOrEmpty(fullTypeName)) return "(null)";

//            int spaceIndex = fullTypeName.IndexOf(' ');
//            if (spaceIndex >= 0)
//                fullTypeName = fullTypeName[(spaceIndex + 1)..];

//            int dotIndex = fullTypeName.LastIndexOf('.');
//            return dotIndex >= 0 ? fullTypeName[(dotIndex + 1)..] : fullTypeName;
//        }

//        public static string ToFieldName(string typeName)
//        {
//            if (string.IsNullOrEmpty(typeName)) return typeName;
//            return char.ToLower(typeName[0]) + typeName[1..];
//        }

//        private static readonly Type[] VectorPrecedence =
//        {
//            typeof(Vector4),
//            typeof(Vector3),
//            typeof(Vector2),
//        };

//        public static Type ResolveResultType(Type[] types)
//        {
//            for (int i = 0; i < types.Length; i++)
//            {
//                Type type = types[i];
//                if (!IsNumericPrimitive(type) && !IsVectorPrimitive(type))
//                    return type;
//            }

//            for (int i = 0; i < VectorPrecedence.Length; i++)
//            {
//                for (int j = 0; j < types.Length; j++)
//                {
//                    if (types[j] == VectorPrecedence[i])
//                        return VectorPrecedence[i];
//                }
//            }

//            for (int i = 0; i < types.Length; i++)
//            {
//                if (types[i] == typeof(double)) return typeof(double);
//            }

//            for (int i = 0; i < types.Length; i++)
//            {
//                if (types[i] == typeof(float)) return typeof(float);
//            }

//            return typeof(int);
//        }

//        private static bool IsNumericPrimitive(Type type)
//        {
//            return type == typeof(int) || type == typeof(float) || type == typeof(double);
//        }

//        public static bool IsVectorPrimitive(Type type)
//        {
//            return type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4);
//        }

//        public static string ToCSharpTypeName(Type type)
//        {
//            if (type == typeof(bool)) return "bool";
//            if (type == typeof(int)) return "int";
//            if (type == typeof(float)) return "float";
//            if (type == typeof(double)) return "double";
//            if (type == typeof(Vector2)) return "Vector2";
//            if (type == typeof(Vector3)) return "Vector3";
//            if (type == typeof(Vector4)) return "Vector4";
//            return type.Name;
//        }

//        private static Type ResolveTypeFromName(string typeName)
//        {
//            if (string.IsNullOrEmpty(typeName)) return null;

//            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            for (int i = 0; i < assemblies.Length; i++)
//            {
//                Type[] types = assemblies[i].GetTypes();
//                for (int j = 0; j < types.Length; j++)
//                {
//                    if (types[j].Name == typeName)
//                        return types[j];
//                }
//            }

//            return null;
//        }

//        private static Type ResolvePathType(Type rootType, string path)
//        {
//            if (rootType == null || string.IsNullOrEmpty(path)) return typeof(object);

//            string[] segments = path.Split('.');
//            Type current = rootType;

//            for (int i = 0; i < segments.Length; i++)
//            {
//                System.Reflection.FieldInfo field = current.GetField(segments[i],
//                    System.Reflection.BindingFlags.Public |
//                    System.Reflection.BindingFlags.NonPublic |
//                    System.Reflection.BindingFlags.Instance);

//                if (field != null)
//                {
//                    current = field.FieldType;
//                    continue;
//                }

//                System.Reflection.PropertyInfo property = current.GetProperty(segments[i],
//                    System.Reflection.BindingFlags.Public |
//                    System.Reflection.BindingFlags.NonPublic |
//                    System.Reflection.BindingFlags.Instance);

//                if (property != null)
//                {
//                    current = property.PropertyType;
//                    continue;
//                }

//                return typeof(object);
//            }

//            return current;
//        }

//        private static Type ResolveComponentPathType(UnityEngine.Object target, string path)
//        {
//            if (target == null || string.IsNullOrEmpty(path)) return typeof(object);

//            string[] segments = path.Split('.');

//            if (target is GameObject && segments.Length >= 2)
//            {
//                Type componentType = ResolveTypeFromName(segments[0]);
//                if (componentType == null) return typeof(object);

//                return ResolvePathType(componentType, string.Join('.', segments[1..]));
//            }

//            return ResolvePathType(target.GetType(), path);
//        }

//        private static string Format(string template, params string[] args)
//        {
//            StringBuilder builder = new();
//            int argIndex = 0;

//            for (int i = 0; i < template.Length; i++)
//            {
//                if (template[i] == '#' && argIndex < args.Length)
//                {
//                    builder.Append(args[argIndex++]);
//                    continue;
//                }

//                builder.Append(template[i]);
//            }

//            return builder.ToString();
//        }

//        private static string F(float value) => value.ToString("0.0############", CultureInfo.InvariantCulture);
//        private static string D(double value) => value.ToString("0.0########################", CultureInfo.InvariantCulture);
//    }
//}