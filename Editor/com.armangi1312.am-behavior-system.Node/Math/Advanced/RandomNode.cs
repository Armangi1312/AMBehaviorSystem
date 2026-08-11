using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using UnityEngine;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Random")]
    public class RandomNode : BaseNode, IMathNode, ISourceGenerationNode
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

        public void Generate(SourceContext context)
        {
            (Type Type, string Name) min = NodeUtilities.GetInputVariable(nameof(Min), context, this);
            (Type Type, string Name) max = NodeUtilities.GetInputVariable(nameof(Max), context, this);

            string name = $"random_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = GetOutType(OutType);

            Argument minArgument = new(min.Type, min.Name);
            Argument maxArgument = new(max.Type, max.Name);

            string template = GetTemplate(OutType);

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

        private static Type GetOutType(NumberType numberType)
        {
            return numberType switch
            {
                NumberType.Integer => typeof(int),
                NumberType.Float => typeof(float),
                NumberType.Double => typeof(double),
                _ => throw new ArgumentOutOfRangeException(nameof(numberType), numberType, null)
            };
        }

        private static string GetTemplate(NumberType numberType)
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