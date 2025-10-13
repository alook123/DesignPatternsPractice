# 建造者模式 (Builder Pattern)

## 概述

建造者模式是一种创建型设计模式，它允许你分步骤地构造复杂对象。该模式将对象构造代码从产品类中抽取出来，并将其放在一个名为建造者的独立对象中。

## 核心概念

### 问题解决

- **复杂对象构造**：当对象有很多可选的配置参数时
- **构造过程复杂**：需要多个步骤才能创建完整对象
- **产品变体**：同样的构造过程需要创建不同配置的产品

### 关键组件

- **Builder接口**：定义构造步骤的抽象接口
- **ConcreteBuilder**：实现Builder接口的具体建造者
- **Product**：被构造的复杂对象
- **Director**：指挥者，定义构造的顺序和步骤

## 代码实现

### 1. Builder接口定义

```csharp
// Builder 接口指定了创建 Product 对象不同部分的方法
public interface IBuilder
{
    void BuildPartA();
    void BuildPartB();
    void BuildPartC();
}
```

### 2. 具体建造者实现

```csharp
public class ConcreteBuilder : IBuilder
{
    private Product _product = new();

    public ConcreteBuilder()
    {
        this.Reset();
    }

    public void Reset()
    {
        this._product = new();
    }

    // 所有生产步骤都使用相同的产品实例
    public void BuildPartA()
    {
        this._product.Add("PartA1");
    }

    public void BuildPartB()
    {
        this._product.Add("PartB1");
    }

    public void BuildPartC()
    {
        this._product.Add("PartC1");
    }

    // 获取最终产品并重置建造者
    public Product GetProduct()
    {
        Product result = this._product;
        this.Reset();
        return result;
    }
}
```

### 3. 产品类

```csharp
public class Product
{
    private readonly List<object> _parts = [];

    public void Add(string part)
    {
        _parts.Add(part);
    }

    public string ListParts()
    {
        if (_parts.Count == 0)
        {
            return "Product parts: \n";
        }

        string str = string.Empty;
        for (int i = 0; i < this._parts.Count; i++)
        {
            str += _parts[i] + ", ";
        }
        str = str.Remove(str.Length - 2); // removing last ", "
        return "Product parts: " + str + "\n";
    }
}
```

### 4. 指挥者类

```csharp
public class Director(IBuilder builder)
{
    public IBuilder Builder { private get; set; } = builder;

    // Director 可以使用相同的构建步骤构建多个产品变体
    public void BuildMinimalViableProduct()
    {
        this._builder.BuildPartA();
    }

    public void BuildFullFeaturedProduct()
    {
        this._builder.BuildPartA();
        this._builder.BuildPartB();
        this._builder.BuildPartC();
    }
}
```

## 测试用例

### 测试1：逐步构建产品

```csharp
[Fact]
public void TestBuilder_CanBuildProductStepByStep()
{
    // Arrange - 创建建造者
    ConcreteBuilder builder = new();

    // Act - 逐步构建产品
    builder.BuildPartA();
    builder.BuildPartB();
    builder.BuildPartC();
    Product product = builder.GetProduct();

    // Assert - 验证产品包含所有部件
    string parts = product.ListParts();
    Assert.Contains("PartA1", parts);
    Assert.Contains("PartB1", parts);
    Assert.Contains("PartC1", parts);
}
```

### 测试2：指挥者构建不同产品变体

```csharp
[Fact]
public void TestDirector_CanBuildDifferentProductVariants()
{
    // Arrange - 创建建造者和指挥者
    ConcreteBuilder builder = new();
    Director director = new(builder);

    // Act & Assert - 构建最小可行产品
    director.BuildMinimalViableProduct();
    Product mvpProduct = builder.GetProduct();
    string mvpParts = mvpProduct.ListParts();
    
    Assert.Contains("PartA1", mvpParts);
    Assert.DoesNotContain("PartB1", mvpParts);
    Assert.DoesNotContain("PartC1", mvpParts);

    // Act & Assert - 构建完整功能产品
    director.BuildFullFeaturedProduct();
    Product fullProduct = builder.GetProduct();
    string fullParts = fullProduct.ListParts();
    
    Assert.Contains("PartA1", fullParts);
    Assert.Contains("PartB1", fullParts);
    Assert.Contains("PartC1", fullParts);
}
```

### 测试3：建造者重置机制

```csharp
[Fact]
public void TestBuilder_GetProductResetsBuilder()
{
    // Arrange
    ConcreteBuilder builder = new();
    
    // Act - 构建第一个产品
    builder.BuildPartA();
    Product firstProduct = builder.GetProduct();
    
    // Act - 构建第二个产品
    builder.BuildPartB();
    Product secondProduct = builder.GetProduct();

    // Assert - 验证产品是独立的（获取产品后建造者被重置）
    string firstParts = firstProduct.ListParts();
    string secondParts = secondProduct.ListParts();
    
    Assert.Contains("PartA1", firstParts);
    Assert.DoesNotContain("PartB1", firstParts);
    
    Assert.Contains("PartB1", secondParts);
    Assert.DoesNotContain("PartA1", secondParts);
}
```

## 使用场景

### 适用场景

1. **复杂对象构造**：对象有很多可选参数和配置
2. **多种产品变体**：需要创建同一产品家族的不同变体
3. **构造过程复杂**：对象创建需要多个步骤
4. **构造顺序重要**：构建步骤的顺序影响最终结果

### 实际应用示例

```csharp
// SQL查询建造者
public class SqlQueryBuilder : IQueryBuilder
{
    private string _select = "";
    private string _from = "";
    private string _where = "";
    private string _orderBy = "";

    public IQueryBuilder Select(string fields)
    {
        _select = $"SELECT {fields}";
        return this;
    }

    public IQueryBuilder From(string table)
    {
        _from = $" FROM {table}";
        return this;
    }

    public IQueryBuilder Where(string condition)
    {
        _where = $" WHERE {condition}";
        return this;
    }

    public IQueryBuilder OrderBy(string field)
    {
        _orderBy = $" ORDER BY {field}";
        return this;
    }

    public string Build()
    {
        return _select + _from + _where + _orderBy;
    }
}

// 使用示例
var query = new SqlQueryBuilder()
    .Select("name, age")
    .From("users")
    .Where("age > 18")
    .OrderBy("name")
    .Build();
// 结果: "SELECT name, age FROM users WHERE age > 18 ORDER BY name"
```

```csharp
// HTTP请求建造者
public class HttpRequestBuilder
{
    private string _url = "";
    private string _method = "GET";
    private Dictionary<string, string> _headers = new();
    private string _body = "";

    public HttpRequestBuilder Url(string url)
    {
        _url = url;
        return this;
    }

    public HttpRequestBuilder Method(string method)
    {
        _method = method;
        return this;
    }

    public HttpRequestBuilder AddHeader(string key, string value)
    {
        _headers[key] = value;
        return this;
    }

    public HttpRequestBuilder Body(string body)
    {
        _body = body;
        return this;
    }

    public HttpRequest Build()
    {
        return new HttpRequest(_url, _method, _headers, _body);
    }
}
```

## 关键实现要点

### 1. 流式接口（Fluent Interface）

```csharp
public class FluentBuilder : IBuilder
{
    public FluentBuilder BuildPartA()
    {
        // 构建逻辑
        return this;
    }

    public FluentBuilder BuildPartB()
    {
        // 构建逻辑
        return this;
    }

    // 支持链式调用
    // builder.BuildPartA().BuildPartB().BuildPartC();
}
```

### 2. 建造者重置

```csharp
public Product GetProduct()
{
    Product result = this._product;
    this.Reset(); // 重要：获取产品后重置建造者
    return result;
}
```

### 3. Director的作用

```csharp
// Director封装了构建的复杂逻辑
public void BuildGamingComputer()
{
    _builder.BuildHighEndCPU();
    _builder.BuildGamingGPU();
    _builder.Build32GBRAM();
    _builder.BuildSSDStorage();
}

public void BuildOfficeComputer()
{
    _builder.BuildMidRangeCPU();
    _builder.BuildIntegratedGPU();
    _builder.Build8GBRAM();
    _builder.BuildHDDStorage();
}
```

## 优势与劣势

### ✅ 优势

1. **分步构造**：可以分步骤创建复杂对象
2. **代码复用**：同样的构造代码可以创建不同的产品变体
3. **单一职责**：构造代码从产品类中分离
4. **精细控制**：可以精确控制构造过程

### ❌ 劣势

1. **代码复杂性**：增加了多个新类
2. **使用场景局限**：只适用于复杂对象的构造
3. **内存开销**：每个建造者都要维护产品状态
4. **学习成本**：相比简单构造函数更复杂

## 与其他模式的关系

- **工厂方法**：建造者专注于分步构造，工厂方法专注于一步创建
- **抽象工厂**：建造者可以使用抽象工厂来获取组件
- **组合模式**：建造者常用来构造组合模式的复杂树形结构
- **原型模式**：建造者创建新对象，原型克隆现有对象

## 最佳实践

### ✅ 推荐做法

1. **使用流式接口**：提供更好的用户体验
2. **验证构造参数**：在Build()方法中验证必需参数
3. **提供默认值**：为可选参数提供合理的默认值
4. **线程安全考虑**：如果需要多线程使用，确保建造者的线程安全

### ❌ 避免做法

1. **过度设计**：不要为简单对象使用建造者模式
2. **忘记重置**：获取产品后忘记重置建造者状态
3. **缺少验证**：不验证构造参数的有效性
4. **混乱的接口**：建造步骤的顺序应该清晰合理

## 现代.NET中的应用

### StringBuilder

```csharp
var sb = new StringBuilder()
    .Append("Hello")
    .Append(" ")
    .Append("World");
string result = sb.ToString();
```

### ConfigurationBuilder

```csharp
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
```

### HttpClient with HttpRequestMessage

```csharp
var request = new HttpRequestMessage()
{
    Method = HttpMethod.Post,
    RequestUri = new Uri("https://api.example.com"),
    Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
};
```

---

*本文档基于 C# .NET 9.0 实现，展示了建造者模式的完整实现和测试用例。*
