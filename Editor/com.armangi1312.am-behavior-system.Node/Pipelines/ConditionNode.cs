using AMBehaviorSystem.Node.Ports;
using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using AMBehaviorSystem.Node.SourceGeneration.Traversal;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Node.Pipelines
{
    [Serializable]
    [NodeMenuItem("Pipelines/Condition")]
    public class ConditionNode : BasePipelineNode, IBranchingSourceGenerationNode
    {
        public override string name => "Condition";

        [Input] public BooleanPort Condition;

        [Output] public PipelineFlowPort True;
        [Output] public PipelineFlowPort False;

        private const string TrueFieldName = "True";
        private const string FalseFieldName = "False";

        public void Generate(SourceContext context, GraphTraversal traversal)
        {
            (Type Type, string Name) condition = NodeUtilities.GetInputVariable(nameof(Condition), context, this);
            
            Argument argument = new(condition.Type, condition.Name);
            
            ExpressionRule rule = new("#", typeof(bool), ArgumentConstraint.OfFixedType(0, typeof(bool)));
            
            Expression conditionExpression = new(argument, rule);
            
            BaseNode trueEntry = GetNextNode(TrueFieldName);
            BaseNode falseEntry = GetNextNode(FalseFieldName);
            
            List<Statement> trueBody = traversal.GenerateBranch(trueEntry);
            List<Statement> falseBody = traversal.GenerateBranch(falseEntry);
            
            IfStatement statement = new(conditionExpression, trueBody, falseBody);
            
            context.InvokeStatements.Add(statement);
        }

        private BaseNode GetNextNode(string outputFieldName)
        {
            NodePort port = outputPorts.Find(p => p.fieldName == outputFieldName);
            if(port == null)
                return null;

            List<SerializableEdge> edges = port.GetEdges();
            if(edges.Count == 0)
                return null;

            return edges[0].inputNode;
        }
    }
}