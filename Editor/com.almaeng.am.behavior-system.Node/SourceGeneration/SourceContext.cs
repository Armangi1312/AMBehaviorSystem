using System;
using System.Collections.Generic;
using System.Text;

namespace AMBehaviorSystem.Node.SourceGeneration
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
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            foreach (string @namespace in UsingNamespaces)
            {
                builder.AppendLine($"using {@namespace};");
            }

            builder.AppendLine();
            builder.AppendLine($"namespace {Namespace}");
            builder.AppendLine("{");
            builder.AppendLine($"\tpublic class {Name} : BasePipeline<{ProcessorType.FullName}, {SettingType.FullName}, {ContextType.FullName}>");
            builder.AppendLine("\t{");

            foreach (Statement statement in MemberStatements)
            {
                builder.AppendLine($"\t\t{statement}");
            }

            builder.AppendLine();
            builder.AppendLine($"\t\tpublic override void Initialize(IReadOnlyList<{ProcessorType.FullName}> processors, IReadOnlyRegistry<{SettingType.FullName}> settings, IReadOnlyRegistry<{ContextType.FullName}> contexts, Component owner)");
            builder.AppendLine("\t\t{");

            foreach (Statement statement in InitializeStatements)
            {
                builder.AppendLine($"\t\t\t{statement}");
            }

            builder.AppendLine("\t\t}");
            builder.AppendLine();

            builder.AppendLine("\t\tpublic override void Invoke(InvokeTiming timing)");
            builder.AppendLine("\t\t{");

            foreach (Statement statement in InvokeStatements)
            {
                builder.AppendLine($"\t\t\t{statement}");
            }

            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}