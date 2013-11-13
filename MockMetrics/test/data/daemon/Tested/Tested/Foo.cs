namespace Tested
{
    public class Foo
    {
        public bool GetBool(bool item)
        {
            return !item;
        }
    }

    public class Bar
    {
        public bool SomeBool { get; set; }
    }
}
