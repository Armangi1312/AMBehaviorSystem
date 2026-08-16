using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AMBehaviorSystem.Node.SourceGeneration.Expressions
{
    public record ExpressionRule
    {
        public string Template { get; }
        public Type Out { get; }

        public IReadOnlyList<ArgumentConstraint> Constraints { get; }

        public ExpressionRule(string template, Type @out, params ArgumentConstraint[] constraints)
        {
            Template = template;
            Out = @out;
            Constraints = constraints.ToList();
        }

        public bool Match(Argument[] arguments)
        {
            Dictionary<int, Type> groupTypes = new();

            for (int i = 0; i < Constraints.Count; i++)
            {
                ArgumentConstraint constraint = Constraints[i];
                Type argumentType = arguments[constraint.ArgumentIndex].Type;

                bool isMatched = constraint.Kind switch
                {
                    ArgumentConstraint.Constraint.Category => MatchCategory(argumentType, constraint.Category),
                    ArgumentConstraint.Constraint.Fixed => constraint.FixedType.CanCast(argumentType),
                    ArgumentConstraint.Constraint.SameAsGroup => MatchGroup(constraint.GroupId, argumentType, groupTypes),
                    _ => false
                };

                if (!isMatched) return false;
            }

            return true;
        }

        private static bool MatchGroup(int groupId, Type argumentType, Dictionary<int, Type> groupTypes)
        {
            if (!groupTypes.TryGetValue(groupId, out Type groupType))
            {
                groupTypes[groupId] = argumentType;
                return true;
            }

            return argumentType.CanCast(groupType);
        }

        private static bool MatchCategory(Type type, ArgumentCategory category)
        {
            ArgumentCategory typeCategory = ArgumentCategory.None;

            if (type.IsInteger()) typeCategory |= ArgumentCategory.Integer;
            if (type.IsFloat()) typeCategory |= ArgumentCategory.Float;
            if (type.IsVector()) typeCategory |= ArgumentCategory.Vector;

            return (typeCategory & category) != 0;
        }

        public Type[] GetConstraintedTypes(Argument[] arguments)
        {
            Type[] expectedTypes = new Type[arguments.Length];
            Dictionary<int, List<int>> groupMembers = new();

            for (int i = 0; i < arguments.Length; i++)
            {
                expectedTypes[i] = arguments[i].Type;
            }

            foreach (ArgumentConstraint constraint in Constraints)
            {
                if (constraint.Kind != ArgumentConstraint.Constraint.SameAsGroup) continue;

                if (!groupMembers.TryGetValue(constraint.GroupId, out List<int> members))
                {
                    members = new List<int>();
                    groupMembers[constraint.GroupId] = members;
                }

                members.Add(constraint.ArgumentIndex);
            }

            foreach (List<int> members in groupMembers.Values)
            {
                Type[] memberTypes = members.Select(index => arguments[index].Type).ToArray();
                Type dominantType = TypeUtilities.GetCastingType(memberTypes);

                foreach (int index in members)
                {
                    expectedTypes[index] = dominantType;
                }
            }

            foreach (ArgumentConstraint constraint in Constraints)
            {
                if (constraint.Kind != ArgumentConstraint.Constraint.Fixed) continue;

                expectedTypes[constraint.ArgumentIndex] = constraint.FixedType;
            }

            return expectedTypes;
        }
    }
}
