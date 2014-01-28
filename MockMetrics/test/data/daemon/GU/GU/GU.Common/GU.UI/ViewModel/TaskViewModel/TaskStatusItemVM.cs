using System;

using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI.ViewModel.ListViewModel;
using GU.DataModel;
using GU.UI.Extension;

namespace GU.UI.ViewModel.TaskViewModel
{
    public class TaskStatusItemVM : AbstractListItemVM<TaskStatus>
    {
        public TaskStatusItemVM(TaskStatus entity, bool isValidateable)
            : base(entity, isValidateable)
        {
        }

        protected override void Initialize()
        {
            try
            {
                StatusString = string.Format("{0} {1} {2}",
                                             Item.Stamp.ToLongDateString(),
                                             Item.Stamp.ToShortTimeString(),
                                             BL.GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>()[(int)Item.State]);
                IconPath = Item.State.GetIconPath();
                Note = Item.Note;
                UserName = Item.User.UserText;
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        public string StatusString { get; set; }

        public string IconPath { get; set; }

        public string Note { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}
