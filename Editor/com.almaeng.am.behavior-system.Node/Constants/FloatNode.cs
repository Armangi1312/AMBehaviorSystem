using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using System.Globalization;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Float")]
    public class FloatNode : BaseValueNode<float, NumberPort>, IConstantNode
    {
        public override string name => "Float";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = $"{Field.ToString(CultureInfo.InvariantCulture)}f";
            string name = $"float_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(float), value);
            ExpressionRule rule = new("#", typeof(float), ArgumentConstraint.OfFixedType(0, typeof(float)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(float), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(float), name);
        }
    }
}
