using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Lerp")]
    public class LerpNode : BaseDynamicTypeNode
    {
        public override string name => "Lerp";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public Port B;
        [Input] public NumberPort T;

        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);
            (Type Type, string Name) t = NodeUtilities.GetInputVariable(nameof(T), context, this);

            string name = $"lerp_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(a.Type, b.Type);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);
            Argument tArgument = new(t.Type, t.Name);

            string template = outType.IsVector()
                ? $"{outType.Name}.Lerp(#, #, #)"
                : "Mathf.Lerp(#, #, #)";

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Numeric),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Numeric),
                ArgumentConstraint.OfCategory(2, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1),
                ArgumentConstraint.OfFixedType(2, typeof(float)));

            Expression expression = new(new[] { leftArgument, rightArgument, tArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}