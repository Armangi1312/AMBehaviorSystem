using AMBehaviorSystem.Node;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Compiler
{
    internal class EmitContext
    {
        private readonly Dictionary<string, (Type Type, string VariableName, string Expression)> variableCache = new();

        public Type GetOutputType(NodeGraph graph, BaseNode node, string outputPort)
        {
            return GetVariable(graph, node, outputPort).Type;
        }

        public string GetVariableName(NodeGraph graph, BaseNode node, string outputPort)
        {
            return GetVariable(graph, node, outputPort).VariableName;
        }

        public string GetExpression(NodeGraph graph, BaseNode node, string outputPort)
        {
            return GetVariable(graph, node, outputPort).Expression;
        }

        public IEnumerable<string> GetDeclarations()
        {
            foreach ((Type type, string variableName, string expression) in variableCache.Values)
            {
                yield return $"{type.FullName} {variableName} = {expression};";
            }
        }

        public (Type Type, string VariableName, string Expression) GetVariable(NodeGraph graph, BaseNode node, string outputPort)
        {
            string cacheKey = $"{node.GUID}:{outputPort}";

            if (variableCache.TryGetValue(cacheKey, out var cachedValue))
            {
                return cachedValue;
            }

            (string expression, Type type) = ExpressionEmitter.EmitExpression(this, graph, node);
            string variableName = $"{node.GetType().Name}_{outputPort}";

            (Type, string, string) result = (type, variableName, expression);

            variableCache.Add(cacheKey, result);

            return result;
        }
    }
}