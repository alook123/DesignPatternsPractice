namespace DesignPatterns.FactoryMethodPattern;

/// <summary>
/// 文档创建应用程序的实际示例
/// </summary>
public abstract class DocumentCreator
{
    // 工厂方法
    public abstract IDocument CreateDocument();

    // 创建和处理文档的核心业务逻辑
    public string EditDocument()
    {
        // 调用工厂方法创建一个文档对象
        IDocument document = CreateDocument();

        // 使用文档
        return $"编辑器: 正在编辑 {document.GetType().Name} - {document.Open()}";
    }
}

// 具体创建者 - PDF文档创建器
public class PDFDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument()
    {
        return new PDFDocument();
    }
}

// 具体创建者 - Word文档创建器
public class WordDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument()
    {
        return new WordDocument();
    }
}

// 具体创建者 - Excel文档创建器
public class ExcelDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument()
    {
        return new ExcelDocument();
    }
}

// 产品接口 - 文档接口
public interface IDocument
{
    string Open();
    string Save();
}

// 具体产品 - PDF文档
public class PDFDocument : IDocument
{
    public string Open()
    {
        return "打开PDF文档...";
    }

    public string Save()
    {
        return "保存PDF文档...";
    }
}

// 具体产品 - Word文档
public class WordDocument : IDocument
{
    public string Open()
    {
        return "打开Word文档...";
    }

    public string Save()
    {
        return "保存Word文档...";
    }
}

// 具体产品 - Excel文档
public class ExcelDocument : IDocument
{
    public string Open()
    {
        return "打开Excel电子表格...";
    }

    public string Save()
    {
        return "保存Excel电子表格...";
    }
}

// 客户端代码示例
public class Application
{
    public void Run()
    {
        Console.WriteLine("应用程序: 启动PDF创建器");
        ClientCode(new PDFDocumentCreator());

        Console.WriteLine("");

        Console.WriteLine("应用程序: 启动Word创建器");
        ClientCode(new WordDocumentCreator());

        Console.WriteLine("");

        Console.WriteLine("应用程序: 启动Excel创建器");
        ClientCode(new ExcelDocumentCreator());
    }

    // 客户端代码与具体创建者的特定类一起工作，但通过其基类接口工作
    // 只要客户端通过基类接口与创建者合作，你就可以将任何创建者子类传递给客户端
    private void ClientCode(DocumentCreator creator)
    {
        Console.WriteLine($"客户端: 我不关心创建者类，但它仍然可以工作\n{creator.EditDocument()}");
    }
}

// 工厂方法模式使用场景：
// 1. 当事先不知道需要创建的对象的确切类型和依赖关系时
// 2. 当希望为库或框架的用户提供扩展其内部组件的方法时
// 3. 当需要复用现有对象而不是重新创建它们时