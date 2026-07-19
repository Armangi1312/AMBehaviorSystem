using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Logic")]
    public class LogicNode : BaseNode, IMathNode
    {
        public override string name => "Logic";

        [Input] public BooleanPort A;
        [Input] public BooleanPort B;

        [Output] public BooleanPort Out;

        public LogicType Logic;

        public enum LogicType
        {
            And,
            Or,
            Xor
        }

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) a = NodeUtilities.GetInputVariable(nameof(A), context, this);
            (Type Type, string Name) b = NodeUtilities.GetInputVariable(nameof(B), context, this);

            string name = $"logic_{GUIDParse.GetGUIDParse(GUID)}";
            Type outType = typeof(bool);

            Argument leftArgument = new(a.Type, a.Name);
            Argument rightArgument = new(b.Type, b.Name);

            string template = GetTemplate(Logic);

            ExpressionRule rule = new(template, outType,
                ArgumentConstraint.OfFixedType(0, typeof(bool)),
                ArgumentConstraint.OfFixedType(1, typeof(bool)));

            Expression expression = new(new[] { leftArgument, rightArgument }, rule);

            DeclarationStatement statement = new(outType, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (outType, name);
        }

        private static string GetTemplate(LogicType logic)
        {
            return logic switch
            {
                LogicType.And => "# && #",
                LogicType.Or => "# || #",
                LogicType.Xor => "# ^ #",
                _ => throw new ArgumentOutOfRangeException(nameof(logic), logic, null)
            };
        }
    }
}