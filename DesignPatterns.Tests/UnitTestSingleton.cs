using DesignPatterns.SingletonPattern;

namespace DesignPatterns.Tests;

public class UnitTestSingleton
{
    [Fact]
    public void TestSingleton()
    {
        Singleton s1 = Singleton.GetInstance();
        Singleton s2 = Singleton.GetInstance();

        if (s1 == s2)
        {
            Console.WriteLine("Singleton works, both variables contain the same instance.");
        }
        else
        {
            Console.WriteLine("Singleton failed, variables contain different instances.");
        }
    }

    [Fact]
    public void TestThreadSafeSingleton()
    {
        Console.WriteLine(
            "{0}\n{1}\n\n{2}\n",
            "If you see the same value, then singleton was reused (yay!)",
            "If you see different values, then 2 singletons were created (booo!!)",
            "RESULT:"
        );

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

}
