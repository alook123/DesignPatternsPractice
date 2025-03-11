namespace DesignPatterns.SingletonPattern;

public sealed class LazySingleton
{
    private LazySingleton() { }

    // 使用 Lazy<T> 类型来实现线程安全的延迟初始化。
    private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());

    public static LazySingleton Instance => _instance.Value;

    // 我们将使用此属性来证明我们的 Singleton 确实有效。
    public string Value { get; set; } = null!;
}

// 优点：
// Lazy<T> 内置线程安全，简化了代码。
// 延迟初始化，只有第一次访问时才创建实例。
// 语法简洁，更现代。
// 缺点：
// 依赖于 Lazy<T>，但在.NET 中非常推荐。
