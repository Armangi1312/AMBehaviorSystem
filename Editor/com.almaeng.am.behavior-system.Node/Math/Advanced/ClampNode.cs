using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Clamp")]
    public class ClampNode : BaseNode, IMathNode
    {
        public override string name => "Clamp";

        [Input] public NumberPort In;
        [Input] public NumberPort Min;
        [Input] public NumberPort Max;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) in_ = NodeUtilities.GetInputVariable(nameof(In), context, this);
            (Type Type, string Name) min = NodeUtilities.GetInputVariable(nameof(Min), context, this);
            (Type Type, string Name) max = NodeUtilities.GetInputVariable(nameof(Max), context, this);

            string name = $"clamp_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(in_.Type, min.Type, max.Type);

            Argument inArgument = new(in_.Type, in_.Name);
            Argument minArgument = new(min.Type, min.Name);
            Argument maxArgument = new(max.Type, max.Name);

            ExpressionRule rule = new("Math.Clamp(#, #, #)", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(2, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1),
                ArgumentConstraint.OfSameGroup(2, 1));

            Expression expression = new(new[] { inArgument, minArgument, maxArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}