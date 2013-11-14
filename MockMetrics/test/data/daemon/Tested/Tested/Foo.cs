namespace Tested
{
    public class Foo
    {
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
