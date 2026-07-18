using AMBehaviorSystem.Node;
using AMBehaviorSystem.Node.Constants;
using AMBehaviorSystem.Node.Data;
using AMBehaviorSystem.Node.Math.Advanced;
using AMBehaviorSystem.Node.Math.Basic;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

namespace AMBehaviorSystem.Compiler
{
    internal static class ExpressionEmitter
    {
        public delegate (string Expression, Type Type) NodeEmitter(EmitContext context, NodeGraph graph, BaseNode node);

        public static readonly Dictionary<Type, NodeEmitter> Factory = new()
        {
            //상수
            [typeof(BooleanNode)] = static (_, _, n) =>
            {
                BooleanNode node = (BooleanNode)n;
                return ($"({(node.Field ? "true" : "false")})", typeof(bool));
            },
            [typeof(DoubleNode)] = static (_, _, n) =>
            {
                DoubleNode node = (DoubleNode)n;
                return ($"({D(node.Field)})", typeof(double));
            },
            [typeof(FloatNode)] = static (_, _, n) =>
            {
                FloatNode node = (FloatNode)n;
                return ($"({F(node.Field)}f)", typeof(float));
            },
            [typeof(IntegerNode)] = static (_, _, n) =>
            {
                IntegerNode node = (IntegerNode)n;
                return ($"({node.Field.ToString(CultureInfo.InvariantCulture)})", typeof(int));
            },
            [typeof(Vector2Node)] = static (_, _, n) =>
            {
                Vector2Node node = (Vector2Node)n;
                return ($"new({F(node.Field.x)}f, {F(node.Field.y)}f)", typeof(Vector2));
            },
            [typeof(Vector3Node)] = static (_, _, n) =>
            {
                Vector3Node node = (Vector3Node)n;
                return ($"new({F(node.Field.x)}f, {F(node.Field.y)}f, {F(node.Field.z)}f)", typeof(Vector3));
            },
            [typeof(Vector4Node)] = static (_, _, n) =>
            {
                Vector4Node node = (Vector4Node)n;
                return ($"new({F(node.Field.x)}f, {F(node.Field.y)}f, {F(node.Field.z)}f, {F(node.Field.w)}f)", typeof(Vector4));
            },
            //기본 수학
            [typeof(AddNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "(# + #)", nameof(AddNode.A), nameof(AddNode.B));
            },
            [typeof(SubtractNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "(# - #)", nameof(SubtractNode.A), nameof(SubtractNode.B));
            },
            [typeof(MultiplyNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "(# * #)", nameof(MultiplyNode.A), nameof(MultiplyNode.B));
            },
            [typeof(DivideNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "(# / #)", nameof(DivideNode.A), nameof(DivideNode.B));
            },
            [typeof(PowerNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Pow(#, #)", nameof(PowerNode.A), nameof(PowerNode.B));
            },
            [typeof(SquareRootNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Sqrt(#)", nameof(SquareRootNode.In));
            },

            //고급 수학
            [typeof(AbsNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Abs(#)", nameof(AbsNode.In));
            },
            [typeof(SinNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Sin(#)", nameof(SinNode.In));
            },
            [typeof(CosNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Cos(#)", nameof(CosNode.In));
            },
            [typeof(TanNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Tan(#)", nameof(TanNode.In));
            },
            [typeof(Atan2Node)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Atan2(#, #)", nameof(Atan2Node.Y), nameof(Atan2Node.X));
            },
            [typeof(CeilNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Ceil(#)", nameof(CeilNode.In));
            },
            [typeof(FloorNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Floor(#)", nameof(FloorNode.In));
            },
            [typeof(RoundNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Round(#)", nameof(RoundNode.In));
            },
            [typeof(MinNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Min(#, #)", nameof(MinNode.A), nameof(MinNode.B));
            },
            [typeof(MaxNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Max(#, #)", nameof(MaxNode.A), nameof(MaxNode.B));
            },
            [typeof(ModuloNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Modulo(#, #)", nameof(ModuloNode.A), nameof(ModuloNode.B));
            },
            [typeof(ClampNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Clamp(#, #, #)", nameof(ClampNode.In), nameof(ClampNode.Min), nameof(ClampNode.Max));
            },
            [typeof(RemapNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Remap(#, #, #, #, #)", nameof(RemapNode.Value), nameof(RemapNode.FromMin), nameof(RemapNode.FromMax), nameof(RemapNode.ToMin), nameof(RemapNode.ToMax));
            },
            [typeof(RandomNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Random(#, #)", nameof(RandomNode.Min), nameof(RandomNode.Max));
            },
            [typeof(ComparisonNode)] = static (context, graph, n) =>
            {
                ComparisonNode node = (ComparisonNode)n;
                string op = node.Comparison switch
                {
                    ComparisonNode.ComparisonType.Equal => "==",
                    ComparisonNode.ComparisonType.NotEqual => "!=",
                    ComparisonNode.ComparisonType.Less => "<",
                    ComparisonNode.ComparisonType.LessOrEqual => "<=",
                    ComparisonNode.ComparisonType.Greater => ">",
                    ComparisonNode.ComparisonType.GreaterOrEqual => ">=",
                    _ => "=="
                };
                return Emit(context, graph, n, typeof(bool), $"(# {op} #)", nameof(ComparisonNode.A), nameof(ComparisonNode.B));
            },
            [typeof(LogicNode)] = static (context, graph, n) =>
            {
                LogicNode node = (LogicNode)n;
                string op = node.Logic switch
                {
                    LogicNode.LogicType.And => "&&",
                    LogicNode.LogicType.Or => "||",
                    LogicNode.LogicType.Xor => "^",
                    _ => "&&"
                };
                return Emit(context, graph, n, typeof(bool), $"(# {op} #)", nameof(LogicNode.A), nameof(LogicNode.B));
            },
            [typeof(NotNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, typeof(bool), "(!#)", nameof(NotNode.In));
            },
            [typeof(DistanceNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, typeof(float), "AMBehaviorSystem.Core.Utilities.MathUtilities.Distance(#, #)", nameof(DistanceNode.A), nameof(DistanceNode.B));
            },
            [typeof(DotNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Dot(#, #)", nameof(DotNode.A), nameof(DotNode.B));
            },
            [typeof(MagnitudeNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, typeof(float), "AMBehaviorSystem.Core.Utilities.MathUtilities.Magnitude(#)", nameof(MagnitudeNode.In));
            },
            [typeof(NormalizeNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Normalize(#)", nameof(NormalizeNode.In));
            },
            [typeof(LerpNode)] = static (context, graph, n) =>
            {
                return Emit(context, graph, n, "AMBehaviorSystem.Core.Utilities.MathUtilities.Lerp(#, #, #)", nameof(LerpNode.A), nameof(LerpNode.B), nameof(LerpNode.T));
            },
            [typeof(SplitNode)] = static (context, graph, n) =>
            {
                SplitNode node = (SplitNode)n;
                (string varName, Type portType) = EmitInputExpression(context, graph, node, nameof(SplitNode.In));
                return (varName, portType);
            },

            [typeof(ContextDataNode)] = static (_, _, n) =>
            {
                ContextDataNode node = (ContextDataNode)n;

                string typeName = ExpressionUtilities.FormatTypeName(node.Type);
                string fieldName = ExpressionUtilities.ToFieldName(typeName);

                Type rootType = ExpressionUtilities.ResolveTypeFromName(typeName);
                Type portType = ExpressionUtilities.ResolvePathType(rootType, node.Path);

                return ($"{fieldName}.{node.Path}", portType);
            },
            [typeof(SettingDataNode)] = static (_, _, n) =>
            {
                SettingDataNode node = (SettingDataNode)n;

                string typeName = ExpressionUtilities.FormatTypeName(node.Type);
                string fieldName = ExpressionUtilities.ToFieldName(typeName);

                Type rootType = ExpressionUtilities.ResolveTypeFromName(typeName);
                Type portType = ExpressionUtilities.ResolvePathType(rootType, node.Path);

                return ($"{fieldName}.{node.Path}", portType);
            },
            [typeof(ComponentDataNode)] = static (_, _, n) =>
            {
                ComponentDataNode node = (ComponentDataNode)n;

                string guid = node.GUID[..8].Replace("-", "");
                string componentFieldName = $"{char.ToLower(node.Type[0])}{node.Type[1..]}{guid}";

                Type portType = ExpressionUtilities.ResolveComponentPathType(node.Type, node.Path);

                return ($"{componentFieldName}.{node.Path}", portType);
            },
        };

        public static (string Expression, Type PortType) EmitExpression(EmitContext context, NodeGraph graph, BaseNode node)
        {
            if (Factory.TryGetValue(node.GetType(), out NodeEmitter emitter))
                return emitter(context, graph, node);

            return ("default", typeof(object));
        }

        public static (string Expression, Type PortType) EmitInputExpression(EmitContext context, NodeGraph graph, BaseNode node, string portFieldName)
        {
            (BaseNode sourceNode, string sourcePort) = GraphTraversal.GetInputNodeWithPort(graph, node, portFieldName);
            if (sourceNode == null)
                return ("default", typeof(object));

            (Type type, string variableName, string expression) = context.GetVariable(graph, sourceNode, sourcePort);
            return (variableName, type);
        }

        public static (string Expression, Type PortType) Emit(EmitContext context, NodeGraph graph, BaseNode node, string template, params string[] ports)
        {
            string[] args = new string[ports.Length];
            Type[] portTypes = new Type[ports.Length];

            for (int i = 0; i < ports.Length; i++)
            {
                (args[i], portTypes[i]) = EmitInputExpression(context, graph, node, ports[i]);
            }

            Type resultType = ExpressionUtilities.GetCastingType(portTypes);

            for (int i = 0; i < args.Length; i++)
            {
                if (portTypes[i] == resultType) continue;
                args[i] = ExpressionUtilities.GetCastedExpression(portTypes[i], resultType, args[i]);
            }

            return (Format(template, args), resultType);
        }

        public static (string Expression, Type PortType) Emit(EmitContext context, NodeGraph graph, BaseNode node, Type resultType, string template, params string[] ports)
        {
            string[] args = new string[ports.Length];
            Type[] portTypes = new Type[ports.Length];

            for (int i = 0; i < ports.Length; i++)
                (args[i], portTypes[i]) = EmitInputExpression(context, graph, node, ports[i]);

            Type argType = ExpressionUtilities.GetCastingType(portTypes);

            for (int i = 0; i < args.Length; i++)
            {
                if (portTypes[i] == argType) continue;
                args[i] = ExpressionUtilities.GetCastedExpression(portTypes[i], argType, args[i]);
            }

            return (Format(template, args), resultType);
        }

        private static string Format(string template, params string[] args)
        {
            StringBuilder builder = new();
            int argIndex = 0;

            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] == '#' && argIndex < args.Length)
                {
                    builder.Append(args[argIndex++]);
                    continue;
                }

                builder.Append(template[i]);
            }

            return builder.ToString();
        }


        private static string F(float value) => value.ToString("0.0############", CultureInfo.InvariantCulture);
        private static string D(double value) => value.ToString("0.0########################", CultureInfo.InvariantCulture);
    }
}