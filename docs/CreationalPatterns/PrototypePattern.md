# 原型模式 Prototype Pattern

## 概述

通过复制现有对象来创建新对象，而不是通过 new 关键字创建。

## 核心思想

- **问题**: 复杂对象创建成本高，重复初始化浪费资源
- **解决**: 克隆现有对象，避免重复初始化

## 代码结构

```csharp
public interface IPrototype<T>
{
    T Clone();
}

public class Person : IPrototype<Person>
{
    public string Name { get; set; }
    public Address Address { get; set; }

    // 浅拷贝
    public Person Clone()
    {
        return (Person)this.MemberwiseClone();
    }

    // 深拷贝
    public Person DeepClone()
    {
        var clone = (Person)this.MemberwiseClone();
        clone.Address = new Address { Street = this.Address.Street };
        return clone;
    }
}
```

## 拷贝类型

- **浅拷贝**: 引用类型字段仍指向原对象
- **深拷贝**: 递归复制所有引用类型字段

## 使用场景

1. **游戏开发**: 复制敌人、道具等游戏对象
2. **文档编辑**: 复制格式相同的文档模板
3. **配置对象**: 基于现有配置创建新配置

## 面试要点

### 优点

- 避免重复初始化，提高性能
- 运行时动态创建对象

### 缺点

- 深拷贝实现复杂
- 循环引用问题需要特殊处理

### 与其他模式区别

- **vs 工厂模式**: 原型通过复制创建，工厂通过 new 创建
- **vs 建造者**: 原型基于现有对象，建造者从零构建

## 记忆口诀

**"复制粘贴造对象"** - Prototype 通过克隆现有对象创建新对象
