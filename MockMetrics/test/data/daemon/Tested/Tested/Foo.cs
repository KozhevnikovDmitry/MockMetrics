using System;

namespace Tested
{
    public class Foo
    {
        public Foo()
        {
            Act += () => { };
        }

        public bool GetBool(bool item)
        {
            var t = new Foo() {StringProperty = "sdf"};

            return !item;
        }

        public string StringProperty { get; set; }

        public event Action Act;
    }

    public class Bar
    {
        public bool SomeBool { get; set; }
    }
}
