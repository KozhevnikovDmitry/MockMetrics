using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostGrad.Core.BL
{
    public interface ILifetimeScope
    {
        T Resolve<T>();
    }
}
