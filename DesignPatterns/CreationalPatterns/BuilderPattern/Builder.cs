namespace DesignPatterns.CreationalPatterns.BuilderPattern;

public interface IBuilder
{
    IBuilder BuildPartA();
    IBuilder BuildPartB();
    IBuilder BuildPartC();
}
public class ConcreteBuilder : IBuilder
{
    private Product _product = new();
    public ConcreteBuilder()
    {
        Reset();
    }
    public void Reset() => _product = new();
    public IBuilder BuildPartA()
    {
        _product.Add("PartA1");
        return this;
    }
    public IBuilder BuildPartB()
    {
        _product.Add("PartB1");
        return this;
    }
    public IBuilder BuildPartC()
    {
        _product.Add("PartC1");
        return this;
    }
    public Product GetProduct()
    {
        Product result = _product;
        Reset();
        return result;
    }
}
public class Product
{
    private readonly List<object> _parts = [];
    public void Add(string part) => _parts.Add(part);
    public string ListParts() => "产品零件: " + string.Join(", ", _parts) + "\n";
}
public class Director(IBuilder builder)
{
    public IBuilder Builder { private get; set; } = builder;
    public void BuildMinimalViableProduct() => Builder.BuildPartA();
    public void BuildFullFeaturedProduct() => Builder.BuildPartA().BuildPartB().BuildPartC();
}
