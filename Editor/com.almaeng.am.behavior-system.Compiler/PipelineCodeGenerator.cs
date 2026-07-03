//using AMBehaviorSystem.Node;
//using AMBehaviorSystem.Node.Data;
//using AMBehaviorSystem.Node.Pipelines;
//using GraphProcessor;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace AMBehaviorSystem.Compiler
//{
//    public static class PipelineCodeGenerator
//    {
//        private const string GeneratedNamespace = "AMBehaviorSystem.Generated";
//        private const string BaseClass = "BasePipeline";
//        private const string ProcessorsParam = "processors";
//        private const string SettingsParam = "settings";
//        private const string ContextsParam = "contexts";
//        private const string TimingParam = "timing";

//        public static string Generate(NodeGraph graph, string className, string namespaceName = GeneratedNamespace)
//        {
//            if (!TryFindEntryNode(graph, out EntryNode entryNode))
//            {
//                Debug.LogError("[PipelineCodeGenerator] EntryNode not found in graph.");
//                return null;
//            }

//            List<string> processorTypes = CollectAllProcessorTypes(graph);
//            List<(string FieldName, string TypeName)> contextFields = CollectContextFields(graph);
//            List<(string FieldName, string TypeName)> settingFields = CollectSettingFields(graph);
//            List<(string GameObjectFieldName, string ComponentFieldName, string ComponentTypeName, string MemberPath)> componentFields = CollectComponentFields(graph);

//            StringBuilder builder = new();

//            AppendUsings(builder);
//            builder.AppendLine();
//            builder.AppendLine($"namespace {namespaceName}");
//            builder.AppendLine("{");
//            AppendClass(builder, className, graph, entryNode, processorTypes, contextFields, settingFields, componentFields);
//            builder.AppendLine("}");

//            return builder.ToString();
//        }

//        private static void AppendUsings(StringBuilder builder)
//        {
//            builder.AppendLine("using System;");
//            builder.AppendLine("using System.Collections.Generic;");
//            builder.AppendLine("using AMBehaviorSystem.Core;");
//            builder.AppendLine("using AMBehaviorSystem.Core.Pipelines;");
//            builder.AppendLine("using AMBehaviorSystem.Core.Utilities;");
//            builder.AppendLine("using UnityEngine;");
//        }

//        private static (string ProcessorType, string SettingType, string ContextType) ResolveControllerTypeArgs(NodeGraph graph)
//        {
//            if (graph is not NodeGraph nodeGraph) return ("Processor", "ISetting", "IContext");
//            nodeGraph.Resolve();

//            UnityEngine.Object controller = nodeGraph.TargetController;
//            if (controller == null) return ("Processor", "ISetting", "IContext");

//            Type baseType = controller.GetType().BaseType;
//            while (baseType != null && baseType != typeof(object))
//            {
//                if (baseType.IsGenericType && baseType.GetGenericArguments().Length >= 3)
//                {
//                    Type[] args = baseType.GetGenericArguments();
//                    return (args[2].Name, args[0].Name, args[1].Name);
//                }
//                baseType = baseType.BaseType;
//            }

//            return ("Processor", "ISetting", "IContext");
//        }

//        private static void AppendClass(
//            StringBuilder builder,
//            string className,
//            NodeGraph graph,
//            EntryNode entryNode,
//            List<string> processorTypes,
//            List<(string FieldName, string TypeName)> contextFields,
//            List<(string FieldName, string TypeName)> settingFields,
//            List<(string GameObjectFieldName, string ComponentFieldName, string ComponentTypeName, string MemberPath)> componentFields)
//        {
//            (string processorType, string settingType, string contextType) = ResolveControllerTypeArgs(graph);

//            builder.AppendLine("\t[Serializable]");
//            builder.AppendLine($"\tpublic class {className} : {BaseClass}<{processorType}, {settingType}, {contextType}>");
//            builder.AppendLine("\t{");

//            AppendFields(builder, processorTypes, contextFields, settingFields, componentFields);
//            builder.AppendLine();
//            AppendInitialize(builder, processorTypes, contextFields, settingFields, componentFields, processorType, settingType, contextType);
//            builder.AppendLine();
//            AppendInvoke(builder, graph, entryNode);

//            builder.AppendLine("\t}");
//        }

//        private static void AppendFields(
//            StringBuilder builder,
//            List<string> processorTypes,
//            List<(string FieldName, string TypeName)> contextFields,
//            List<(string FieldName, string TypeName)> settingFields,
//            List<(string GameObjectFieldName, string ComponentFieldName, string ComponentTypeName, string MemberPath)> componentFields)
//        {
//            foreach (string typeName in processorTypes)
//                builder.AppendLine($"\t\tprivate {typeName} {ToFieldName(typeName)};");

//            foreach ((string fieldName, string typeName) in contextFields)
//                builder.AppendLine($"\t\tprivate {typeName} {fieldName};");

//            foreach ((string fieldName, string typeName) in settingFields)
//                builder.AppendLine($"\t\tprivate {typeName} {fieldName};");

//            foreach ((string gameObjectFieldName, string componentFieldName, string componentTypeName, _) in componentFields)
//            {
//                builder.AppendLine($"\t\t[SerializeField] private GameObject {gameObjectFieldName};");
//                builder.AppendLine($"\t\tprivate {componentTypeName} {componentFieldName};");
//            }
//        }

//        private static void AppendInitialize(
//            StringBuilder builder,
//            List<string> processorTypes,
//            List<(string FieldName, string TypeName)> contextFields,
//            List<(string FieldName, string TypeName)> settingFields,
//            List<(string GameObjectFieldName, string ComponentFieldName, string ComponentTypeName, string MemberPath)> componentFields,
//            string processorType,
//            string settingType,
//            string contextType)
//        {
//            builder.AppendLine($"\t\tpublic override void Initialize(IReadOnlyList<{processorType}> {ProcessorsParam}, IReadOnlyRegistry<{settingType}> {SettingsParam}, IReadOnlyRegistry<{contextType}> {ContextsParam})");
//            builder.AppendLine("\t\t{");

//            foreach (string typeName in processorTypes)
//                builder.AppendLine($"\t\t\t{ToFieldName(typeName)} = Find<{typeName}>({ProcessorsParam});");

//            foreach ((string fieldName, string typeName) in contextFields)
//                builder.AppendLine($"\t\t\t{fieldName} = {ContextsParam}.Get<{typeName}>();");

//            foreach ((string fieldName, string typeName) in settingFields)
//                builder.AppendLine($"\t\t\t{fieldName} = {SettingsParam}.Get<{typeName}>();");

//            foreach ((string gameObjectFieldName, string componentFieldName, string componentTypeName, _) in componentFields)
//                builder.AppendLine($"\t\t\t{componentFieldName} = {gameObjectFieldName}.GetComponent<{componentTypeName}>();");

//            builder.AppendLine("\t\t}");
//        }

//        private static void AppendInvoke(StringBuilder builder, NodeGraph graph, EntryNode entryNode)
//        {
//            builder.AppendLine($"\t\tpublic override void Invoke(InvokeTiming {TimingParam})");
//            builder.AppendLine("\t\t{");

//            BaseNode firstNode = GraphTraversal.GetOutputNode(graph, entryNode, nameof(EntryNode.Entry));
//            if (firstNode != null)
//                TraverseNode(graph, builder, firstNode, new HashSet<string>(), 3);

//            builder.AppendLine("\t\t}");
//        }

//        private static void TraverseNode(NodeGraph graph, StringBuilder builder, BaseNode node, HashSet<string> visited, int indent)
//        {
//            if (node == null || !visited.Add(node.GUID)) return;

//            switch (node)
//            {
//                case InvokeNode invokeNode:
//                    EmitInvokeNode(graph, builder, invokeNode, visited, indent);
//                    break;
//                case ConditionNode conditionNode:
//                    EmitConditionNode(graph, builder, conditionNode, visited, indent);
//                    break;
//            }
//        }

//        private static void EmitInvokeNode(NodeGraph graph, StringBuilder builder, InvokeNode node, HashSet<string> visited, int indent)
//        {
//            string tabs = Tabs(indent);

//            foreach (string rawTypeName in node.ProcessorTypes)
//            {
//                string typeName = FormatTypeName(rawTypeName);
//                builder.AppendLine($"{tabs}InvokeProcessor({ToFieldName(typeName)}, {TimingParam});");
//            }

//            BaseNode nextNode = GraphTraversal.GetOutputNode(graph, node, nameof(BasePipelineNode.Next));
//            TraverseNode(graph, builder, nextNode, visited, indent);
//        }

//        private static void EmitConditionNode(NodeGraph graph, StringBuilder builder, ConditionNode node, HashSet<string> visited, int indent)
//        {
//            string tabs = Tabs(indent);
//            EmitContext emitContext = new();

//            BaseNode conditionSourceNode = GraphTraversal.GetInputNode(graph, node, nameof(ConditionNode.Condition));
//            string conditionVarName = conditionSourceNode != null
//                ? emitContext.GetOrEmit(graph, conditionSourceNode, out _)
//                : "false";

//            emitContext.WriteDeclarations(builder, indent);

//            builder.AppendLine($"{tabs}if ({conditionVarName})");
//            builder.AppendLine($"{tabs}{{");
//            BaseNode trueNode = GraphTraversal.GetOutputNode(graph, node, nameof(ConditionNode.True));
//            TraverseNode(graph, builder, trueNode, new HashSet<string>(visited), indent + 1);
//            builder.AppendLine($"{tabs}}}");

//            builder.AppendLine($"{tabs}else");
//            builder.AppendLine($"{tabs}{{");
//            BaseNode falseNode = GraphTraversal.GetOutputNode(graph, node, nameof(ConditionNode.False));
//            TraverseNode(graph, builder, falseNode, new HashSet<string>(visited), indent + 1);
//            builder.AppendLine($"{tabs}}}");
//        }

//        private static List<string> CollectAllProcessorTypes(NodeGraph graph)
//        {
//            HashSet<string> seen = new();
//            List<string> result = new();

//            foreach (BaseNode node in graph.nodes)
//            {
//                if (node is not InvokeNode invokeNode) continue;

//                foreach (string rawTypeName in invokeNode.ProcessorTypes)
//                {
//                    string typeName = FormatTypeName(rawTypeName);
//                    if (seen.Add(typeName))
//                        result.Add(typeName);
//                }
//            }

//            return result;
//        }

//        private static List<(string FieldName, string TypeName)> CollectContextFields(NodeGraph graph)
//        {
//            HashSet<string> seen = new();
//            List<(string, string)> result = new();

//            foreach (BaseNode node in graph.nodes)
//            {
//                if (node is not ContextDataNode contextNode) continue;

//                string typeName = FormatTypeName(contextNode.Type);
//                if (seen.Add(typeName))
//                    result.Add((ToFieldName(typeName), typeName));
//            }

//            return result;
//        }

//        private static List<(string FieldName, string TypeName)> CollectSettingFields(NodeGraph graph)
//        {
//            HashSet<string> seen = new();
//            List<(string, string)> result = new();

//            foreach (BaseNode node in graph.nodes)
//            {
//                if (node is not SettingDataNode settingNode) continue;

//                string typeName = FormatTypeName(settingNode.Type);
//                if (seen.Add(typeName))
//                    result.Add((ToFieldName(typeName), typeName));
//            }

//            return result;
//        }

//        private static List<(string GameObjectFieldName, string ComponentFieldName, string ComponentTypeName, string MemberPath)> CollectComponentFields(NodeGraph graph)
//        {
//            List<(string, string, string, string)> result = new();

//            foreach (BaseNode node in graph.nodes)
//            {
//                if (node is not ComponentDataNode componentNode) continue;

//                string guid = componentNode.GUID[..8].Replace("-", "");
//                string[] segments = componentNode.Path.Split('.');
//                string componentTypeName = segments.Length >= 2 ? segments[0] : "Transform";
//                string memberPath = segments.Length >= 2 ? string.Join('.', segments[1..]) : componentNode.Path;
//                string gameObjectFieldName = $"object{guid}";
//                string componentFieldName = $"{char.ToLower(componentTypeName[0])}{componentTypeName[1..]}{guid}";

//                result.Add((gameObjectFieldName, componentFieldName, componentTypeName, memberPath));
//            }

//            return result;
//        }

//        private static bool TryFindEntryNode(NodeGraph graph, out EntryNode entryNode)
//        {
//            entryNode = graph.nodes.OfType<EntryNode>().FirstOrDefault();
//            return entryNode != null;
//        }

//        private static string FormatTypeName(string fullTypeName)
//        {
//            if (string.IsNullOrEmpty(fullTypeName)) return "(null)";

//            int spaceIndex = fullTypeName.IndexOf(' ');
//            if (spaceIndex >= 0)
//                fullTypeName = fullTypeName[(spaceIndex + 1)..];

//            int dotIndex = fullTypeName.LastIndexOf('.');
//            return dotIndex >= 0 ? fullTypeName[(dotIndex + 1)..] : fullTypeName;
//        }

//        private static string ToFieldName(string typeName)
//        {
//            if (string.IsNullOrEmpty(typeName)) return typeName;
//            return char.ToLower(typeName[0]) + typeName[1..];
//        }

//        private static string Tabs(int count) => new('\t', count);
//    }
//}
