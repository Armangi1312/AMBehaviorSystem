using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Not")]
    public class NotNode : BaseNode, IMathNode
    {
        public override string name => "Not";

        [Input] public BooleanPort In;

        [Output] public BooleanPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"not_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = typeof(bool);

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("!#", outType,
                ArgumentConstraint.OfFixedType(0, typeof(bool)));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}