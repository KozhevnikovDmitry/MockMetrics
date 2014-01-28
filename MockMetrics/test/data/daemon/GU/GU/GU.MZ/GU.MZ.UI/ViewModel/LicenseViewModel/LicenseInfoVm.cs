using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseInfoVm : EntityInfoVm<License>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseInfoVm(IDomainDataMapper<License> entityMapper, IDictionaryManager dictionaryManager) 
            : base(entityMapper)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region Binding Properties

        public string RegNumber
        {
            get { return Entity.RegNumber; }
        }

        public string GrantOrderRegNumber
        {
            get { return Entity.GrantOrderRegNumber; }
        }

        public string GrantDate
        {
            get { return Entity.GrantDate.HasValue ? Entity.GrantDate.Value.ToLongDateString() : "Не указано"; }
        }

        public string DueDate
        {
            get { return Entity.DueDate.HasValue ? Entity.DueDate.Value.ToLongDateString() : "Бессрочно"; }
        }

        public string GrantOrderStamp
        {
            get { return Entity.GrantOrderStamp.HasValue ? Entity.GrantOrderStamp.Value.ToLongDateString() : "Не указано"; }
        }

        public string LicensedActivity
        {
            get { return _dictionaryManager.GetDictionaryItem<LicensedActivity>(Entity.LicensedActivityId).Name; }
        }

        public string LicensiarHeadName
        {
            get { return Entity.LicensiarHeadName; }
        }

        public string LicensiarHeadPosition
        {
            get { return Entity.LicensiarHeadPosition; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Лицензия №{0} от {1}", RegNumber, GrantDate);
        }
    }
}