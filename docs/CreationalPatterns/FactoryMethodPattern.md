# 工厂方法模式 Factory Method Pattern

## 概述

定义一个创建对象的接口，但让子类决定实例化哪个类。

## 核心思想

- **问题**: 直接 new 对象导致代码耦合度高
- **解决**: 通过工厂方法创建对象，具体创建逻辑由子类实现

## 代码结构

```csharp
// 抽象创建者
public abstract class Creator
{
    public abstract IProduct FactoryMethod();
}

// 具体创建者
public class Creator1 : Creator
{
    public override IProduct FactoryMethod() => new Product1();
}

public class Creator2 : Creator
{
    public override IProduct FactoryMethod() => new Product2();
}

// 产品接口
public interface IProduct
{
    string Operation();
}

// 具体产品
public class Product1 : IProduct
{
    public string Operation() => "{Product1 的结果}";
}
```

## 使用场景

1. **框架设计**: 让用户扩展框架的内部组件
2. **数据库驱动**: 不同数据库需要不同的连接对象  
3. **UI组件**: 不同平台需要不同的按钮实现
4. **日志系统**: 文件日志、数据库日志、网络日志

## 面试要点

### 优点

- 遵循开闭原则，易于扩展新产品
- 避免直接依赖具体类
- 符合单一职责原则

### 缺点

- 增加了代码复杂度
- 每添加一个产品就需要一个创建者

### 与其他模式区别

- **vs 简单工厂**: 工厂方法用继承，简单工厂用参数
- **vs 抽象工厂**: 工厂方法创建一种产品，抽象工厂创建产品族
- **vs Builder**: 工厂方法一步创建，Builder分步构建

### 开闭原则体现

- **对扩展开放**: 新增产品只需新增Creator子类
- **对修改封闭**: 不需要修改现有代码

## 记忆口诀

**"子类决定造什么"** - Factory Method 让子类决定创建哪种对象
