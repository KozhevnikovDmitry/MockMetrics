using System.ComponentModel;

namespace GU.DataModel.Inquiry
{
    [DefaultValue(InquiryStatusType.None)]
    public enum InquiryStatusType
    {
        None = 0,
        NotFilled = 1,
        Sending = 2,
        Accepted = 3,
        Invalid = 4,
        Rejected = 5,
        FailedToGetResponse = 6,
        FailedToSendRequest = 7,
        ResponseReceived = 8
    }
}