namespace DesignPatterns.CreationalPatterns.FactoryMethodPattern;

public abstract class Creator
{
    public abstract IProduct FactoryMethod();
}
public class Creator1 : Creator
{
    public override IProduct FactoryMethod() => new Product1();
}
public class Creator2 : Creator
{
    public override IProduct FactoryMethod() => new Product2();
}
public interface IProduct
{
    string Operation();
}
public class Product1 : IProduct
{
    public string Operation() => "{Product1 的结果}";
}
public class Product2 : IProduct
{
    public string Operation() => "{Product2 的结果}";
}
public class Client(Creator creator)
{
    private readonly Creator _creator = creator;

    public void Run()
    {
        IProduct product = _creator.FactoryMethod();
        product.Operation();
    }
}
