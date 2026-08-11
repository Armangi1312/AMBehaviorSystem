using System;

namespace AMBehaviorSystem.Node.SourceGeneration.Expressions
{
    public readonly struct ArgumentConstraint
    {
        public int ArgumentIndex { get; }
        public Constraint Kind { get; }
        public ArgumentCategory Category { get; }
        public Type FixedType { get; }
        public int GroupId { get; }

        private ArgumentConstraint(int argumentIndex, Constraint kind, ArgumentCategory category, Type fixedType, int groupId)
        {
            ArgumentIndex = argumentIndex;
            Kind = kind;
            Category = category;
            FixedType = fixedType;
            GroupId = groupId;
        }

        public static ArgumentConstraint OfCategory(int argumentIndex, ArgumentCategory category)
        {
            return new(argumentIndex, Constraint.Category, category, null, 0);
        }

        public static ArgumentConstraint OfFixedType(int argumentIndex, Type fixedType)
        {
            return new(argumentIndex, Constraint.Fixed, ArgumentCategory.None, fixedType, 0);
        }

        public static ArgumentConstraint OfSameGroup(int argumentIndex, int groupId)
        {
            return new(argumentIndex, Constraint.SameAsGroup, ArgumentCategory.None, null, groupId);
        }

        public enum Constraint
        {
            Category,
            Fixed,
            SameAsGroup
        }
    }
}
