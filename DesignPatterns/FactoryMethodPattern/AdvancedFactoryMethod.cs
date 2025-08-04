namespace DesignPatterns.FactoryMethodPattern;

// 参数化工厂方法模式
// 这种变体允许根据参数创建不同类型的产品

/// <summary>
/// 定义文档类型的枚举
/// </summary>
public enum DocumentType
{
    PDF,
    Word,
    Excel,
    Text
}

/// <summary>
/// 参数化工厂 - 可以根据类型参数创建不同的文档
/// </summary>
public class ParameterizedDocumentFactory
{
    // 参数化工厂方法
    public IDocument CreateDocument(DocumentType type)
    {
        return type switch
        {
            DocumentType.PDF => new PDFDocument(),
            DocumentType.Word => new WordDocument(),
            DocumentType.Excel => new ExcelDocument(),
            DocumentType.Text => new TextDocument(),
            _ => throw new ArgumentException($"未知的文档类型: {type}")
        };
    }
}

/// <summary>
/// 另一种实现方式 - 使用字典注册产品创建器
/// </summary>
public class ConfigurableDocumentFactory
{
    // 使用委托存储创建产品的方法
    private readonly Dictionary<string, Func<IDocument>> _creators = new();

    // 注册产品创建器
    public void RegisterCreator(string documentType, Func<IDocument> creator)
    {
        _creators[documentType] = creator;
    }

    // 创建文档
    public IDocument CreateDocument(string documentType)
    {
        if (!_creators.ContainsKey(documentType))
        {
            throw new ArgumentException($"没有注册此类型的创建器: {documentType}");
        }

        return _creators[documentType]();
    }
}

// 新增的具体产品 - 文本文档
public class TextDocument : IDocument
{
    public string Open()
    {
        return "打开文本文档...";
    }

    public string Save()
    {
        return "保存文本文档...";
    }
}

// 客户端代码示例
public class AdvancedApplication
{
    public void RunParameterized()
    {
        // 使用参数化工厂
        ParameterizedDocumentFactory factory = new ParameterizedDocumentFactory();

        // 创建不同类型的文档
        IDocument pdfDoc = factory.CreateDocument(DocumentType.PDF);
        IDocument wordDoc = factory.CreateDocument(DocumentType.Word);
        IDocument excelDoc = factory.CreateDocument(DocumentType.Excel);
        IDocument textDoc = factory.CreateDocument(DocumentType.Text);

        Console.WriteLine(pdfDoc.Open());
        Console.WriteLine(wordDoc.Open());
        Console.WriteLine(excelDoc.Open());
        Console.WriteLine(textDoc.Open());
    }

    public void RunConfigurable()
    {
        // 使用可配置工厂
        ConfigurableDocumentFactory factory = new ConfigurableDocumentFactory();

        // 注册创建器
        factory.RegisterCreator("pdf", () => new PDFDocument());
        factory.RegisterCreator("word", () => new WordDocument());
        factory.RegisterCreator("excel", () => new ExcelDocument());
        factory.RegisterCreator("text", () => new TextDocument());

        // 使用工厂创建产品
        IDocument pdfDoc = factory.CreateDocument("pdf");
        IDocument wordDoc = factory.CreateDocument("word");

        Console.WriteLine(pdfDoc.Open());
        Console.WriteLine(wordDoc.Open());

        // 动态注册新的创建器
        factory.RegisterCreator("custom", () => new CustomDocument());
        IDocument customDoc = factory.CreateDocument("custom");
        Console.WriteLine(customDoc.Open());
    }
}

// 运行时动态创建的产品
public class CustomDocument : IDocument
{
    public string Open()
    {
        return "打开自定义文档...";
    }

    public string Save()
    {
        return "保存自定义文档...";
    }
}

// 工厂方法模式的优点：
// 1. 遵循开闭原则：可以引入新类型的产品而无需修改现有的工厂代码
// 2. 遵循单一职责原则：将产品创建逻辑移到一个地方，便于维护
// 3. 松耦合：客户端代码与具体产品类解耦

// 工厂方法模式的变体：
// 1. 简单工厂：不通过继承而是使用参数来确定创建什么产品（如上面的ParameterizedDocumentFactory）
// 2. 抽象工厂：创建相关对象的家族，而不需要指定其具体类
// 3. 静态工厂方法：使用静态方法作为工厂方法