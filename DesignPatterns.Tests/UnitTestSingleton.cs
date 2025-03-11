using DesignPatterns.SingletonPattern;

namespace DesignPatterns.Tests;

public class UnitTestSingleton
{
    [Fact]
    public void TestSingleton()
    {
        Singleton s1 = Singleton.GetInstance();
        Console.WriteLine(s1.Value);
        Singleton s2 = Singleton.GetInstance();
        Console.WriteLine(s2.Value);

        if (s1 == s2)
        {
            Console.WriteLine("Singleton 有效，两个变量包含相同的实例。");
        }
        else
        {
            Console.WriteLine("Singleton 失败，两个变量包含不同的实例。");
        }
    }

    [Fact]
    public void TestThreadSafeSingleton()
    {
        Console.WriteLine($"{"如果您看到相同的值，则单例被重用了（耶！）"}\n{"如果您看到不同的值，则表示创建了 2 个单例（嘘！！）"}\n\n{"结果:"}\n");

        Thread process1 = new(() =>
        {
            ThreadSafeSingleton singleton = ThreadSafeSingleton.GetInstance("FOO");
            Console.WriteLine(singleton.Value);
        });
        Thread process2 = new(() =>
        {
            ThreadSafeSingleton singleton = ThreadSafeSingleton.GetInstance("BAR");
            Console.WriteLine(singleton.Value);
        });

        process1.Start();
        process2.Start();

        process1.Join();
        process2.Join();
    }

    [Fact]
    public void TestLazySingleton()
    {
        Console.WriteLine($"{"如果您看到相同的值，则单例被重用了（耶！）"}\n{"如果您看到不同的值，则表示创建了 2 个单例（嘘！！）"}\n\n{"结果:"}\n");

        Thread process1 = new(() =>
        {
            LazySingleton singleton = LazySingleton.Instance;
            singleton.Value = "FOO";
            Console.WriteLine(singleton.Value);
        });
        Thread process2 = new(() =>
        {
            LazySingleton singleton = LazySingleton.Instance;
            singleton.Value = "BAR";
            Console.WriteLine(singleton.Value);
        });

        process1.Start();
        process2.Start();

        process1.Join();
        process2.Join();
    }
}
