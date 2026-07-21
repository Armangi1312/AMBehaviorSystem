using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Basic
{
    [Serializable]
    [NodeMenuItem("Math/Basic/Divide")]
    public class DivideNode : BaseDynamicTypeNode, IMathNode
    {
        public override string name => "Divide";

        protected override Type DefaultType => typeof(NumberPort);

        [Input] public Port A;
        [Input] public NumberPort B;

        [Output] public Port Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"divide_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(a.Type, b.Type);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            ExpressionRule[] rules = new ExpressionRule[]
            {
                new("# / #", outType,
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                    ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                    ArgumentConstraint.OfSameGroup(0, 1),
                    ArgumentConstraint.OfSameGroup(1, 1)),

                new("# / #", outType,
                    ArgumentConstraint.OfCategory(0, ArgumentCategory.Vector),
                    ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar))
            };

            Expression expression = new(new[] { leftArgument, rightArgument }, rules);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }
    }
}