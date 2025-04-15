namespace DesignPatterns.SingletonPattern;

// Singleton 类定义了 `GetInstance` 方法，它可以作为构造函数的替代，并允许客户端反复访问此类的相同实例。
// EN：Singleton 应该始终是一个“密封”类，以防止通过外部类以及嵌套类进行类继承。
public sealed class Singleton
{
    // Singleton 的构造函数应该始终是私有的，以防止使用“new”运算符直接构造调用。
    private Singleton() { }

    // Singleton 实例存储在静态字段中。
    // 有多种初始化此字段的方法，它们各有优缺点。
    // 在本例中，我们将展示这些方法中最简单的一种，然而，这种方法在多线程程序中实际上并不适用。
    private static Singleton? _instance;

    // 这是控制对Singleton实例访问的静态方法。
    // 在第一次运行时，它创建一个Singleton对象并将其放置到静态字段中。
    // 在随后的运行中，它返回存储在静态字段中的现有客户端对象。
    public static Singleton GetInstance() => _instance ??= new Singleton();
}
// 优点：
// 代码简单易懂。
// 只在第一次调用时创建实例，节省资源。
// 适用于单线程环境。
// 缺点：
// 线程不安全。
// 如果多个线程同时访问 `GetInstance` 方法，可能会创建多个实例。
// 这可能会导致不必要的资源浪费和潜在的错误。