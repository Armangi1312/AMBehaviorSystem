using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Data
{
    [Serializable]
    [NodeMenuItem("Data/Setting Data")]
    public class SettingDataNode : BaseDataNode, ISourceGenerationNode
    {
        public override string name => "Setting Data";

        public void Generate(SourceContext context)
        {
            string typeName = Type.Name;
            string settingName = CodeFormatUtility.ToCamelCase(typeName);

            if(!context.DeclaredSettings.Contains(typeName))
            {
                context.DeclaredSettings.Add(typeName);

                if(!string.IsNullOrEmpty(Type.Namespace))
                    context.UsingNamespaces.Add(Type.Namespace);

                DeclarationStatement declaration = new(DeclarationStatement.AccessModifier.Private, Type, settingName);

                Argument genericArgument = new(Type, typeName);

                ExpressionRule assignmentRule = new("settings.Get<#>()", Type, ArgumentConstraint.OfFixedType(0, Type));

                Expression assignmentExpression = new(new[] { genericArgument }, assignmentRule);
                AssignmentStatement assignment = new(settingName, assignmentExpression);

                context.InitializeStatements.Add(assignment);
                context.MemberStatements.Add(declaration);
            }

            context.OutputLocals[PortKey.Of(GUID, settingName)] = (OutputType, settingName);

            string value = $"{settingName}.{Path}";
            string name = $"setting_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(OutputType, value);
            ExpressionRule rule = new("#", OutputType, ArgumentConstraint.OfFixedType(0, OutputType));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(OutputType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (OutputType, name);
        }
    }
}