# 抽象工厂模式 Abstract Factory Pattern

## 概述

提供一个创建相关或依赖对象族的接口，而无需指定它们具体的类。

## 核心思想

- **问题**: 需要创建一系列相关的产品对象，但不想依赖具体类
- **解决**: 定义抽象工厂接口，每个具体工厂创建一个产品族

## 代码结构

```csharp
// 抽象工厂接口
public interface IAbstractFactory
{
    IAbstractProductA CreateProductA();
    IAbstractProductB CreateProductB();
}

// 具体工厂1 - 创建产品族1
public class ConcreteFactory1 : IAbstractFactory
{
    public IAbstractProductA CreateProductA() => new ConcreteProductA1();
    public IAbstractProductB CreateProductB() => new ConcreteProductB1();
}

// 具体工厂2 - 创建产品族2
public class ConcreteFactory2 : IAbstractFactory
{
    public IAbstractProductA CreateProductA() => new ConcreteProductA2();
    public IAbstractProductB CreateProductB() => new ConcreteProductB2();
}

// 抽象产品A
public interface IAbstractProductA
{
    string UsefulFunctionA();
}

// 具体产品A1
public class ConcreteProductA1 : IAbstractProductA
{
    public string UsefulFunctionA() => "The result of the product A1.";
}
```

## 使用场景

1. **UI主题系统**: 创建不同风格的按钮、输入框、面板
2. **数据库访问**: MySQL工厂、SQLServer工厂，创建连接、命令、适配器
3. **游戏开发**: 不同种族的建筑、兵种、技能
4. **跨平台开发**: Windows工厂、Mac工厂，创建窗口、菜单、对话框

## 面试要点

### 优点

- 保证产品族的一致性
- 符合开闭原则，易于扩展新的产品族
- 客户端与具体产品解耦

### 缺点

- 增加新产品类型困难（需要修改所有工厂）
- 代码复杂度较高

### 与其他模式区别

- **vs 工厂方法**: 抽象工厂创建产品族，工厂方法创建单一产品
- **vs 建造者**: 抽象工厂一步创建，建造者分步构建
- **vs 原型**: 抽象工厂通过继承，原型通过复制

### 产品族 vs 产品等级

- **产品族**: 同一工厂创建的所有产品（如：苹果手机+苹果电脑）
- **产品等级**: 不同工厂创建的同类产品（如：苹果手机+华为手机）

## 记忆口诀

**"一家工厂造全套"** - Abstract Factory 一个工厂创建一套相关产品
