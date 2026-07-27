using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Condition")]
    public class ConditionNode : BasePipelineNode
    {
        public override string name => "Condition";

        [Input] public BooleanPort Condition;

        [Output] public PipelineFlowPort True;
        [Output] public PipelineFlowPort False;

        protected override void Process()
        {
            SourceContext context = ((NodeGraph)graph).SourceContext;

            (Type Type, string Name) condition = NodeUtilities.GetInputVariable(nameof(Condition), context, this);

            List<Statement> trueStatements = new();
            List<Statement> falseStatements = new();

            Expression expression = new(new Argument(condition.Type, condition.Name), new ExpressionRule("#", typeof(bool), ArgumentConstraint.OfFixedType(0, typeof(bool))));

            IfStatement ifStatement = new(expression, trueStatements, falseStatements);

            context.InvokeStatements.Add(ifStatement);
        }
    }
}