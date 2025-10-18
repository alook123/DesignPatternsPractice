# 单例模式 Singleton Pattern

## 概述

确保一个类只有一个实例，并提供全局访问点。

## 核心思想

- **问题**: 某些类只应该有一个实例（数据库连接、日志器）
- **解决**: 私有构造函数 + 静态实例 + 全局访问方法

## 两种推荐实现方式

### 1. 嵌套类单例（线程安全 + 懒加载）

```csharp
public sealed class NestedSingleton
{
    private NestedSingleton() { }
    public static NestedSingleton Instance => Nested.Instance;
    
    private class Nested
    {
        internal static readonly NestedSingleton Instance = new();
    }
}
```

### 2. Lazy 单例（推荐）

```csharp
public sealed class LazySingleton
{
    private LazySingleton() { }
    private static readonly Lazy<LazySingleton> _instance = new(() => new LazySingleton());
    public static LazySingleton Instance => _instance.Value;
}
```

## 不推荐的实现方式（面试常问）

### 1. 简单单例 ❌

```csharp
public class SimpleSingleton
{
    private static SimpleSingleton _instance;
    private SimpleSingleton() { }
    
    public static SimpleSingleton Instance
    {
        get
        {
            if (_instance == null)  // 线程不安全！
                _instance = new SimpleSingleton();
            return _instance;
        }
    }
}
```

**问题**: 多线程环境下可能创建多个实例

### 2. 加锁单例 ❌

```csharp
public class LockSingleton
{
    private static LockSingleton _instance;
    private static readonly object _lock = new object();
    private LockSingleton() { }
    
    public static LockSingleton Instance
    {
        get
        {
            lock (_lock)  // 每次访问都加锁，性能差！
            {
                if (_instance == null)
                    _instance = new LockSingleton();
                return _instance;
            }
        }
    }
}
```

**问题**: 每次获取实例都要加锁，性能低下

### 3. 静态单例 ❌

```csharp
public class StaticSingleton
{
    public static readonly StaticSingleton Instance = new();  // 启动时就创建！
    private StaticSingleton() { }
}
```

**问题**: 应用启动时就创建实例，不是懒加载

## 使用场景

1. **配置管理**: 应用程序配置
2. **日志记录**: 全局日志器
3. **数据库连接**: 连接池管理
4. **缓存管理**: 全局缓存

## 面试要点

### 优点

- 控制实例数量，节约内存
- 提供全局访问点
- 线程安全且性能良好

### 缺点

- 违反单一职责原则
- 单元测试困难（全局状态）

### 实现方式总结

#### 推荐使用 ✅

- **嵌套类单例**: 利用类加载机制保证线程安全，懒加载，性能最优
- **Lazy单例**: 使用 .NET 的 Lazy 泛型，代码更简洁，推荐使用

#### 不推荐使用 ❌

- **简单单例**: 线程不安全，多线程可能创建多个实例
- **加锁单例**: 每次访问都加锁，性能低下
- **静态单例**: 非懒加载，应用启动时就创建实例

### 访问方式选择

**Instance 属性 vs GetInstance() 方法**：

- **推荐**: `MySingleton.Instance` (属性方式)
- **原因**: 更符合 C# 惯用法，代码简洁，.NET 框架一致性
- **使用**: `var logger = Logger.Instance;`

### 面试回答技巧

当面试官问"你知道几种单例实现方式"时：

1. **先说推荐的**: 嵌套类和Lazy方式
2. **再说不推荐的**: 简单、加锁、静态方式
3. **分析问题**: 说明每种不推荐方式的具体问题
4. **总结**: 现代开发中推荐使用嵌套类或Lazy方式

## 记忆口诀

**"全局唯一一个娃"** - Singleton 确保类只有一个实例
