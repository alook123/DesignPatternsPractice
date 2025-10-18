using DesignPatterns.StructuralPatterns.AdapterPattern;

namespace DesignPatterns.Tests;

public class UnitTestAdapter
{
    [Fact]
    public void Adapter_Should_MakeAdapteeCompatibleWithTarget()
    {
        // Arrange
        Adaptee adaptee = new();
        ITarget adapter = new Adapter(adaptee);

        // Act
        string result = adapter.GetRequest();

        // Assert
        Assert.Equal("This is 'Specific request.'", result);
    }

    [Fact]
    public void Adaptee_Should_ReturnSpecificRequest()
    {
        // Arrange
        Adaptee adaptee = new();

        // Act
        string result = adaptee.GetSpecificRequest();

        // Assert
        Assert.Equal("Specific request.", result);
    }

    [Fact]
    public void Adapter_Should_ImplementTargetInterface()
    {
        // Arrange
        Adaptee adaptee = new();
        Adapter adapter = new(adaptee);

        // Act & Assert
        Assert.IsAssignableFrom<ITarget>(adapter);
    }
}