using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockMetrics.Tests
{
    public class Expression : Inverter
    {
        public Expression()
            : base()
        {
            
        }

        public void SomeMethod(int arg = 100500)
        {
            var str = new string('a', 1);
            switch (str)
            {
                case(""):
                {
                    break;
                }
                default:
                {
                    break;
                }
            }

            return;
        }
    }
}
