using DesignPatterns.SingletonPattern;

namespace DesignPatterns.Tests;

public class UnitTestSingleton
{
    [Fact]
    public void TestSingleton()
    {
        Singleton s1 = Singleton.GetInstance();
        Singleton s2 = Singleton.GetInstance();
        Assert.Equal(s1, s2);
    }

    [Fact]
    public async Task TestSingletonAsync()
    {
        Singleton? s1 = null;
        Singleton? s2 = null;

        async Task RunTasksAsync()
        {
            var task1 = Task.Run(() => s1 = Singleton.GetInstance());
            var task2 = Task.Run(() => s2 = Singleton.GetInstance());

            await Task.WhenAll(task1, task2);
        }
        await RunTasksAsync();
        Assert.Equal(s1, s2);
    }

    [Fact]
    public async Task TestStaticSingletonAsync()
    {
        StaticSingleton? s1 = null;
        StaticSingleton? s2 = null;

        async Task RunTasksAsync()
        {
            var task1 = Task.Run(() => s1 = StaticSingleton.GetInstance());
            var task2 = Task.Run(() => s2 = StaticSingleton.GetInstance());

            await Task.WhenAll(task1, task2);
        }
        await RunTasksAsync();
        Assert.Equal(s1, s2);
    }

    [Fact]
    public async Task TestThreadSafeSingletonAsync()
    {
        ThreadSafeSingleton? s1 = null;
        ThreadSafeSingleton? s2 = null;

        async Task RunTasksAsync()
        {
            var task1 = Task.Run(() => s1 = ThreadSafeSingleton.GetInstance());
            var task2 = Task.Run(() => s2 = ThreadSafeSingleton.GetInstance());

            await Task.WhenAll(task1, task2);
        }
        await RunTasksAsync();
        Assert.Equal(s1, s2);

    }

    [Fact]
    public async Task TestLazySingletonAsync()
    {
        LazySingleton? s1 = null;
        LazySingleton? s2 = null;

        async Task RunTasksAsync()
        {
            var task1 = Task.Run(() => s1 = LazySingleton.GetInstance());
            var task2 = Task.Run(() => s2 = LazySingleton.GetInstance());

            await Task.WhenAll(task1, task2);
        }
        await RunTasksAsync();
        Assert.Equal(s1, s2);
    }
}
