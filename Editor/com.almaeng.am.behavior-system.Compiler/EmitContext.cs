//using AMBehaviorSystem.Node;
//using AMBehaviorSystem.Node.Math.Advanced;
//using GraphProcessor;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AMBehaviorSystem.Compiler
//{
//    internal class EmitContext
//    {
//        private readonly Dictionary<string, string> nodeVarNames = new();
//        private readonly Dictionary<string, Type> nodeVarTypes = new();
//        private readonly List<(string TypeName, string VarName, string Expression)> declarations = new();
//        private int varCounter;

//        public string GetOrEmit(NodeGraph graph, BaseNode node, out Type portType)
//            => GetOrEmit(graph, node, null, out portType);

//        public string GetOrEmit(NodeGraph graph, BaseNode node, string outputPort, out Type portType)
//        {
//            string cacheKey = $"{node.GUID}:{outputPort}";

//            if (nodeVarNames.TryGetValue(cacheKey, out string existingVarName))
//            {
//                portType = nodeVarTypes[cacheKey];
//                return existingVarName;
//            }

//            if (node is SplitNode && outputPort != null)
//            {
//                string baseVarName = GetOrEmit(graph, node, null, out _);
//                string memberAccess = outputPort switch
//                {
//                    "X" => "x",
//                    "Y" => "y",
//                    "Z" => "z",
//                    "W" => "w",
//                    _ => "x"
//                };

//                string expression = $"{baseVarName}.{memberAccess}";
//                portType = typeof(float);

//                nodeVarNames[cacheKey] = expression;
//                nodeVarTypes[cacheKey] = portType;
//                return expression;
//            }

//            (string expr, Type resolvedPortType) = ExpressionEmitter.EmitExpression(this, graph, node);
//            portType = resolvedPortType;

//            string varName = $"var{++varCounter}";
//            string typeName = "var";

//            declarations.Add((typeName, varName, expr));
//            nodeVarNames[cacheKey] = varName;
//            nodeVarTypes[cacheKey] = portType;

//            return varName;
//        }

//        public void WriteDeclarations(StringBuilder builder, int indent)
//        {
//            string tabs = new('\t', indent);

//            foreach ((string typeName, string varName, string expression) in declarations)
//                builder.AppendLine($"{tabs}{typeName} {varName} = {expression};");
//        }
//    }
//}