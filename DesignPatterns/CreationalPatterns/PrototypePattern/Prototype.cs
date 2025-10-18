namespace DesignPatterns.CreationalPatterns.PrototypePattern;

public interface IPrototype<T>
{
    T ShallowCopy();
    T DeepClone();
}

public class Prototype(string name) : IPrototype<Prototype>
{
    public string Name { get; set; } = name;
    public List<string> Items { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
    public NestedObject Nested { get; set; } = new();
    public Prototype ShallowCopy() => (Prototype)MemberwiseClone();
    public Prototype DeepClone()
    {
        Prototype clone = new(Name)
        {
            Items = [.. Items],
            Properties = new Dictionary<string, object>(Properties),
            Nested = Nested.DeepClone()
        };

        return clone;
    }
}

public class NestedObject : IPrototype<NestedObject>
{
    public string Data { get; set; } = "默认数据";
    public int Counter { get; set; } = 0;
    public NestedObject ShallowCopy() => (NestedObject)MemberwiseClone();
    public NestedObject DeepClone()
    {
        return new NestedObject
        {
            Data = new string(Data),
            Counter = Counter
        };
    }
}