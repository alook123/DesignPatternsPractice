namespace DesignPatterns.PrototypePattern;

// 原型接口声明了克隆方法。在大多数情况下，这是一个包含单个 Clone 方法的接口。
public interface IPrototype<T>
{
    /// <summary>
    /// 浅拷贝 - 创建当前对象的浅表副本。
    /// 浅拷贝会复制值类型字段，但引用类型字段仍然指向原始对象。
    /// </summary>
    /// <returns>当前对象的浅表副本</returns>
    T ShallowCopy();

    /// <summary>
    /// 深拷贝 - 创建当前对象的完全独立副本。
    /// 深拷贝会递归复制所有字段，确保克隆体与原始对象没有任何共享引用。
    /// </summary>
    /// <returns>当前对象的完全独立副本</returns>
    T DeepClone();
}

public class Prototype(string name) : IPrototype<Prototype>
{
    public string Name { get; set; } = name;
    public List<string> Items { get; set; } = [];
    public Dictionary<string, object> Properties { get; set; } = [];
    public NestedObject Nested { get; set; } = new();

    // 浅拷贝 - 引用类型字段仍然指向原始对象
    public Prototype ShallowCopy()
    {
        // MemberwiseClone 在这里也是安全的强制转换
        return (Prototype)this.MemberwiseClone();
    }

    // 深拷贝 - 完全独立的副本
    public Prototype DeepClone()
    {
        Prototype clone = new(Name)
        {
            // 深拷贝集合
            Items = [.. this.Items],

            // 深拷贝字典
            Properties = new Dictionary<string, object>(this.Properties),

            // 深拷贝嵌套对象
            Nested = this.Nested.DeepClone()
        };

        return clone;
    }

    public override string ToString()
    {
        string itemsStr = string.Join(", ", Items);
        string propsStr = string.Join(", ", Properties.Select(p => $"{p.Key}={p.Value}"));
        return $"ComplexPrototype: Name={Name}, Items=[{itemsStr}], Properties=[{propsStr}], Nested={Nested}";
    }
}

// 嵌套对象
public class NestedObject : IPrototype<NestedObject>
{
    public string Data { get; set; } = "Default Data";
    public int Counter { get; set; } = 0;

    public NestedObject ShallowCopy()
    {
        return new NestedObject
        {
            Data = this.Data,
            Counter = this.Counter
        };
    }

    public NestedObject DeepClone()
    {
        return ShallowCopy();
    }

    public override string ToString()
    {
        return $"NestedObject(Data={Data}, Counter={Counter})";
    }
}

// 原型模式的优点：
// 1. 可以克隆对象，而无需与它们所属的具体类相耦合
// 2. 可以克隆预生成原型，避免反复运行初始化代码
// 3. 可以更方便地生成复杂对象
// 4. 可以用继承以外的方式来处理复杂对象的不同配置

// 原型模式的缺点：
// 1. 克隆包含循环引用的复杂对象可能会非常麻烦
// 2. 深拷贝可能会消耗较多内存和处理时间