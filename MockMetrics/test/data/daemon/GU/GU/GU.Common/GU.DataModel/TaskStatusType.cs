using System.ComponentModel;

namespace GU.DataModel
{
    [DefaultValue(TaskStatusType.None)]
    public enum TaskStatusType
    {
        None = 0,
        NotFilled = 1,
        CheckupWaiting = 2,
        Accepted = 3,
        Working = 4,
        Ready = 5,
        Done = 6,
        Rejected = 7
    }
}