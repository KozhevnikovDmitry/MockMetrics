using System;
using System.Collections.Generic;

using Common.BL.Validation;

using GU.DataModel;
using GU.DataModel.Inquiry;

namespace GU.BL.Policy.Interface
{
    public interface IInquiryPolicy
    {
        ValidationErrorInfo Validate(Inquiry inquiry);

        ValidationErrorInfo CanSave(Inquiry inquiry);

        Inquiry CreateEmptyInquiry(InquiryType inquiryType, Task task = null);
    }
}
