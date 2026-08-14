using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AMBehaviorSystem.Node.SourceGeneration.Context
{
    public class SourceContext
    {
        public List<Statement> MemberStatements { get; } = new();
        public List<Statement> InitializeStatements { get; } = new();
        public List<Statement> InvokeStatements { get; } = new();

        public HashSet<string> UsingNamespaces = new()
        {
            "AMBehaviorSystem.Core",
            "AMBehaviorSystem.Core.Attributes",
            "AMBehaviorSystem.Core.Pipelines",
            "AMBehaviorSystem.Core.Utilities",
            "System",
            "System.Collections.Generic",
            "UnityEngine"
        };

        public Dictionary<PortKey, (Type Type, string Name)> OutputFields { get; } = new();
        public Dictionary<PortKey, (Type Type, string Name)> OutputLocals { get; } = new();
        public HashSet<string> DeclaredProcessors { get; } = new();

        public string Name { get; }
        public string Namespace { get; }

        public Type ProcessorType { get; }
        public Type SettingType { get; }
        public Type ContextType { get; }

        public SourceContext(string name, string @namespace, Type processorType, Type settingType, Type contextType)
        {
            Name = name;
            Namespace = @namespace;

            ProcessorType = processorType;
            SettingType = settingType;
            ContextType = contextType;
        }

        public void Clear()
        {
            MemberStatements.Clear();
            InitializeStatements.Clear();
            InvokeStatements.Clear();
            OutputLocals.Clear();
            OutputFields.Clear();
            DeclaredProcessors.Clear();
        }

        public override string ToString()
        {
            try
            {
                StringBuilder builder = new();

                AppendUsingDirectives(builder);
                builder.AppendLine();
                AppendNamespaceAndClassHeader(builder);
                AppendMembers(builder);
                builder.AppendLine();
                AppendInitializeMethod(builder);
                builder.AppendLine();
                AppendInvokeMethod(builder);

                builder.AppendLine("\t}");
                builder.AppendLine("}");

                return builder.ToString();
            }
            catch(Exception ex)
            {
                Debug.LogError($"Error generating source for '{Name}': {ex.Message}\n{ex.StackTrace}");
                Debug.LogError(ex);
                return "";
            }
        }

        private void AppendUsingDirectives(StringBuilder builder)
        {
            foreach(string @namespace in UsingNamespaces)
            {
                builder.AppendLine($"using {@namespace};");
            }
        }

        private void AppendNamespaceAndClassHeader(StringBuilder builder)
        {
            builder.AppendLine($"namespace {Namespace}");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic class {Name} : BasePipeline<{ProcessorType.FullName}, {SettingType.FullName}, {ContextType.FullName}>");
            builder.AppendLine("\t{");
        }

        private void AppendMembers(StringBuilder builder)
        {
            builder.Append(CodeFormatUtility.RenderStatements(MemberStatements, 2));
        }

        private void AppendInitializeMethod(StringBuilder builder)
        {
            builder.AppendLine($"\t\tpublic override void Initialize(IReadOnlyList<{ProcessorType.FullName}> processors, IReadOnlyRegistry<{SettingType.FullName}> settings, IReadOnlyRegistry<{ContextType.FullName}> contexts, Component owner)");
            builder.AppendLine("\t\t{");
            builder.Append(CodeFormatUtility.RenderStatements(InitializeStatements, 3));
            builder.AppendLine("\t\t}");
        }

        private void AppendInvokeMethod(StringBuilder builder)
        {
            builder.AppendLine("\t\tpublic override void Invoke(InvokeTiming timing)");
            builder.AppendLine("\t\t{");
            builder.Append(CodeFormatUtility.RenderStatements(InvokeStatements, 3));
            builder.AppendLine("\t\t}");
        }
    }
}
