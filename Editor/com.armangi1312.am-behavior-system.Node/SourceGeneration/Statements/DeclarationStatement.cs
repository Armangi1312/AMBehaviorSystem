using AMBehaviorSystem.Node.SourceGeneration.Expressions;
using AMBehaviorSystem.Node.SourceGeneration.Utilities;
using System;

namespace AMBehaviorSystem.Node.SourceGeneration.Statements
{
    public class DeclarationStatement : Statement
    {
        public AccessModifier Access { get; }
        public Type Type { get; }
        public string Name { get; }
        public Expression Value { get; }

        public DeclarationStatement(Type type, string name)
        {
            Access = AccessModifier.None;
            Type = type;
            Name = name;
            Value = null;
        }

        public DeclarationStatement(AccessModifier access, Type type, string name)
        {
            Access = access;
            Type = type;
            Name = name;
            Value = null;
        }

        public DeclarationStatement(Type type, string name, Expression value)
        {
            Access = AccessModifier.None;
            Type = type;
            Name = name;
            Value = value;
        }

        public DeclarationStatement(AccessModifier access, Type type, string name, Expression value)
        {
            Access = access;
            Type = type;
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            string access = GetAccessModifierString();
            string type = TypeUtilities.GetTypeAlias(Type);

            if (Value == null)
            {
                return $"{access}{type} {Name};";
            }

            return $"{access}{type} {Name} = {Value};";
        }

        private string GetAccessModifierString()
        {
            return Access switch
            {
                AccessModifier.Public => "public ",
                AccessModifier.Private => "private ",
                AccessModifier.Protected => "protected ",
                AccessModifier.Internal => "internal ",
                AccessModifier.ProtectedInternal => "protected internal ",
                AccessModifier.PrivateProtected => "private protected ",
                AccessModifier.None => string.Empty,
                _ => string.Empty
            };
        }

        public enum AccessModifier
        {
            Public,
            Protected,
            Private,
            Internal,
            ProtectedInternal,
            PrivateProtected,
            None
        }
    }
}