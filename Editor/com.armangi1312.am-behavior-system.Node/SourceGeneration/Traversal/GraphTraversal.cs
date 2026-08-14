using AMBehaviorSystem.Node.SourceGeneration.Context;
using AMBehaviorSystem.Node.SourceGeneration.Statements;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AMBehaviorSystem.Node.SourceGeneration.Traversal
{
    public class GraphTraversal : BaseGraphProcessor
    {
        private const string NextFieldName = "Next";

        private List<BaseNode> nodes;
        private readonly SourceContext sourceContext;
        private readonly HashSet<BaseNode> visited = new();

        public GraphTraversal(BaseGraph graph, SourceContext context) : base(graph)
        {
            sourceContext = context;
        }

        public override void Run()
        {
            sourceContext.Clear();
            visited.Clear();

            foreach(BaseNode node in nodes)
            {
                if(visited.Contains(node))
                    continue;

                GenerateNode(node);
            }
        }

        public List<Statement> GenerateBranch(BaseNode startNode)
        {
            List<BaseNode> chain = CollectFlowChain(startNode);
            List<BaseNode> ordered = chain.OrderBy(n => n.computeOrder).ToList();

            List<Statement> outerSnapshot = new(sourceContext.InvokeStatements);
            sourceContext.InvokeStatements.Clear();

            foreach(BaseNode node in ordered)
            {
                if(visited.Contains(node))
                    continue;

                GenerateNode(node);
            }

            List<Statement> branchBody = new(sourceContext.InvokeStatements);

            sourceContext.InvokeStatements.Clear();
            sourceContext.InvokeStatements.AddRange(outerSnapshot);

            return branchBody;
        }

        private void GenerateNode(BaseNode node)
        {
            visited.Add(node);

            switch(node)
            {
                case IBranchingSourceGenerationNode branchingNode:
                branchingNode.Generate(sourceContext, this);
                break;
            
                case ISourceGenerationNode generationNode:
                generationNode.Generate(sourceContext);
                break;
            
                case INonGenerativeNode:
                break;
            
                default:
                throw new InvalidOperationException($"Node {node.name} ({node.GUID}) does not implement {nameof(ISourceNode)}.");
            }
        }

        private List<BaseNode> CollectFlowChain(BaseNode startNode)
        {
            List<BaseNode> chain = new();
            BaseNode currentNode = startNode;

            while(currentNode != null && !visited.Contains(currentNode))
            {
                chain.Add(currentNode);
                currentNode = GetNextFlowNode(currentNode, NextFieldName);
            }

            return chain;
        }

        private static BaseNode GetNextFlowNode(BaseNode node, string outputFieldName)
        {
            if(node == null)
                return null;

            NodePort port = node.outputPorts.Find(p => p.fieldName == outputFieldName);

            if(port == null)
                return null;

            List<SerializableEdge> edges = port.GetEdges();

            if(edges.Count == 0)
                return null;

            return edges[0].inputNode;
        }

        public override void UpdateComputeOrder()
        {
            nodes = graph.nodes.OrderBy(n => n.computeOrder).ToList();
        }
    }
}
