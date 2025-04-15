namespace DesignPatterns.SingletonPattern;

public sealed class StaticSingleton
{
    static StaticSingleton() { }

    private StaticSingleton() { }

    private static readonly StaticSingleton _instance = new();

    // public static StaticSingleton Instance => _instance;

    public static StaticSingleton GetInstance()
    {
        return _instance;
    }
}

// 优点：
// 静态构造保证了线程安全。
// 实例在程序启动时就被创建（饿汉式）。
// 适用于单线程和多线程环境。
// 由于实例在静态构造函数中创建，因此不会浪费资源。
// 语法简洁，易于理解。

// 缺点：
// 无法延迟初始化，可能会浪费资源。
// 如果实例创建时需要大量资源，可能会导致启动时间延长。
// 不能在运行时控制实例的创建顺序。