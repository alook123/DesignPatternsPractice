using DesignPatterns.CreationalPatterns.SingletonPattern;
using System.Collections.Concurrent;

namespace DesignPatterns.Tests;

public class UnitTestSingleton
{
    [Fact]
    public async Task TestNestedSingletonAsync()
    {
        const int taskCount = 1000;
        ConcurrentBag<NestedSingleton?> instances = [];

        Task CreateInstanceAsync(int index)
        {
            instances.Add(NestedSingleton.Instance);
            return Task.CompletedTask;
        }

        IEnumerable<Task> tasks = Enumerable
            .Range(0, taskCount)
            .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

        await Task.WhenAll(tasks);

        Assert.NotNull(instances.First());
        for (int i = 1; i < taskCount; i++)
        {
            Assert.True(instances.TryTake(out NestedSingleton? instance));
            Assert.NotNull(instance);
            Assert.Equal(instance, instances.First());
        }
    }

    [Fact]
    public async Task TestLazySingletonAsync()
    {
        const int taskCount = 1000;
        ConcurrentBag<LazySingleton?> instances = [];

        Task CreateInstanceAsync(int index)
        {
            instances.Add(LazySingleton.Instance);
            return Task.CompletedTask;
        }

        IEnumerable<Task> tasks = Enumerable
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