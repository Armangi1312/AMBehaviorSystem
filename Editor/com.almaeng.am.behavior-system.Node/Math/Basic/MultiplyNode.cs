using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Multiply")]
    public class MultiplyNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Multiply";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public NumberPort B;

        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"multiply_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(a.Type, b.Type);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            ExpressionRule[] rules = new ExpressionRule[]
            {
                new("# * #", outType,
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                    ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                    ArgumentConstraint.OfSameGroup(0, 1),
                    ArgumentConstraint.OfSameGroup(1, 1)),

                new("# * #", outType,
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector),
                    ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar)),

                new("# * #", outType,
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                    ArgumentConstraint.OfCategory(1, ArgumentCategory.Vector))
            };

            Expression expression = new(new[] { leftArgument, rightArgument }, rules);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}