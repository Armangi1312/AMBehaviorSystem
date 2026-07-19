using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Remap")]
    public class RemapNode : BaseNode, IMathNode
    {
        public override string name => "Remap";

        [Input] public NumberPort Value;
        [Input] public NumberPort FromMin;
        [Input] public NumberPort FromMax;
        [Input] public NumberPort ToMin;
        [Input] public NumberPort ToMax;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) value = NodeUtilities.GetInputVariable(nameof(Value), context, this);
            (Type Type, string Name) fromMin = NodeUtilities.GetInputVariable(nameof(FromMin), context, this);
            (Type Type, string Name) fromMax = NodeUtilities.GetInputVariable(nameof(FromMax), context, this);
            (Type Type, string Name) toMin = NodeUtilities.GetInputVariable(nameof(ToMin), context, this);
            (Type Type, string Name) toMax = NodeUtilities.GetInputVariable(nameof(ToMax), context, this);

            string name = $"remap_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = TypeUtilities.GetCastingType(value.Type, fromMin.Type, fromMax.Type, toMin.Type, toMax.Type);

            Argument valueArgument = new(value.Type, value.Name);
            Argument fromMinArgument = new(fromMin.Type, fromMin.Name);
            Argument fromMaxArgument = new(fromMax.Type, fromMax.Name);
            Argument toMinArgument = new(toMin.Type, toMin.Name);
            Argument toMaxArgument = new(toMax.Type, toMax.Name);

            ExpressionRule rule = new("# + (# - #) / (# - #) * (# - #)", outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(2, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(3, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(4, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1),
                ArgumentConstraint.OfSameGroup(2, 1),
                ArgumentConstraint.OfSameGroup(3, 1),
                ArgumentConstraint.OfSameGroup(4, 1));

            Expression expression = new(
                new[] { toMinArgument, valueArgument, fromMinArgument, fromMaxArgument, fromMinArgument, toMaxArgument, toMinArgument },
                rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }
    }
}