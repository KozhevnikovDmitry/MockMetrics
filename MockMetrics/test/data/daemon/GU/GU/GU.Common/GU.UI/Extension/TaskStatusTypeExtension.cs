using GU.DataModel;

namespace GU.UI.Extension
{
    public static class TaskStatusTypeExtension
    {
        public static string GetIconPath(this TaskStatusType taskStatusType)
        {
            switch (taskStatusType)
            {
                case TaskStatusType.Accepted:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Entered_32x32.png";
                    }
                case TaskStatusType.CheckupWaiting:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Checkup_32x32.png";
                    }
                case TaskStatusType.Done:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Check_32x32.png";
                    }
                case TaskStatusType.NotFilled:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/New_32x32.png";
                    }
                case TaskStatusType.Ready:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Information_32x32.png";
                    }
                case TaskStatusType.Rejected:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Remove_32x32.png";
                    }
                case TaskStatusType.Working:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Working_32x32.png";
                    }
                default:
                    {
                        return "/GU.UI;component/Resources/Icons/32X32/Help_32x32.png";
                    }
            }
        }
    }
}
