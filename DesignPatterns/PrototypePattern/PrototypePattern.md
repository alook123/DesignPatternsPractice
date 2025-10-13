# 原型模式 (Prototype Pattern)

## 概述

原型模式是一种创建型设计模式，它允许通过复制现有对象来创建新对象，而不需要依赖于它们的具体类。这个模式特别适用于创建复杂对象的成本较高时，可以通过克隆现有实例来避免昂贵的初始化操作。

## 核心概念

### 浅拷贝 vs 深拷贝

- **浅拷贝 (Shallow Copy)**: 创建新对象实例，但引用类型字段仍然指向原始对象的相同实例
- **深拷贝 (Deep Clone)**: 创建完全独立的副本，递归复制所有引用类型字段

## 代码实现

### 1. 原型接口定义

```csharp
public interface IPrototype<T>
{
    /// <summary>
    /// 浅拷贝 - 创建当前对象的浅表副本。
    /// 浅拷贝会复制值类型字段，但引用类型字段仍然指向原始对象。
    /// </summary>
    T ShallowCopy();

    /// <summary>
    /// 深拷贝 - 创建当前对象的完全独立副本。
    /// 深拷贝会递归复制所有字段，确保克隆体与原始对象没有任何共享引用。
    /// </summary>
    T DeepClone();
}
```

### 2. 主原型类实现

```csharp
public class Prototype(string name) : IPrototype<Prototype>
{
    public string Name { get; set; } = name;
    public List<string> Items { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
    public NestedObject Nested { get; set; } = new();

    // 浅拷贝实现
    public Prototype ShallowCopy()
    {
        return (Prototype)this.MemberwiseClone();
    }

    // 深拷贝实现
    public Prototype DeepClone()
    {
        Prototype clone = new(Name)
        {
            Items = [.. this.Items],                                          // 深拷贝集合
            Properties = new Dictionary<string, object>(this.Properties),      // 深拷贝字典
            Nested = this.Nested.DeepClone()                                 // 深拷贝嵌套对象
        };
        return clone;
    }
}
```

### 3. 嵌套对象实现

```csharp
public class NestedObject : IPrototype<NestedObject>
{
    public string Data { get; set; } = "Default Data";
    public int Counter { get; set; } = 0;

    public NestedObject ShallowCopy()
    {
        return new NestedObject
        {
            Data = this.Data,
            Counter = this.Counter
        };
    }

    public NestedObject DeepClone()
    {
        return ShallowCopy(); // 对于简单对象，浅拷贝等同于深拷贝
    }
}
```

## 测试用例

### 浅拷贝测试

```csharp
[Fact]
public void TestShallowCopy()
{
    // Arrange - 准备测试数据
    Prototype original = new("测试对象");
    original.Items.Add("item1");
    original.Properties["key1"] = "value1";
    original.Nested.Data = "original data";

    // Act - 执行浅拷贝
    Prototype clone = original.ShallowCopy();

    // Assert - 验证浅拷贝特性
    Assert.NotSame(original, clone);                    // ✅ 不同的对象实例
    Assert.Equal(original.Name, clone.Name);            // ✅ 值类型字段复制
    Assert.Same(original.Items, clone.Items);           // ✅ 引用相同的集合
    Assert.Same(original.Properties, clone.Properties); // ✅ 引用相同的字典
    Assert.Same(original.Nested, clone.Nested);         // ✅ 引用相同的嵌套对象
}
```

### 深拷贝测试

```csharp
[Fact]
public void TestDeepCopy()
{
    // Arrange - 准备测试数据
    Prototype original = new("原始");
    original.Items.Add("item1");
    original.Items.Add("item2");
    original.Properties["key1"] = "value1";
    original.Nested.Data = "original data";
    original.Nested.Counter = 42;

    // Act - 执行深拷贝
    Prototype clone = original.DeepClone();

    // Assert - 验证深拷贝特性
    Assert.NotSame(original, clone);                           // ✅ 不同的对象实例
    Assert.Equal(original.Name, clone.Name);                   // ✅ 相同的值
    Assert.Equal(original.Items, clone.Items);                 // ✅ 相同的集合内容
    Assert.NotSame(original.Items, clone.Items);               // ✅ 但是不同的集合实例
    Assert.Equal(original.Properties, clone.Properties);       // ✅ 相同的字典内容
    Assert.NotSame(original.Properties, clone.Properties);     // ✅ 但是不同的字典实例
    Assert.Equal(original.Nested.Data, clone.Nested.Data);     // ✅ 相同的嵌套对象内容
    Assert.NotSame(original.Nested, clone.Nested);             // ✅ 但是不同的嵌套对象实例
}
```

## 关键实现要点

### 1. MemberwiseClone() 的使用

```csharp
public Prototype ShallowCopy()
{
    return (Prototype)this.MemberwiseClone();
}
```

- `MemberwiseClone()` 是 .NET 中实现浅拷贝的标准方法
- 它会创建新的对象实例，但复制所有字段的引用

### 2. 深拷贝的递归实现

```csharp
public Prototype DeepClone()
{
    Prototype clone = new(Name)
    {
        Items = [.. this.Items],                          // 使用集合表达式创建新集合
        Properties = new Dictionary<string, object>(this.Properties), // 使用拷贝构造函数
        Nested = this.Nested.DeepClone()                 // 递归调用深拷贝
    };
    return clone;
}
```

## 使用场景

### 适用场景

1. **对象创建成本高昂**：当初始化对象需要大量资源时
2. **避免子类爆炸**：当需要创建大量相似对象的变体时
3. **运行时确定对象类型**：当需要在运行时创建对象而不知道具体类型时
4. **配置复杂的对象**：当对象有很多配置选项时

### 实际应用示例

```csharp
// 游戏开发中的角色模板
var warriorTemplate = new CharacterPrototype("战士")
{
    Health = 100,
    Strength = 50,
    Equipment = { "剑", "盾牌", "铠甲" }
};

// 快速创建多个战士
var warrior1 = warriorTemplate.DeepClone();
var warrior2 = warriorTemplate.DeepClone();

// 文档模板系统
var reportTemplate = new DocumentPrototype("季度报告")
{
    Sections = { "摘要", "数据分析", "结论" },
    Styles = { ["font"] = "Arial", ["size"] = 12 }
};

var q1Report = reportTemplate.DeepClone();
var q2Report = reportTemplate.DeepClone();
```

## 优势与劣势

### ✅ 优势

1. **解耦合性**：客户端代码不需要知道具体的类
2. **性能优化**：避免重复的复杂初始化操作
3. **灵活性**：可以在运行时动态创建对象
4. **简化配置**：通过克隆预配置的原型简化对象创建

### ❌ 劣势

1. **循环引用问题**：复杂对象的循环引用可能导致克隆困难
2. **深拷贝成本**：深拷贝可能消耗大量内存和处理时间
3. **实现复杂性**：需要为每个类实现克隆逻辑
4. **维护难度**：当类结构变化时需要同步更新克隆方法

## 与其他模式的关系

- **工厂模式**：原型模式可以作为工厂模式的替代方案
- **备忘录模式**：都涉及对象状态的保存和恢复
- **装饰器模式**：可以用原型模式创建装饰器的不同配置

## 最佳实践

1. **明确区分浅拷贝和深拷贝**：根据使用场景选择合适的拷贝方式
2. **处理循环引用**：在深拷贝中实现循环引用检测
3. **性能考虑**：对于大对象，考虑使用浅拷贝或延迟拷贝
4. **单元测试**：确保拷贝行为符合预期，特别是引用类型字段的处理

---

*本文档基于 C# .NET 9.0 实现，展示了原型模式的完整实现和测试用例。*
