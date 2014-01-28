using System.Collections.Generic;
using System.Linq;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerShortVM
    {
        private Claim _claim;
        private readonly List<DisabilityType> DisabilityTypeList;

        public  DeclarerShortVM (Claim claim)
        {
            _claim = claim;
            DisabilityTypeList = HqFacade.GetDictionaryManager().GetDictionary<DisabilityType>();
        }

        /// <summary>
        /// ФИО заявителя
        /// </summary>
        public string ClaimDeclarerFio 
        {
            get { return _claim.Declarer.Sname + " " + _claim.Declarer.Name + " " + _claim.Declarer.Patronymic; }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public string BirthDate
        {
            get { return _claim.Declarer.BirthDate.ToString("dd.MM.yyyy"); }
        }

        /// <summary>
        /// Информация об инвалидности
        /// </summary>
        public string DeclarerDisability 
        {
            get { return _claim.Declarer.Disability == null ? "нет" : DisabilityTypeList.Single(t => t.Id == _claim.Declarer.Disability.DisabilityTypeId).ToString(); }
        }

        /// <summary>
        /// Колличество родтсвенников заявителя
        /// </summary>
        public int RelativesCount 
        {
            get { return _claim.Relatives.Count(); }
        }
    }
}
