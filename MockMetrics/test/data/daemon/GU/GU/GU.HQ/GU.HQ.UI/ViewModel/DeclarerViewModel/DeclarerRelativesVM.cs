using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerRelativesVM : AbstractListVM<DeclarerRelative>
    {
        private readonly Claim _claim;

        public DeclarerRelativesVM(Claim claim)
            : base(claim.Relatives)
        {
            _claim = claim;
        }

        /// <summary>
        /// Добавляет нового родственника в список
        /// </summary>
        protected override void AddItem()
        {
            _claim.Relatives.Add(DeclarerRelative.CreateInstance());
        }

        /// <summary>
        /// Удаление из списка родственников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(((DeclarerRelative)e.Result).Person.Sname)
                   || !String.IsNullOrEmpty(((DeclarerRelative)e.Result).Person.Name)
                   || !String.IsNullOrEmpty(((DeclarerRelative)e.Result).Person.Patronymic))
                {
                    var dlr =
                        NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить информацию о родственнике \"" +
                                                     ((DeclarerRelative) e.Result).Person.Sname + " " +
                                                     ((DeclarerRelative) e.Result).Person.Name + " " +
                                                     ((DeclarerRelative) e.Result).Person.Patronymic + "\"?");
                    if (dlr == MessageBoxResult.Yes)
                    {
                        _claim.Relatives.Remove((DeclarerRelative) e.Result);
                    }
                }
                else
                {
                    _claim.Relatives.Remove((DeclarerRelative)e.Result);
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
