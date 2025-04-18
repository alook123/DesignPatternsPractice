using System.Collections.Concurrent;
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
        const int taskCount = 1000;
        ConcurrentBag<Singleton?> instances = [];

        Task CreateInstanceAsync(int index)
        {
            instances.Add(Singleton.GetInstance());
            return Task.CompletedTask;
        }

        var tasks = Enumerable
            .Range(0, taskCount)
            .Select(i => Task.Run(async () => await CreateInstanceAsync(i)))
            .ToArray();

        await Task.WhenAll(tasks);

        Assert.NotNull(instances.First());
        for (int i = 1; i < taskCount; i++)
        {
            Assert.True(instances.TryTake(out Singleton? instance));
            Assert.NotNull(instance);
            Assert.Equal(instance, instances.First());
        }
    }

    [Fact]
    public async Task TestStaticSingletonAsync()
    {
        const int taskCount = 1000;
        ConcurrentBag<StaticSingleton?> instances = [];

        Task CreateInstanceAsync(int index)
        {
            instances.Add(StaticSingleton.GetInstance());
            return Task.CompletedTask;
        }

        var tasks = Enumerable
            .Range(0, taskCount)
            .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

        await Task.WhenAll(tasks);

        Assert.NotNull(instances.First());
        for (int i = 1; i < taskCount; i++)
        {
            Assert.True(instances.TryTake(out StaticSingleton? instance));
            Assert.NotNull(instance);
            Assert.Equal(instance, instances.First());
        }
    }

    [Fact]
    public async Task TestThreadSafeSingletonAsync()
    {
        const int taskCount = 1000;
        ConcurrentBag<ThreadSafeSingleton?> instances = [];

        Task CreateInstanceAsync(int index)
        {
            instances.Add(ThreadSafeSingleton.GetInstance());
            return Task.CompletedTask;
        }

        var tasks = Enumerable
            .Range(0, taskCount)
            .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

        await Task.WhenAll(tasks);

        Assert.NotNull(instances.First());
        for (int i = 1; i < taskCount; i++)
        {
            Assert.True(instances.TryTake(out ThreadSafeSingleton? instance));
            Assert.NotNull(instance);
            Assert.Equal(instance, instances.First());
        }
    }

    [Fact]
    public async Task TestLazySingletonAsync()
    {
        const int taskCount = 1000;
        ConcurrentBag<LazySingleton?> instances = [];

        async Task CreateInstanceAsync(int index)
        {
            instances.Add(await LazySingleton.GetInstance());
        }

        var tasks = Enumerable
            .Range(0, taskCount)
            .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

        await Task.WhenAll(tasks);

        Assert.NotNull(instances.First());
        for (int i = 1; i < taskCount; i++)
        {
            Assert.True(instances.TryTake(out LazySingleton? instance));
            Assert.NotNull(instance);
            Assert.Equal(instance, instances.First());
        }
    }
}
