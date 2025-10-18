namespace DesignPatterns.StructuralPatterns.AdapterPattern;

public interface ITarget
{
    string GetRequest();
}

// Adaptee 包含一些有用的行为，但其接口与现有的客户端代码不兼容。Adaptee 需要进行一些适配才能被客户端代码使用。
public class Adaptee
{
    public string GetSpecificRequest()
    {
        return "Specific request.";
    }
}

// Adapter 使得 Adaptee 的接口与 Target 的接口兼容。
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        this._adaptee = adaptee;
    }

    public string GetRequest()
    {
        return $"This is '{this._adaptee.GetSpecificRequest()}'";
    }
}
public class Client
{
    public static void ClientCode(ITarget target)
    {
        Console.WriteLine(target.GetRequest());
    }
}