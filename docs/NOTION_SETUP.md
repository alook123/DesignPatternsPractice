# Notion 同步设置指南

## 📋 前提条件

### 1. 创建 Notion 集成

1. 访问 [Notion Integrations](https://www.notion.so/my-integrations)
2. 点击 "New Integration"
3. 填写基本信息：
   - Name: `Design Patterns Sync`
   - Associated workspace: 选择你的工作区
   - Type: Internal
4. 点击 "Submit" 创建集成
5. 复制 "Internal Integration Token"（以 `secret_` 开头）

### 2. 创建 Notion 数据库

1. 在 Notion 中创建新页面
2. 添加数据库，选择 "Table" 视图
3. 设置数据库属性：

| 属性名 | 类型 | 说明 |
|--------|------|------|
| Name | Title | 文档标题 |
| Category | Select | 文档分类 |
| Type | Select | 文档类型 |
| Priority | Number | 优先级 |
| Emoji | Text | 图标 |
| Last Updated | Date | 最后更新时间 |
| File Path | Text | 文件路径 |

4. 为 Category 添加选项：
   - 📚 总结对比
   - 📖 基础文档
   - 🏗️ 创建型模式
   - 🔧 结构型模式
   - 📄 其他文档

5. 为 Type 添加选项：
   - 对比文档
   - 指南文档
   - 设计模式
   - 文档

### 3. 获取数据库 ID

1. 打开数据库页面
2. 复制 URL 中的数据库 ID：

   ```
   https://notion.so/workspace/DATABASE_ID?v=...
   ```

   DATABASE_ID 是一个 32 字符的字符串

### 4. 分享数据库给集成

1. 在数据库页面点击右上角 "Share"
2. 点击 "Invite"
3. 搜索你创建的集成名称
4. 选择集成并给予 "Can edit" 权限

## 🔧 GitHub Secrets 配置

在你的 GitHub 仓库中设置以下 Secrets：

1. 进入仓库 → Settings → Secrets and variables → Actions
2. 添加以下 Repository secrets：

| Secret 名称 | 值 | 说明 |
|-------------|-----|------|
| `NOTION_TOKEN` | secret_xxx... | Notion 集成 Token |
| `NOTION_DATABASE_ID` | 32字符ID | 数据库 ID |

## 📊 数据库视图配置

为了更好的导航体验，建议创建以下视图：

### 1. 按分类查看

- Group by: Category
- Sort: Priority (升序)
- Filter: 无

### 2. 设计模式概览

- Filter: Type = "设计模式"
- Sort: Category, Priority
- Properties: Name, Category, Emoji, Last Updated

### 3. 最近更新

- Sort: Last Updated (降序)
- Properties: Name, Category, Type, Last Updated

## 🎨 美化建议

### 数据库模板

可以为不同类型的文档创建模板：

1. 点击数据库右上角 "..." → "Templates"
2. 创建 "设计模式" 模板，包含：
   - 概述
   - 核心思想
   - 代码结构
   - 使用场景
   - 面试要点
   - 记忆口诀

### 页面图标

脚本会自动为不同模式添加对应的 emoji：

- 🔒 单例模式
- 🏭 工厂方法
- 🏢 抽象工厂
- 🔧 建造者
- 📋 原型模式
- 🔌 适配器模式

### 导航页面

建议创建一个主导航页面，包含：

- 各分类的链接
- 学习路径建议
- 快速索引

## ⚡ 触发同步

同步会在以下情况自动触发：

- 推送到 main 分支
- 修改 `docs/**` 目录下的文件
- 修改 `DesignPatterns/**/*.md` 文件
- 修改 `CreationalPatternsComparison.md` 文件

## 🐛 故障排除

### 常见问题

1. **401 Unauthorized**
   - 检查 NOTION_TOKEN 是否正确
   - 确认集成有访问数据库的权限

2. **404 Not Found**
   - 检查 NOTION_DATABASE_ID 是否正确
   - 确认数据库已分享给集成

3. **同步失败**
   - 查看 GitHub Actions 日志
   - 检查 Markdown 文件格式是否正确

### 手动同步

如果需要手动触发同步：

1. 进入 GitHub Actions
2. 选择 "Sync Docs to Notion" workflow
3. 点击 "Run workflow"

## 📈 监控和维护

- 定期检查 GitHub Actions 执行状态
- 监控 Notion 数据库更新情况
- 根据需要调整分类和优先级
