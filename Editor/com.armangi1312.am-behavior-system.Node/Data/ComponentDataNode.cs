using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    [NodeMenuItem("Data/Component Data")]
    public class ComponentDataNode : BaseDataNode, ISourceGenerationNode
    {
        public override string name => "Component Data";

        public void Generate(SourceContext context)
        {
            string typeName = Type.Name;
            string componentName = CodeFormatUtility.ToCamelCase(typeName);

            if(!context.DeclaredComponents.Contains(typeName))
            {
                context.DeclaredComponents.Add(typeName);

                if(!string.IsNullOrEmpty(Type.Namespace))
                    context.UsingNamespaces.Add(Type.Namespace);

                DeclarationStatement declaration = new(DeclarationStatement.AccessModifier.Private, Type, componentName);

                Argument genericArgument = new(Type, typeName);

                ExpressionRule assignmentRule = new("owner.GetComponentInChildren<#>()", Type, ArgumentConstraint.OfFixedType(0, Type));

                Expression assignmentExpression = new(new[] { genericArgument }, assignmentRule);
                AssignmentStatement assignment = new(componentName, assignmentExpression);

                context.InitializeStatements.Add(assignment);
                context.MemberStatements.Add(declaration);
            }

            context.OutputLocals[PortKey.Of(GUID, componentName)] = (OutputType, componentName);

            string value = $"{componentName}.{Path}";
            string name = $"component_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(OutputType, value);
            ExpressionRule rule = new("#", OutputType, ArgumentConstraint.OfFixedType(0, OutputType));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(OutputType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (OutputType, name);
        }
    }
}