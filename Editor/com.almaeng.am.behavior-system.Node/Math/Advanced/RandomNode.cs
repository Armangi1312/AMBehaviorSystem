using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Random")]
    public class RandomNode : BaseNode, IMathNode
    {
        public override string name => "Random";

        [Input] public NumberPort Min;
        [Input] public NumberPort Max;
        public NumberType OutType;

        [Output] public NumberPort Out;

        public enum NumberType
        {
            Integer,
            Float,
            Double
        }

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) min = NodeUtilities.GetInputVariable(nameof(Min), context, this);
            (Type Type, string Name) max = NodeUtilities.GetInputVariable(nameof(Max), context, this);

            string name = $"random_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = ResolveOutType(OutType);

            Argument minArgument = new(min.Type, min.Name);
            Argument maxArgument = new(max.Type, max.Name);

            string template = ResolveTemplate(OutType);

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfCategory(0, ArgumentCategory.Scalar),
                ArgumentConstraint.OfCategory(1, ArgumentCategory.Scalar),
                ArgumentConstraint.OfSameGroup(0, 1),
                ArgumentConstraint.OfSameGroup(1, 1));

            Expression expression = new(new[] { minArgument, maxArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[PortKey.Of(GUID, nameof(Out))] = (outType, name);
        }

        private static Type ResolveOutType(NumberType numberType)
        {
            return numberType switch
            {
                NumberType.Integer => typeof(int),
                NumberType.Float => typeof(float),
                NumberType.Double => typeof(double),
                _ => throw new ArgumentOutOfRangeException(nameof(numberType), numberType, null)
            };
        }

        private static string ResolveTemplate(NumberType numberType)
        {
            return numberType switch
            {
                NumberType.Integer => "UnityEngine.Random.Range((int)(#), (int)(#))",
                NumberType.Float => "UnityEngine.Random.Range((float)(#), (float)(#))",
                NumberType.Double => "((double)UnityEngine.Random.Range((float)(#), (float)(#)))",
                _ => throw new ArgumentOutOfRangeException(nameof(numberType), numberType, null)
            };
        }
    }
}