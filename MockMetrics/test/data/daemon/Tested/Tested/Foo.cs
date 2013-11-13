using System.IO;

namespace Tested
{
    public class Foo
    {
        public Foo()
        {
            using (MemoryStream m1 = new MemoryStream(),
                                m2 = new MemoryStream(),
                                m3 = new MemoryStream())
            {
                
            }
        }

        public bool GetBool(bool item)
        {
            var t = new Foo() {StringProperty = "sdf"};

            return !item;
        }

        public string StringProperty { get; set; }
    }

    public class Bar
    {
        public bool SomeBool { get; set; }
    }
}
