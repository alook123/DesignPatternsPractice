namespace DesignPatterns.SingletonPattern;

public sealed class LazySingleton
{
    private LazySingleton() { }

    // 使用 Lazy<T> 类型来实现线程安全的延迟初始化。
    private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());

    public static LazySingleton GetInstance()
    {
        return _instance.Value;
    }
}

// 优点：
// Lazy<T> 内置线程安全，简化了代码。
// 延迟初始化，只有第一次访问时才创建实例。
// 语法简洁，更现代。
// 适用于多线程环境。
// 允许在运行时控制实例的创建顺序。
// 允许使用 Lazy<T> 的其他功能，如初始化选项、线程安全选项等。
// 缺点：
// 依赖于 Lazy<T>，但在.NET 中非常推荐。
// 可能会引入额外的内存开销，尤其是在高并发情况下。