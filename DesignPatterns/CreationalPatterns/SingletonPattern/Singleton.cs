namespace DesignPatterns.CreationalPatterns.SingletonPattern;

public sealed class NestedSingleton
{
    private NestedSingleton() { }
    public static NestedSingleton Instance => Nested.Instance;
    private class Nested
    {
        internal static readonly NestedSingleton Instance = new();
    }
}

public sealed class LazySingleton
{
    private LazySingleton() { }
    private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());
    public static LazySingleton Instance => _instance.Value;
}