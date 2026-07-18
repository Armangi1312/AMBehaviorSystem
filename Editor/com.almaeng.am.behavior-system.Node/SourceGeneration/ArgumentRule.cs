using System;
using System.Linq;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public class ArgumentRule
    {
        public Argument TargetArgument { get; }
        public Type[] ConditionTypes { get; }

        public Type[] ExpectedTypes { get; }
        public Type Out { get; }

        public string Template { get; }

        public ArgumentRule(Argument targetArgument, Type[] conditionTypes, Type[] expectedTypes, Type @out, string template)
        {
            TargetArgument = targetArgument;
            ConditionTypes = conditionTypes;
            ExpectedTypes = expectedTypes;
            Out = @out;
            Template = template;
        }

        public bool IsMatched()
        {
            if (TargetArgument == null || ConditionTypes == null) return false;

            return ConditionTypes.Contains(TargetArgument.Type);
        }
    }
}
