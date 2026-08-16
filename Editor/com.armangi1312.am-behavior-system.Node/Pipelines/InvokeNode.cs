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
            for(int i = 0; i < ProcessorTypes.Count; i++)
            {
                Debug.Log($"Processing processor type: {ProcessorTypes[i]}");
                (string assemblyName, string @namespace, string className, string fullName) = ParseTypeString(ProcessorTypes[i]);

                string processorName = CodeFormatUtility.ToCamelCase(className);

                if(!context.DeclaredProcessors.Contains(fullName))
                {
                    context.DeclaredProcessors.Add(fullName);

                    if(!string.IsNullOrEmpty(@namespace))
                        context.UsingNamespaces.Add(@namespace);

                    DeclarationStatement declaration = new(DeclarationStatement.AccessModifier.Private, typeof(Processor), processorName);

                    Argument genericArgument = new(typeof(Type), className);
                    Argument processorsArgument = new(typeof(IReadOnlyList<Processor>), "processors");

                    ExpressionRule assignmentRule = new("Find<#>(#)", typeof(Processor),
                        ArgumentConstraint.OfFixedType(0, typeof(Type)),
                        ArgumentConstraint.OfFixedType(1, typeof(IReadOnlyList<Processor>)));

                    Expression assignmentExpression = new(new[] { genericArgument, processorsArgument }, assignmentRule);
                    AssignmentStatement assignment = new(processorName, assignmentExpression);

                    context.InitializeStatements.Add(assignment);
                    context.MemberStatements.Add(declaration);
                }

                context.OutputLocals[PortKey.Of(GUID, processorName)] = (typeof(Processor), processorName);

                Argument processorArgument = new(typeof(Processor), processorName);
                Argument invokeTimingArgument = new(typeof(InvokeTiming), "timing");

                ExpressionRule rule = new("InvokeProcessor(#, #)", typeof(void),
                    ArgumentConstraint.OfFixedType(0, typeof(Processor)),
                    ArgumentConstraint.OfFixedType(1, typeof(InvokeTiming)));

                Expression expression = new(new[] { processorArgument, invokeTimingArgument }, rule);

                InvokeStatement statement = new(expression);

                context.InvokeStatements.Add(statement);
            }
        }

        private static (string AssemblyName, string Namespace, string ClassName, string FullName) ParseTypeString(string typeString)
        {
            int spaceIndex = typeString.IndexOf(' ');

            string assemblyName = typeString[..spaceIndex];
            string fullName = typeString[(spaceIndex + 1)..];

            int lastDotIndex = fullName.LastIndexOf('.');

            string @namespace = lastDotIndex >= 0 ? fullName[..lastDotIndex] : string.Empty;
            string className = lastDotIndex >= 0 ? fullName[(lastDotIndex + 1)..] : fullName;

            return (assemblyName, @namespace, className, fullName);
        }
    }
}