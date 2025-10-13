# 单例模式 (Singleton Pattern)

## 概述

单例模式是一种创建型设计模式，它确保一个类只有一个实例，并提供全局访问点。这个模式在需要严格控制某些资源的访问时非常有用，比如数据库连接池、日志记录器、配置管理器等。

## 实现方式对比

### 1. 简单单例（非线程安全）

```csharp
public sealed class Singleton
{
    private Singleton() { }
    
    private static Singleton? _instance;

    public static Singleton GetInstance() => _instance ??= new Singleton();
}
```

**特点：**

- ✅ 代码简单易懂
- ✅ 延迟初始化，节省资源
- ❌ 线程不安全
- ❌ 多线程环境可能创建多个实例

### 2. 线程安全单例（双重检查锁）

```csharp
public sealed class ThreadSafeSingleton
{
    private ThreadSafeSingleton() { }
    
    private static ThreadSafeSingleton? _instance;
    private static readonly Lock _lock = new();

    public static ThreadSafeSingleton GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                _instance ??= new();
            }
        }
        return _instance;
    }
}
```

**特点：**

- ✅ 线程安全
- ✅ 延迟初始化
- ✅ 性能优化（双重检查）
- ⚠️ 代码稍复杂
- ⚠️ 锁可能影响性能

### 3. 静态单例（饿汉式）

```csharp
public sealed class StaticSingleton
{
    static StaticSingleton() { }
    private StaticSingleton() { }
    
    private static readonly StaticSingleton _instance = new();

    public static StaticSingleton GetInstance() => _instance;
}
```

**特点：**

- ✅ 线程安全（静态构造函数保证）
- ✅ 代码简洁
- ✅ 无锁，性能好
- ❌ 无延迟初始化
- ❌ 可能浪费资源

### 4. Lazy&lt;T&gt; 单例（推荐）

```csharp
public sealed class LazySingleton
{
    private LazySingleton() { }
    
    private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());

    public static LazySingleton GetInstance() => _instance.Value;
}
```

**特点：**

- ✅ 线程安全（Lazy&lt;T&gt;内置）
- ✅ 延迟初始化
- ✅ 代码简洁现代
- ✅ .NET 推荐方式
- ⚠️ 依赖 Lazy&lt;T&gt;

## 测试用例

### 基本单例测试

```csharp
[Fact]
public void TestSingleton()
{
    Singleton s1 = Singleton.GetInstance();
    Singleton s2 = Singleton.GetInstance();
    Assert.Equal(s1, s2); // 验证是同一个实例
}
```

### 多线程安全测试

```csharp
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

    IEnumerable<Task> tasks = Enumerable
        .Range(0, taskCount)
        .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

    await Task.WhenAll(tasks);

    // 验证所有实例都是同一个对象
    Assert.NotNull(instances.First());
    for (int i = 1; i < taskCount; i++)
    {
        Assert.True(instances.TryTake(out ThreadSafeSingleton? instance));
        Assert.NotNull(instance);
        Assert.Equal(instance, instances.First());
    }
}
```

### Lazy单例测试

```csharp
[Fact]
public async Task TestLazySingletonAsync()
{
    const int taskCount = 1000;
    ConcurrentBag<LazySingleton?> instances = [];

    Task CreateInstanceAsync(int index)
    {
        instances.Add(LazySingleton.GetInstance());
        return Task.CompletedTask;
    }

    IEnumerable<Task> tasks = Enumerable
        .Range(0, taskCount)
        .Select(i => Task.Run(async () => await CreateInstanceAsync(i)));

    await Task.WhenAll(tasks);

    // 验证线程安全性和唯一性
    Assert.NotNull(instances.First());
    foreach (var instance in instances)
    {
        Assert.NotNull(instance);
        Assert.Equal(instance, instances.First());
    }
}
```

### 静态单例测试

```csharp
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

    IEnumerable<Task> tasks = Enumerable
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
```

## 使用场景比较

| 场景 | 推荐实现 | 原因 |
|------|----------|------|
| 单线程应用 | 简单单例 | 代码简洁，无多线程考虑 |
| 多线程应用 | Lazy单例 | 线程安全，延迟初始化，现代化 |
| 启动时必需 | 静态单例 | 避免延迟初始化开销 |
| 高并发场景 | Lazy单例 | 内置优化，性能好 |
| 传统项目 | 线程安全单例 | 兼容性好，可控性强 |

## 关键实现要点

### 1. 双重检查锁定模式

```csharp
public static ThreadSafeSingleton GetInstance()
{
    if (_instance == null)  // 第一次检查
    {
        lock (_lock)
        {
            _instance ??= new();  // 第二次检查
        }
    }
    return _instance;
}
```

- **第一次检查**：避免已初始化后的锁开销
- **锁定**：确保线程安全
- **第二次检查**：防止重复初始化

### 2. Lazy&lt;T&gt; 的优势

```csharp
private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());
```

- **内置线程安全**：无需手动处理锁
- **延迟初始化**：只在首次访问时创建
- **性能优化**：.NET 框架优化

## 最佳实践

### ✅ 推荐做法

1. **使用 `sealed` 关键字**：防止继承破坏单例
2. **私有构造函数**：防止外部实例化
3. **优先选择 Lazy&lt;T&gt;**：现代、安全、简洁
4. **充分测试多线程场景**：确保线程安全

### ❌ 避免做法

1. **不要使用公共构造函数**
2. **不要忽略线程安全**（除非确定单线程）
3. **不要过度使用单例**：可能导致紧耦合
4. **避免单例承担过多职责**

## 实际应用示例

### 配置管理器

```csharp
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _instance = 
        new(() => new ConfigurationManager());
    
    private readonly Dictionary<string, string> _settings;
    
    private ConfigurationManager()
    {
        _settings = LoadSettings(); // 加载配置文件
    }
    
    public static ConfigurationManager Instance => _instance.Value;
    
    public string GetSetting(string key) => 
        _settings.TryGetValue(key, out var value) ? value : string.Empty;
        
    public void SetSetting(string key, string value) => 
        _settings[key] = value;
        
    private Dictionary<string, string> LoadSettings()
    {
        // 模拟从配置文件加载设置
        return new Dictionary<string, string>
        {
            ["DatabaseConnection"] = "Server=localhost;Database=MyApp;",
            ["LogLevel"] = "Information",
            ["MaxRetries"] = "3"
        };
    }
}
```

### 日志记录器

```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = new(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
    
    public void LogInfo(string message) => 
        Console.WriteLine($"[INFO {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        
    public void LogError(string message) => 
        Console.WriteLine($"[ERROR {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        
    public void LogWarning(string message) => 
        Console.WriteLine($"[WARNING {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
}
```

### 数据库连接管理器

```csharp
public sealed class DatabaseConnectionManager
{
    private static readonly Lazy<DatabaseConnectionManager> _instance = 
        new(() => new DatabaseConnectionManager());
    
    private readonly string _connectionString;
    private readonly object _lock = new();
    
    private DatabaseConnectionManager()
    {
        _connectionString = ConfigurationManager.Instance.GetSetting("DatabaseConnection");
    }
    
    public static DatabaseConnectionManager Instance => _instance.Value;
    
    public IDbConnection GetConnection()
    {
        lock (_lock)
        {
            // 这里可以实现连接池逻辑
            return new SqlConnection(_connectionString);
        }
    }
}
```

## 优势与劣势

### ✅ 优势

1. **控制实例数量**：严格保证只有一个实例
2. **全局访问点**：提供统一的访问方式
3. **延迟初始化**：节省资源，按需创建
4. **线程安全**：多线程环境下的安全保障

### ❌ 劣势

1. **单一职责原则违反**：既控制实例创建又处理业务逻辑
2. **紧耦合**：全局状态可能导致模块间紧密耦合
3. **测试困难**：难以模拟和单元测试
4. **并发瓶颈**：可能成为系统性能瓶颈

## 与其他模式的关系

- **工厂模式**：单例可以作为工厂模式的实现基础
- **抽象工厂**：抽象工厂本身可以设计为单例
- **建造者模式**：建造者可以是单例
- **原型模式**：原型注册表可以是单例

## 现代替代方案

### 依赖注入 (Dependency Injection)

```csharp
// 使用 DI 容器注册单例
services.AddSingleton<IConfigurationService, ConfigurationService>();
services.AddSingleton<ILogger, FileLogger>();
```

**优势：**

- 更好的可测试性
- 松耦合
- 生命周期管理
- 依赖倒置

---

*本文档基于 C# .NET 9.0 实现，展示了单例模式的完整实现和测试用例。*
