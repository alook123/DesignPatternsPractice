namespace DesignPatterns.SingletonPattern;

using System.Threading;

// 这种 Singleton 实现被称为“双重检查锁”。
// 它在多线程环境中是安全的，并为 Singleton 对象提供了延迟初始化。
public sealed class ThreadSafeSingleton
{
    private ThreadSafeSingleton() { }

    private static ThreadSafeSingleton? _instance;

    // 我们现在有一个锁对象，它将用于在第一次访问 Singleton 时同步线程。
    private static readonly Lock _lock = new();

    public static ThreadSafeSingleton GetInstance(string value)
    {
        // 一旦实例准备就绪，就需要此条件来防止线程因锁而失败。
        if (_instance == null)
        {
            // 现在，想象一下程序刚刚启动。
            // 由于还没有 Singleton 实例，多个线程可以同时通过前面的条件并几乎同时到达这一点。
            // 其中第一个将获取锁并继续执行，而其余线程将在此处等待。
            lock (_lock)
            {
                // 第一个获取锁的线程到达此条件，进入内部并创建 Singleton 实例。
                // 一旦它离开锁块，可能一直在等待锁释放的线程可能会进入此部分。
                // 但由于 Singleton 字段已初始化，因此线程不会创建新对象。
                _instance ??= new ThreadSafeSingleton
                {
                    Value = value
                };
            }
        }
        return _instance;
    }

    // 我们将使用此属性来证明我们的 Singleton 确实有效。
    public string Value { get; set; } = null!;
}