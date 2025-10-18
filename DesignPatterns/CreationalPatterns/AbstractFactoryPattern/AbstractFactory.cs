namespace DesignPatterns.CreationalPatterns.AbstractFactoryPattern;

public interface IAbstractFactory
{
    IAbstractProductA CreateProductA();
    IAbstractProductB CreateProductB();
}
public class Factory1 : IAbstractFactory
{
    public IAbstractProductA CreateProductA() => new ProductA1();
    public IAbstractProductB CreateProductB() => new ProductB1();
}
public class Factory2 : IAbstractFactory
{
    public IAbstractProductA CreateProductA() => new ProductA2();
    public IAbstractProductB CreateProductB() => new ProductB2();
}
public interface IAbstractProductA
{
    string UsefulFunctionA();
}
public class ProductA1 : IAbstractProductA
{
    public string UsefulFunctionA() => "产品A1的结果。";
}
public class ProductA2 : IAbstractProductA
{
    public string UsefulFunctionA() => "产品A2的结果。";
}
public interface IAbstractProductB
{
    string UsefulFunctionB();
    string AnotherUsefulFunctionB(IAbstractProductA collaborator);
}

public class ProductB1 : IAbstractProductB
{
    public string UsefulFunctionB() => "产品B1的结果。";

    public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
    {
        string result = collaborator.UsefulFunctionA();
        return $"产品B1与({result})协作的结果。";
    }
}

public class ProductB2 : IAbstractProductB
{
    public string UsefulFunctionB()
    {
        return "产品B2的结果。";
    }
    public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
    {
        string result = collaborator.UsefulFunctionA();
        return $"产品B2与({result})协作的结果。";
    }
}