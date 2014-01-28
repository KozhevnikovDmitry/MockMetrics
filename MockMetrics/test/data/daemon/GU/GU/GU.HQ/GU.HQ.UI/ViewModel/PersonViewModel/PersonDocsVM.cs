using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonDocsVM : AbstractListVM<PersonDoc>
    {
        private readonly Person _person;

        public PersonDocsVM(Person person) 
            : base(person.Documents)
        {
            _person = person;
        }

        /// <summary>
        /// Добавление новго документа
        /// </summary>
        protected override void AddItem()
        {
            _person.Documents.Add(PersonDoc.CreateInstance());
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {   
                var dlr = NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить Документ");

                if (dlr == MessageBoxResult.Yes)
                {
                    _person.Documents.Remove((PersonDoc)e.Result);
                }
                
            }
            catch (BLLException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }
        
    }
}
