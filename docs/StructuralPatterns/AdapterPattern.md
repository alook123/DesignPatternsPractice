# 适配器模式 Adapter Pattern

## 概述

将一个类的接口转换成客户希望的另外一个接口，使得原本由于接口不兼容而不能一起工作的类可以一起工作。

## 核心思想

- **问题**: 现有类的接口与所需接口不匹配
- **解决**: 创建适配器类，包装现有类并提供期望的接口

## 代码结构

```csharp
// 目标接口 - 客户期望的接口
public interface ITarget
{
    string GetRequest();
}

// 被适配者 - 需要适配的现有类
public class Adaptee
{
    public string GetSpecificRequest()
    {
        return "Specific request.";
    }
}

// 适配器 - 使Adaptee兼容ITarget接口
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public string GetRequest()
    {
        return $"This is '{_adaptee.GetSpecificRequest()}'";
    }
}
```

## 使用场景

1. **第三方库集成**: 将第三方库的接口适配到自己的系统
2. **遗留系统改造**: 让老系统适配新的接口标准
3. **数据格式转换**: XML转JSON，不同数据库的适配
4. **API版本兼容**: 旧版API适配新版接口

## 两种实现方式

### 1. 对象适配器（组合方式）✅

```csharp
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;  // 组合
    
    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }
}
```

### 2. 类适配器（继承方式）

```csharp
public class Adapter : Adaptee, ITarget  // 多重继承
{
    // 只有当 Adaptee 可继承且你需要直接复用其 protected 内容时可用
}
```

## 面试要点

### 优点

- 提高类的复用性
- 增加类的透明性和复用性
- 灵活性好，不会破坏原有系统

### 缺点

- 增加系统复杂性
- 增加代码阅读难度

### 与其他模式区别

- **vs 装饰者**: 适配器改变接口，装饰者增强功能
- **vs 外观模式**: 适配器是接口转换，外观是简化接口
- **vs 代理模式**: 适配器改变接口，代理保持接口不变

### 实际应用举例

```csharp
// 适配第三方支付接口
public interface IPayment
{
    bool Pay(decimal amount);
}

public class AlipayAdapter : IPayment
{
    private readonly AlipaySDK _alipay;
    
    public bool Pay(decimal amount)
    {
        return _alipay.ProcessPayment(amount.ToString());
    }
}
```

## 记忆口诀

**"插头转换器"** - Adapter 就像插头转换器，让不兼容的接口可以连接
