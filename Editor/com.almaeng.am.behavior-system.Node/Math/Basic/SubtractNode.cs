using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Subtract")]
    public class SubtractNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Subtract";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public Port B;

        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"subtract_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(a.Type, b.Type);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            ExpressionRule rule = new(
                "# - #", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Numeric),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Numeric),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { leftArgument, rightArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}
