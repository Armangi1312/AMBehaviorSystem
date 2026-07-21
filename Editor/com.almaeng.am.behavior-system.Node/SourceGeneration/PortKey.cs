using System;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    public readonly struct PortKey : IEquatable<PortKey>
    {
        public string NodeGUID { get; }
        public string Identifier { get; }

        private PortKey(string nodeGUID, string identifier)
        {
            NodeGUID = nodeGUID;
            Identifier = identifier;
        }

        public static PortKey Of(string nodeGUID, string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException("Identifier must not be null or empty. Use PortKey.Default for nodes without a named port.", nameof(identifier));

            return new PortKey(nodeGUID, identifier);
        }

        public static PortKey Default(string nodeGUID)
        {
            return new PortKey(nodeGUID, string.Empty);
        }

        public bool Equals(PortKey other) => NodeGUID == other.NodeGUID && Identifier == other.Identifier;
        public override bool Equals(object obj) => obj is PortKey other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(NodeGUID, Identifier);
    }
}
