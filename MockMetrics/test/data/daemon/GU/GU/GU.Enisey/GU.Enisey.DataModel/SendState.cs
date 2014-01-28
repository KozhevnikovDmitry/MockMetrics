using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GU.Enisey.DataModel
{
    public enum SendState
    {
        None = 1,
        Success = 2,
        ConvertFail = 3,
        SendFail = 4,
        ProcessingFail = 5
    }
}
