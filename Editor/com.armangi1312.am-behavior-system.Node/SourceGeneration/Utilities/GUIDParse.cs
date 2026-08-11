namespace AMBehaviorSystem.Node.SourceGeneration.Utilities
{
    internal static class GUIDParse
    {
        public static string GetGUIDParse(string guid) => guid.Replace("-", "_");
    }
}
