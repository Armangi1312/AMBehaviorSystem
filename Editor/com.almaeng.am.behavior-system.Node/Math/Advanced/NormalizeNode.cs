using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Normalize")]
    public class NormalizeNode : BaseDynamicTypeNode
    {
        public override string name => "Normalize";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port In;
        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) input = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"normalize_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = input.Type;

            Argument argument = new(input.Type, input.Name);

            ExpressionRule rule = new("#.normalized", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector));

            Expression expression = new(argument, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}