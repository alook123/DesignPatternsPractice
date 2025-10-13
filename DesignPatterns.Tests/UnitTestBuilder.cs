using DesignPatterns.BuilderPattern;

namespace DesignPatterns.Tests;

public class UnitTestBuilder
{
    [Fact]
    public void TestBuilder_CanBuildProductStepByStep()
    {
        // Arrange - 创建建造者
        ConcreteBuilder builder = new();

        // Act - 逐步构建产品
        builder.BuildPartA();
        builder.BuildPartB();
        builder.BuildPartC();
        Product product = builder.GetProduct();

        // Assert - 验证产品包含所有部件
        string parts = product.ListParts();
        Assert.Contains("PartA1", parts);
        Assert.Contains("PartB1", parts);
        Assert.Contains("PartC1", parts);
    }

    [Fact]
    public void TestDirector_CanBuildDifferentProductVariants()
    {
        // Arrange - 创建建造者和指挥者
        ConcreteBuilder builder = new();
        Director director = new(builder);

        // Act & Assert - 构建最小可行产品
        director.BuildMinimalViableProduct();
        Product mvpProduct = builder.GetProduct();
        string mvpParts = mvpProduct.ListParts();

        Assert.Contains("PartA1", mvpParts);
        Assert.DoesNotContain("PartB1", mvpParts);
        Assert.DoesNotContain("PartC1", mvpParts);

        // Act & Assert - 构建完整功能产品
        director.BuildFullFeaturedProduct();
        Product fullProduct = builder.GetProduct();
        string fullParts = fullProduct.ListParts();

        Assert.Contains("PartA1", fullParts);
        Assert.Contains("PartB1", fullParts);
        Assert.Contains("PartC1", fullParts);
    }

    [Fact]
    public void TestBuilder_GetProductResetsBuilder()
    {
        // Arrange
        ConcreteBuilder builder = new();

        // Act - 构建第一个产品
        builder.BuildPartA();
        Product firstProduct = builder.GetProduct();

        // Act - 构建第二个产品
        builder.BuildPartB();
        Product secondProduct = builder.GetProduct();

        // Assert - 验证产品是独立的（获取产品后建造者被重置）
        string firstParts = firstProduct.ListParts();
        string secondParts = secondProduct.ListParts();

        Assert.Contains("PartA1", firstParts);
        Assert.DoesNotContain("PartB1", firstParts);

        Assert.Contains("PartB1", secondParts);
        Assert.DoesNotContain("PartA1", secondParts);
    }
}