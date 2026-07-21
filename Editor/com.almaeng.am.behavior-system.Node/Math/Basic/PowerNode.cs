using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Power")]
    public class PowerNode : BaseNode, IMathNode
    {
        public override string name => "Power";

        [Input] public NumberPort A;
        [Input] public NumberPort B;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"power_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(a.Type, b.Type);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            Type castingType = TypeUtilities.GetCastingType(a.Type, b.Type);
            string template = castingType == typeof(double) ? "Math.Pow(#, #)" : "MathF.Pow(#, #)";

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { leftArgument, rightArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}
