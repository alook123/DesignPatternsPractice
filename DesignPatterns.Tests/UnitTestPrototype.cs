using DesignPatterns.CreationalPatterns.PrototypePattern;

namespace DesignPatterns.Tests;

public class UnitTestPrototype
{
    [Fact]
    public void TestShallowCopy()
    {
        // Arrange
        Prototype original = new("测试对象");
        original.Items.Add("item1");
        original.Properties["key1"] = "value1";
        original.Nested.Data = "original data";

        // Act
        Prototype clone = original.ShallowCopy();

        // Assert
        Assert.NotSame(original, clone); // 不同的对象实例
        Assert.Equal(original.Name, clone.Name); // 值类型字段复制
        Assert.Same(original.Items, clone.Items); // 浅拷贝：引用相同的集合
        Assert.Same(original.Properties, clone.Properties); // 浅拷贝：引用相同的字典
        Assert.Same(original.Nested, clone.Nested); // 浅拷贝：引用相同的嵌套对象
    }

    [Fact]
    public void TestDeepCopy()
    {
        // Arrange
        Prototype original = new("原始");
        original.Items.Add("item1");
        original.Items.Add("item2");
        original.Properties["key1"] = "value1";
        original.Nested.Data = "original data";
        original.Nested.Counter = 42;

        // Act
        Prototype clone = original.DeepClone();

        // Assert
        Assert.NotSame(original, clone); // 不同的对象实例
        Assert.Equal(original.Name, clone.Name); // 相同的值
        Assert.Equal(original.Items, clone.Items); // 相同的内容
        Assert.NotSame(original.Items, clone.Items); // 但是不同的集合实例
        Assert.Equal(original.Properties, clone.Properties); // 相同的内容
        Assert.NotSame(original.Properties, clone.Properties); // 但是不同的字典实例
        Assert.Equal(original.Nested.Data, clone.Nested.Data); // 相同的嵌套对象内容
        Assert.Equal(original.Nested.Counter, clone.Nested.Counter); // 相同的嵌套对象内容
        Assert.NotSame(original.Nested, clone.Nested); // 但是不同的嵌套对象实例
    }
}