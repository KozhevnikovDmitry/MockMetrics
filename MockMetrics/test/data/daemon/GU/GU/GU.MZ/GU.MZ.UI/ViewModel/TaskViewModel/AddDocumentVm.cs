using System;
using System.Collections.ObjectModel;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.ValidationViewModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{
    public class AddDocumentVm : ValidateableVM
    {
        private readonly ObservableCollection<AddDocumentVm> _parentList;

        public AddDocumentVm(string name, int quantity, ObservableCollection<AddDocumentVm> parentList)
        {
            _isValidateable = true;
            _parentList = parentList;
            Name = name;
            Quantity = quantity;
            RemoveDocumentCommand = new DelegateCommand(RemoveDocument);
        }

        #region Binding Properties
        
        public string Name { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Binding Commands

        public DelegateCommand RemoveDocumentCommand { get; set; }

        private void RemoveDocument()
        {
            try
            {
                _parentList.Remove(this);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        #endregion

        #region IValidateableVM

        public override string this[string columnName]
        {
            get
            {
                if (!AllowValidate)
                {
                    return null;
                }


                switch (columnName)
                {
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(Name.Trim()))
                            {
                                return "Наименование документа должно быть заполнено";
                            }
                            break;
                        }
                    case "Quantity":
                        {
                            if (Quantity <= 0)
                            {
                                return "Количество документов - целое число больше 0";
                            }
                            break;
                        }
                }

                return null;
            }
        }

        #endregion
    }
}