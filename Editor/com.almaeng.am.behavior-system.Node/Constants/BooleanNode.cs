using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Constants
{
    [Serializable]
    [NodeMenuItem("Constant/Boolean")]
    public class BooleanNode : BaseValueNode<bool, BooleanPort>, IConstantNode
    {
        public override string name => "Boolean";

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            string value = Field ? "true" : "false";
            string name = $"boolean_{GUIDParse.GetGUIDParse(GUID)}";

            Argument argument = new(typeof(bool), value);
            ExpressionRule rule = new("#", typeof(bool), ArgumentConstraint.OfFixedType(0, typeof(bool)));

            Expression expression = new(argument, rule);
            DeclarationStatement statement = new(typeof(bool), name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (typeof(bool), name);
        }
    }
}
