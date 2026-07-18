using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration;
using GraphProcessor;
using System;

namespace AMBehaviorSystem.Node.Math.Advanced
{
    [Serializable]
    [NodeMenuItem("Math/Advanced/Abs")]
    public class AbsNode : BaseNode, IMathNode
    {
        public override string name => "Abs";

        [Input] public NumberPort In;

        [Output] public NumberPort Out;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) @in = NodeUtilities.GetInputVariable(nameof(In), context, this);

            string name = $"abs_{GUIDParse.GetGUIDParse(GUID)}";
            Argument argument = new(@in.Type, @in.Name);
            Expression expression = new(new[] { argument }, new[] { typeof(ScalarFilter) }, @in.Type, "Math.Abs(#)");
            DeclarationStatement statement = new(@in.Type, name, expression);

            context.InvokeStatements.Add(statement);
            context.OutputLocals[GUID] = (@in.Type, name);
        }
    }
}
