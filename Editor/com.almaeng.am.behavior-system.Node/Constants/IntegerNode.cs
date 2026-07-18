using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
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

            string value = $"{Field.ToString(CultureInfo.InvariantCulture)}";
            string name = $"integer_{GUIDParse.GetGUIDParse(GUID)}";
            Argument argument = new(typeof(int), value);
            Expression expression = new(new[] { argument }, new[] { typeof(int) }, typeof(int), "#");
            DeclarationStatement statement = new(typeof(int), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(int), name);
        }
    }
}
