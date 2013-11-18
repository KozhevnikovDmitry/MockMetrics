using System;
using System.Linq;

namespace Tested
{
    public class Foo
    {
        public Foo()
        {
           
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
