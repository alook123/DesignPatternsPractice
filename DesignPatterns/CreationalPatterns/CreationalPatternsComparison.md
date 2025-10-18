# 创建型设计模式对比总结

## 概述

创建型模式关注对象的创建过程，提供了创建对象的最佳方式。本文档对比了5种主要的创建型模式。

## 五种模式速览

| 模式 | 核心目的 | 关键词 | 记忆口诀 |
|------|----------|--------|----------|
| **Singleton** | 确保只有一个实例 | 全局唯一 | "全局唯一一个娃" |
| **Factory Method** | 子类决定创建什么 | 继承创建 | "子类决定造什么" |
| **Abstract Factory** | 创建产品族 | 产品家族 | "一家工厂造全套" |
| **Builder** | 分步构建复杂对象 | 步骤化 | "按步骤搭积木" |
| **Prototype** | 通过复制创建对象 | 克隆复制 | "复制粘贴造对象" |

## 详细对比

### 1. 使用场景对比

#### Singleton 单例模式

- **适用**: 配置管理、日志记录、数据库连接池
- **特点**: 控制实例数量，提供全局访问
- **何时用**: 确保系统中只需要一个实例

#### Factory Method 工厂方法

- **适用**: 框架设计、数据库驱动、UI组件
- **特点**: 通过继承扩展，符合开闭原则
- **何时用**: 需要根据条件创建不同产品，且经常扩展新产品

#### Abstract Factory 抽象工厂

- **适用**: UI主题系统、跨平台开发、游戏种族系统
- **特点**: 创建相关产品族，保证产品一致性
- **何时用**: 需要创建一系列相关或相互依赖的产品

#### Builder 建造者

- **适用**: SQL查询构建、HTTP请求、复杂配置对象
- **特点**: 分步构建，过程可控，结果多样
- **何时用**: 对象有很多可选参数，构造过程复杂

#### Prototype 原型

- **适用**: 游戏对象复制、文档模板、配置克隆
- **特点**: 通过复制避免重复初始化
- **何时用**: 对象创建成本高，且需要大量相似对象

### 2. 创建方式对比

```csharp
// Singleton - 控制实例数量
var instance = MySingleton.Instance;

// Factory Method - 通过子类创建
Creator creator = new ConcreteCreator();
IProduct product = creator.FactoryMethod();

// Abstract Factory - 创建产品族
IAbstractFactory factory = new ConcreteFactory1();
IProductA productA = factory.CreateProductA();
IProductB productB = factory.CreateProductB();

// Builder - 分步构建
var builder = new ConcreteBuilder();
builder.BuildPartA();
builder.BuildPartB();
Product product = builder.GetProduct();

// Prototype - 通过复制创建
IPrototype original = new ConcretePrototype();
IPrototype copy = original.Clone();
```

### 3. 复杂度对比

| 模式 | 实现复杂度 | 理解难度 | 维护成本 |
|------|------------|----------|----------|
| Singleton | ⭐⭐ | ⭐⭐ | ⭐⭐ |
| Factory Method | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ |
| Abstract Factory | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| Builder | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ |
| Prototype | ⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ |

### 4. 扩展性对比

#### 添加新产品类型

- **Factory Method**: ✅ 容易 - 新增Creator子类
- **Abstract Factory**: ❌ 困难 - 需修改所有工厂接口
- **Builder**: ✅ 容易 - 新增构建步骤
- **Prototype**: ✅ 容易 - 实现克隆接口
- **Singleton**: ❌ 不适用

#### 添加新产品族

- **Abstract Factory**: ✅ 容易 - 新增工厂实现
- **Factory Method**: ❌ 不适用
- **其他**: ❌ 不适用

## 面试要点

### 常见面试问题

#### 1. "工厂方法和抽象工厂有什么区别？"

**回答要点**:

- **产品数量**: 工厂方法创建一种产品，抽象工厂创建产品族
- **扩展方式**: 工厂方法通过继承，抽象工厂通过产品族
- **使用场景**: 工厂方法适合单一产品扩展，抽象工厂适合相关产品组合

#### 2. "什么时候用建造者，什么时候用工厂？"

**回答要点**:

- **参数复杂度**: 参数多且可选用Builder，参数少用Factory
- **构建过程**: 需要分步控制用Builder，一步到位用Factory
- **结果多样性**: 同样步骤产生不同结果用Builder

#### 3. "单例模式有什么问题？"

**回答要点**:

- **测试困难**: 全局状态难以隔离
- **违反原则**: 违反单一职责原则
- **线程安全**: 需要考虑多线程问题

### 选择决策树

```text
需要创建对象？
├─ 只需要一个实例？ → Singleton
├─ 复制现有对象？ → Prototype
├─ 对象构造复杂？
│  ├─ 参数很多？ → Builder
│  └─ 需要扩展？ → Factory Method
└─ 需要创建产品族？ → Abstract Factory
```

## 最佳实践建议

### 1. 优先级推荐

1. **Factory Method** - 最常用，符合开闭原则
2. **Builder** - 处理复杂对象构建
3. **Singleton** - 谨慎使用，考虑依赖注入替代
4. **Abstract Factory** - 复杂系统的产品族管理
5. **Prototype** - 特定场景下的性能优化

### 2. 组合使用

- **Singleton + Factory**: 单例工厂
- **Builder + Factory**: 工厂创建建造者
- **Abstract Factory + Singleton**: 单例抽象工厂

### 3. 现代替代方案

- **依赖注入**: 替代Singleton
- **记录类型**: 替代简单Builder
- **表达式树**: 动态创建替代Factory

## 总结

创建型模式解决了对象创建的各种问题：

- **控制数量** → Singleton
- **延迟决定** → Factory Method  
- **保证一致** → Abstract Factory
- **分步构建** → Builder
- **高效复制** → Prototype

选择模式时要考虑：扩展性、复杂度、维护成本和实际需求。面试中重点掌握各模式的区别和适用场景。
