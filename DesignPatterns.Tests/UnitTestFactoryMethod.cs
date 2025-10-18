using DesignPatterns.CreationalPatterns.FactoryMethodPattern;

namespace DesignPatterns.Tests;

public class UnitTestFactoryMethod
{
    [Fact]
    public void TestFactoryMethod()
    {
        // Arrange
        Creator creator1 = new Creator1();
        Creator creator2 = new Creator2();

        // Act
        IProduct product1 = creator1.FactoryMethod();
        IProduct product2 = creator2.FactoryMethod();

        // Assert
        Assert.IsType<Product1>(product1);
        Assert.Equal("{Product1 的结果}", product1.Operation());

        Assert.IsType<Product2>(product2);
        Assert.Equal("{Product2 的结果}", product2.Operation());

        Assert.NotSame(product1, product2); // 不同的产品实例
    }
}
