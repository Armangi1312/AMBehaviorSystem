using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseNode, IMathNode
    {
        public override string name => "Abs";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"abs_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type;

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("Math.Abs(#)", outType, ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}