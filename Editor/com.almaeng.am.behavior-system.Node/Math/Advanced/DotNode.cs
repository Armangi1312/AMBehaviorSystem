using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Dot")]
    public class DotNode : BaseDynamicTypeNode
    {
        public override string name => "Dot";

        protected override Type DefaultType => typeof(Vector2Port);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"dot_{GUIDParse.GetGUIDParse(GUID)}";
            Type vectorType = TypeUtilities.GetCastingType(a.Type, b.Type);
            Type outType = typeof(float);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            string template = $"{vectorType.Name}.Dot(#, #)";

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Vector),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { leftArgument, rightArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}