using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatterns.BuilderPattern
{
    // Builder 接口指定了创建 Product 对象不同部分的方法。
    public interface IBuilder
    {
        void BuildPartA();

        void BuildPartB();

        void BuildPartC();
    }


    // 具体建造者类遵循建造者接口，并提供建造步骤的具体实现。
    // 你的程序中可能包含多个不同的建造者变体，它们的实现方式也各不相同。
    public class ConcreteBuilder : IBuilder
    {
        private Product _product = new();

        // 新的构建器实例应该包含一个空白产品对象，用于进一步组装。
        public ConcreteBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._product = new();
        }

        // 所有生产步骤都使用相同的产品实例。
        public void BuildPartA()
        {
            this._product.Add("PartA1");
        }

        public void BuildPartB()
        {
            this._product.Add("PartB1");
        }

        public void BuildPartC()
        {
            this._product.Add("PartC1");
        }

        // 具体建造者应该提供自己的方法来检索结果。
        // 这是因为不同类型的建造者可能会创建完全不同的、不遵循相同接口的产品。
        // 因此，这些方法不能在基础建造者接口中声明（至少在静态类型编程语言中是这样）。
        //
        // 通常，在将最终结果返回给客户端后，构建器实例应该准备好开始生成另一个产品。
        // 这就是为什么在 `GetProduct` 方法体的末尾调用重置方法是一种常见做法。
        // 然而，这种行为并不是强制性的，你可以让构建器在客户端代码显式调用重置之前
        // 等待处理上一个结果。
        public Product GetProduct()
        {
            Product result = this._product;

            this.Reset();

            return result;
        }
    }

    // 只有当你的产品相当复杂并且需要大量配置时，使用建造者模式才是有意义的。
    //
    // 与其他创建型模式不同，不同的具体建造者可以生成不相关的产品。
    // 换句话说，各个建造者的结果可能并不总是遵循相同的接口。
    public class Product
    {
        private readonly List<object> _parts = [];

        public void Add(string part)
        {
            _parts.Add(part);
        }

        public string ListParts()
        {
            if (_parts.Count == 0)
            {
                return "Product parts: \n";
            }

            string str = string.Empty;

            for (int i = 0; i < this._parts.Count; i++)
            {
                str += _parts[i] + ", ";
            }

            str = str.Remove(str.Length - 2); // removing last ", "

            return "Product parts: " + str + "\n";
        }
    }

    // Director 仅负责按特定顺序执行构建步骤。
    // 当需要按照特定顺序或配置生产产品时，它非常有用。
    // 严格来说，Director 类是可选的，因为客户端可以直接控制构建器。
    public class Director(IBuilder builder)
    {
        public IBuilder Builder { private get; set; } = builder;

        // Director 可以使用相同的构建步骤构建多个产品变体。
        public void BuildMinimalViableProduct()
        {
            this.Builder.BuildPartA();
        }

        public void BuildFullFeaturedProduct()
        {
            this.Builder.BuildPartA();
            this.Builder.BuildPartB();
            this.Builder.BuildPartC();
        }
    }
}