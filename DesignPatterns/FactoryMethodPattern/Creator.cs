namespace DesignPatterns.FactoryMethodPattern;

// Creator 类声明了工厂方法，该方法应返回 Product 类的对象。
// Creator 的子类通常会提供此方法的实现。
public abstract class Creator
{
    // 创建者还可能提供工厂方法的默认实现。
    public abstract IProduct FactoryMethod();

    // 创建者的主要职责不是创建产品。它通常包含一些依赖于 Product 对象的核心业务逻辑，
    // 这些对象由工厂方法返回。子类可以通过重写工厂方法并返回不同类型的产品来间接更改此业务逻辑。
    public string SomeOperation()
    {
        // 调用工厂方法创建一个产品对象。
        var product = FactoryMethod();
        // 现在使用产品。
        var result = $"Creator: 使用 {product.Operation()} 进行工作";

        return result;
    }
}

// 具体创建者会重写工厂方法以更改其返回的产品类型。
public class ConcreteCreator1 : Creator
{
    // 注意，方法的签名仍使用抽象产品类型，即使具体产品实际上是从此方法返回的。
    // 这样，创建者就可以独立于具体产品类。
    public override IProduct FactoryMethod()
    {
        return new ConcreteProduct1();
    }
}

public class ConcreteCreator2 : Creator
{
    public override IProduct FactoryMethod()
    {
        return new ConcreteProduct2();
    }
}

// 产品接口声明了所有具体产品必须实现的操作。
public interface IProduct
{
    string Operation();
}

// 具体产品提供产品接口的各种实现。
public class ConcreteProduct1 : IProduct
{
    public string Operation()
    {
        return "ConcreteProduct1 的结果";
    }
}

public class ConcreteProduct2 : IProduct
{
    public string Operation()
    {
        return "ConcreteProduct2 的结果";
    }
}

// 优点：
// 避免了创建者和具体产品之间的紧密耦合。
// 遵循单一职责原则，将产品创建代码放在程序的一个位置。
// 遵循开闭原则，可以引入新的产品类型，而无需修改现有代码。
// 可以轻松扩展新的具体产品。

// 缺点：
// 代码可能变得更复杂，因为需要引入许多新的子类来实现模式。
// 在只需要创建一种产品的场景中可能显得过度设计。