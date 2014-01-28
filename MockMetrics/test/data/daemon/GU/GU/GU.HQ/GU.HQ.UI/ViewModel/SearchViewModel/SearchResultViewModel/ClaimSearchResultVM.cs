using System;
using System.Collections.Generic;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class ClaimSearchResultVM : AbstractSearchResultVM<Claim>
    {
        /// <summary>
        /// Класс ViewModel для представления реузльтата поиска заявков.
        /// </summary>
        /// <param name="entity">Отображаемая заявка</param>
        public ClaimSearchResultVM(Claim entity)
            : base(entity)
        { }


        /// <summary>
        /// Инициализация полей привязки.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                ListClaimStatus = HqFacade.GetDictionaryManager().GetEnumDictionary<ClaimStatusType>();
                ClaimDate = Result.ClaimDate.Value.ToString("dd.MM.yyyy HH:mm:ss");

                DeclarerFIO = Result.Declarer == null ?" ": Result.Declarer.Sname + " " + Result.Declarer.Name + " " + Result.Declarer.Patronymic;
                ClaimStatus = ListClaimStatus[(int)Result.CurrentStatusTypeId];
                Note = Result.Note;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        /// <summary>
        ///  
        /// </summary>
        public string ClaimDate { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string DeclarerFIO { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public object ClaimStatus { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string Note { get; set; }


        public Dictionary<int, string> ListClaimStatus { get; private set; }

        #endregion
    }
}
