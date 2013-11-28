﻿using System;

namespace Tested
{
    public class Foo
    {
        public bool GetBool(bool item)
        {
            return !item;
        }

        public Bar GetBar(Bar item)
        {
            return item;
        }

        public string StringProperty { get; set; }

        public Bar Bar { get; set; }

        public event Action Act;
    }

    public class Bar
    {
        public bool SomeBool { get; set; }
    }
}
