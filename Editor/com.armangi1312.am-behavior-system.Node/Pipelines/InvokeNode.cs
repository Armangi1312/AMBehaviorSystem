using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Invoke")]
    public class InvokeNode : BasePipelineNode, ISourceGenerationNode
    {
        public override string name => "Invoke";

        public List<string> ProcessorTypes = new();

        public void Generate(SourceContext context)
        {
            for (int i = 0; i < ProcessorTypes.Count; i++)
            {
                string typeName = FormatTypeName(ProcessorTypes[i]);

                string processorName = CodeFormatUtility.ToCamelCase(typeName);

                if (context.OutputLocals.TryAdd(PortKey.Of(GUID, processorName), (typeof(Processor), processorName)))
                {
                    DeclarationStatement declaration = new(DeclarationStatement.AccessModifier.Private, typeof(Processor), processorName);

                    Argument genericArgument = new(typeof(Type), typeName);
                    Argument processorsArgument = new(typeof(IReadOnlyList<Processor>), "processors");

                    ExpressionRule assignmentRule = new("Find<#>(#)", typeof(Processor),
                        ArgumentConstraint.OfFixedType(0, typeof(Type)),
                        ArgumentConstraint.OfFixedType(1, typeof(IReadOnlyList<Processor>)));

                    Expression assignmentExpression = new(new[] { genericArgument, processorsArgument }, assignmentRule);
                    AssignmentStatement assignment = new(processorName, assignmentExpression);

                    context.InitializeStatements.Add(assignment);
                    context.MemberStatements.Add(declaration);
                }

                Argument processorArgument = new(typeof(Processor), processorName);
                Argument invokeTimingArgument = new(typeof(InvokeTiming), "timing");

                ExpressionRule rule = new("Invoke(#, #)", typeof(void),
                    ArgumentConstraint.OfFixedType(0, typeof(Processor)),
                    ArgumentConstraint.OfFixedType(1, typeof(InvokeTiming)));

                Expression expression = new(new[] { processorArgument, invokeTimingArgument }, rule);

                InvokeStatement statement = new(expression);

                context.InvokeStatements.Add(statement);
            }
        }

        private static string FormatTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return "(null)";

            int spaceIndex = fullTypeName.IndexOf(' ');
            if (spaceIndex < 0) return fullTypeName;

            string typeName = fullTypeName[(spaceIndex + 1)..];
            int dotIndex = typeName.LastIndexOf('.');
            return dotIndex >= 0 ? typeName[(dotIndex + 1)..] : typeName;
        }
    }
}