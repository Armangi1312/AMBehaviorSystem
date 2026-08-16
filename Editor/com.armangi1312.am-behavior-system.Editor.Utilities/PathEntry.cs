using System;

namespace AMBehaviorSystem.Editor.Utilities
{
    public readonly struct PathEntry
    {
        public string Path { get; }
        public Type Type { get; }

        public PathEntry(string path, Type type)
        {
            Path = path;
            Type = type;
        }
    }
}
