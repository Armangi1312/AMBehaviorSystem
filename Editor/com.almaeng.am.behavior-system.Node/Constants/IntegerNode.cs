using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Globalization;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Integer")]
    public class IntegerNode : BaseValueNode<int, NumberPort>, IConstantNode
    {
        public override string name => "Integer";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = Field.ToString(CultureInfo.InvariantCulture);
            string name = $"int_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(int), value);
            ExpressionRule rule = new("#", typeof(int), ArgumentConstraint.OfFixedType(0, typeof(int)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(int), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (typeof(int), name);
        }
    }
}
