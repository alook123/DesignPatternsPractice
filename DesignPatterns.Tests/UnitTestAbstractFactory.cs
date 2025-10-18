using DesignPatterns.CreationalPatterns.AbstractFactoryPattern;

namespace DesignPatterns.Tests;

public class UnitTestAbstractFactory
{
    [Fact]
    public void Factory1_Should_Create_ProductFamily1()
    {
        // Arrange
        IAbstractFactory factory = new Factory1();

        // Act
        IAbstractProductA productA = factory.CreateProductA();
        IAbstractProductB productB = factory.CreateProductB();

        // Assert
        Assert.IsType<ProductA1>(productA);
        Assert.IsType<ProductB1>(productB);
        Assert.Equal("The result of the product A1.", productA.UsefulFunctionA());
        Assert.Equal("The result of the product B1.", productB.UsefulFunctionB());
    }

    [Fact]
    public void Factory2_Should_Create_ProductFamily2()
    {
        // Arrange
        IAbstractFactory factory = new Factory2();

        // Act
        IAbstractProductA productA = factory.CreateProductA();
        IAbstractProductB productB = factory.CreateProductB();

        // Assert
        Assert.IsType<ProductA2>(productA);
        Assert.IsType<ProductB2>(productB);
        Assert.Equal("The result of the product A2.", productA.UsefulFunctionA());
        Assert.Equal("The result of the product B2.", productB.UsefulFunctionB());
    }

    [Fact]
    public void Products_Should_Collaborate_Within_SameFamily()
    {
        // Arrange
        IAbstractFactory factory1 = new Factory1();
        IAbstractFactory factory2 = new Factory2();

        // Act
        IAbstractProductA productA1 = factory1.CreateProductA();
        IAbstractProductB productB1 = factory1.CreateProductB();

        IAbstractProductA productA2 = factory2.CreateProductA();
        IAbstractProductB productB2 = factory2.CreateProductB();

        // Assert - 同一族产品可以协作
        string collaboration1 = productB1.AnotherUsefulFunctionB(productA1);
        string collaboration2 = productB2.AnotherUsefulFunctionB(productA2);

        Assert.Contains("B1 collaborating with the (The result of the product A1.)", collaboration1);
        Assert.Contains("B2 collaborating with the (The result of the product A2.)", collaboration2);
    }
}