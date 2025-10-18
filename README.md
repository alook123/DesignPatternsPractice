# 设计模式实践项目

📚 学习和实践设计模式的完整项目，包含代码实现、单元测试和详细文档。

## 🚀 快速开始

```bash
# 克隆项目
git clone https://github.com/alook123/DesignPatternsPractice.git

# 进入项目目录
cd DesignPatternsPractice

# 构建项目
dotnet build

# 运行测试
dotnet test
```

## 📁 项目结构

```
DesignPatternsPractice/
├── 📖 docs/                      # 📚 所有文档
│   ├── CreationalPatterns/       # 🏗️ 创建型模式文档
│   ├── StructuralPatterns/       # 🔧 结构型模式文档
│   ├── Comparisons/              # 📊 对比总结文档
│   └── *.md                      # 基础指南文档
│
├── 💻 DesignPatterns/            # 设计模式实现代码
│   ├── CreationalPatterns/       # 创建型模式
│   └── StructuralPatterns/       # 结构型模式
│
├── ✅ DesignPatterns.Tests/      # 单元测试
└── ⚙️ .github/workflows/         # CI/CD & Notion 同步
```

## 🎯 已实现的模式

### 🏗️ 创建型模式 (5/5)

- ✅ 🔒 [单例模式 Singleton](docs/CreationalPatterns/SingletonPattern.md)
- ✅ 🏭 [工厂方法 Factory Method](docs/CreationalPatterns/FactoryMethodPattern.md)
- ✅ 🏢 [抽象工厂 Abstract Factory](docs/CreationalPatterns/AbstractFactoryPattern.md)
- ✅ 🔧 [建造者 Builder](docs/CreationalPatterns/BuilderPattern.md)
- ✅ 📋 [原型 Prototype](docs/CreationalPatterns/PrototypePattern.md)

### 🔧 结构型模式 (1/7)

- ✅ 🔌 [适配器 Adapter](docs/StructuralPatterns/AdapterPattern.md)
- 🚧 装饰者 Decorator (计划中)
- 🚧 外观 Facade (计划中)
- 🚧 代理 Proxy (计划中)

### 📊 对比总结

- ✅ [创建型模式对比](docs/Comparisons/CreationalPatternsComparison.md)

## 🔄 Notion 同步

项目配置了自动同步到 Notion 数据库：

- 📝 自动同步所有文档变更
- 🎨 美观的分类和导航
- 🔍 强大的搜索和筛选
- 详见：[Notion 设置指南](docs/NOTION_SETUP.md)

## 🧪 测试覆盖

```bash
# 运行所有测试
dotnet test

# 查看测试覆盖率
dotnet test --collect:"XPlat Code Coverage"
```

当前测试状态：✅ 所有模式都有完整的单元测试

## 💡 学习建议

1. **初学者**: 从 [Getting Started](docs/Getting-Started.md) 开始
2. **进阶学习**: 按照创建型 → 结构型 → 行为型的顺序学习
3. **面试准备**: 重点看对比文档和各模式的"面试要点"部分
4. **实践项目**: 所有代码都可以直接运行和测试

## 🤝 贡献

欢迎贡献代码和文档！

1. Fork 项目
2. 创建 feature 分支
3. 提交 PR
4. 文档会自动同步到 Notion

## 📄 许可证

MIT License - 详见 [LICENSE](LICENSE) 文件
