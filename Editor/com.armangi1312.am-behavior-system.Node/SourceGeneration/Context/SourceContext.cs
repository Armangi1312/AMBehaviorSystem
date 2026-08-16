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
            "AMBehaviorSystem",
            "AMBehaviorSystem.Attributes",
            "AMBehaviorSystem.Pipelines",
            "AMBehaviorSystem.Utilities",
            "System",
            "System.Collections.Generic",
            "UnityEngine"
        };

        public Dictionary<PortKey, (Type Type, string Name)> OutputFields { get; } = new();
        public Dictionary<PortKey, (Type Type, string Name)> OutputLocals { get; } = new();

        public HashSet<string> DeclaredProcessors { get; } = new();
        public HashSet<string> DeclaredComponents { get; } = new();
        public HashSet<string> DeclaredSettings { get; } = new();
        public HashSet<string> DeclaredContexts { get; } = new();

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

            UsingNamespaces.Clear();
            UsingNamespaces.Add("AMBehaviorSystem");
            UsingNamespaces.Add("AMBehaviorSystem.Attributes");
            UsingNamespaces.Add("AMBehaviorSystem.Pipelines");
            UsingNamespaces.Add("AMBehaviorSystem.Utilities");
            UsingNamespaces.Add("System");
            UsingNamespaces.Add("System.Collections.Generic");
            UsingNamespaces.Add("UnityEngine");

            OutputLocals.Clear();
            OutputFields.Clear();

            DeclaredProcessors.Clear();
            DeclaredComponents.Clear();
            DeclaredSettings.Clear();
            DeclaredContexts.Clear();
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
            AddNamespaceIfNotEmpty(ProcessorType.Namespace);
            AddNamespaceIfNotEmpty(SettingType.Namespace);
            AddNamespaceIfNotEmpty(ContextType.Namespace);

            builder.AppendLine($"namespace {Namespace}");
            builder.AppendLine("{");
            builder.AppendLine("\t[Serializable]");
            builder.AppendLine($"\tpublic class {Name} : BasePipeline<{ProcessorType.Name}, {SettingType.Name}, {ContextType.Name}>");
            builder.AppendLine("\t{");
        }

        private void AppendMembers(StringBuilder builder)
        {
            builder.Append(CodeFormatUtility.RenderStatements(MemberStatements, 2));
        }

        private void AppendInitializeMethod(StringBuilder builder)
        {
            builder.AppendLine($"\t\tpublic override void Initialize(IReadOnlyList<{ProcessorType.Name}> processors, IReadOnlyRegistry<{SettingType.Name}> settings, IReadOnlyRegistry<{ContextType.Name}> contexts, Component owner)");
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

        private void AddNamespaceIfNotEmpty(string @namespace)
        {
            if(!string.IsNullOrEmpty(@namespace))
                UsingNamespaces.Add(@namespace);
        }
    }
}
