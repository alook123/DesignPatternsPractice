namespace DesignPatterns.SingletonPattern;

public sealed class StaticSingleton
{
    static StaticSingleton() { }

    private StaticSingleton() { }

    private static readonly StaticSingleton _instance = new();

    public static StaticSingleton Instance => _instance;

    public void SomeBusinessLogic()
    {
        // ...
    }
}

// 优点：
// 静态构造保证了线程安全。
// 实例在程序启动时就被创建（饿汉式）。
// 缺点：
// 无法延迟初始化，可能会浪费资源。