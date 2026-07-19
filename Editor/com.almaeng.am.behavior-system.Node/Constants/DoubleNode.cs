using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using System.Globalization;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Double")]
    public class DoubleNode : BaseValueNode<double, NumberPort>, IConstantNode
    {
        public override string name => "Double";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = $"{Field.ToString(CultureInfo.InvariantCulture)}d";
            string name = $"double_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(double), value);
            ExpressionRule rule = new("#", typeof(double), ArgumentConstraint.OfFixedType(0, typeof(double)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(double), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(double), name);
        }
    }
}
