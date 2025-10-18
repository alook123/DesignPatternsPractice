# 建造者模式 Builder Pattern

## 概述

将复杂对象的构造过程与其表示分离，使得同样的构造过程可以创建不同的表示。

## 核心思想

- **问题**: 对象有很多可选参数，构造函数参数过多
- **解决**: 步骤化构建，每步设置不同属性

## 代码结构

```csharp
// 建造者接口
public interface IBuilder
{
    void BuildPartA();
    void BuildPartB();
    void BuildPartC();
}

// 具体建造者
public class ConcreteBuilder : IBuilder
{
    private Product _product = new();
    
    public void BuildPartA() => _product.Add("PartA1");
    public void BuildPartB() => _product.Add("PartB1");
    public void BuildPartC() => _product.Add("PartC1");
    
    public Product GetProduct()
    {
        Product result = _product;
        Reset();
        return result;
    }
}

// 指挥者（可选）
public class Director(IBuilder builder)
{
    public IBuilder Builder { private get; set; } = builder;
    
    public void BuildMinimalProduct() => Builder.BuildPartA();
    
    public void BuildFullProduct()
    {
        Builder.BuildPartA();
        Builder.BuildPartB();
        Builder.BuildPartC();
    }
}
```

## 使用场景

1. **SQL查询构建**: 动态拼接SQL语句
2. **HTTP请求**: 设置headers、参数、body
3. **配置对象**: 复杂配置的逐步构建
4. **UI组件**: 复杂界面的分步构建

## 面试要点

### 优点

- 分步构建，过程清晰
- 同一构建过程可创建不同对象
- 易于控制构建细节

### 缺点

- 增加了代码复杂度
- 创建的对象必须有共同点

### 与其他模式区别

- **vs 工厂模式**: Builder 分步构建，Factory 一步创建
- **vs 抽象工厂**: Builder 关注构建过程，抽象工厂关注产品族

### Director 的作用

- **可选组件**: 封装具体的构建算法
- **复用性**: 同一Director可用于不同Builder
- **客户端简化**: 隐藏复杂的构建步骤

## 记忆口诀

**"按步骤搭积木"** - Builder 分步骤构建复杂对象
