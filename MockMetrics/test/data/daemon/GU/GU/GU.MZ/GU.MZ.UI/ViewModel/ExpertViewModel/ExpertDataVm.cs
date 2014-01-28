using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertViewModel
{
    /// <summary>
    /// ����� ViewModel ��� ����������� ������ �������
    /// </summary>
    public class ExpertDataVm : SmartValidateableVm<Expert>
    {
        
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// ����� ViewModel ��� ����������� ������ �������
        /// </summary>
        public ExpertDataVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(Expert entity)
        {
            base.Initialize(entity);
            AccreditateActivityList = _dictionaryManager.GetDictionary<AccreditateActivity>();
            ExpertStateTypeList = _dictionaryManager.GetEnumDictionary<ExpertStateType>();
        }

        #region Binding Properties

        /// <summary>
        /// ���������� ��� ������������� ����� ������������� �� ������������
        /// </summary>
        public string AccreditateDocumentNumber
        {
            get
            {
                return Entity.AccreditateDocumentNumber;
            }
            set
            {
                if (Entity.AccreditateDocumentNumber != value)
                {
                    Entity.AccreditateDocumentNumber = value;
                    RaisePropertyChanged(() => AccreditateDocumentNumber);
                }
            }
        }

        /// <summary>
        /// ���������� ��� ������������� ���� ��������� ������������ ��������
        /// </summary>
        public DateTime? AccreditationDueDate
        {
            get
            {
                return Entity.AccreditationDueDate;
            }
            set
            {
                if (Entity.AccreditationDueDate != value)
                {
                    Entity.AccreditationDueDate = value;
                    RaisePropertyChanged(() => AccreditationDueDate);
                }
            }
        }

        /// <summary>
        /// ���������� ��� ������������� Id ������������ �� ������� ������������ �������
        /// </summary>
        public int AccreditateActivityid
        {
            get
            {
                return Entity.AccreditateActivityid;
            }
            set
            {
                if (Entity.AccreditateActivityid != value)
                {
                    Entity.AccreditateActivityid = value;
                    RaisePropertyChanged(() => AccreditateActivityid);
                }
            }
        }

        /// <summary>
        /// ���������� ������ �������������, �� ������� ����� ����������������� ��������
        /// </summary>
        public List<AccreditateActivity> AccreditateActivityList { get; private set; }
        
        /// <summary>
        /// ���������� ��� �������������� ��� ��������� ��c�����
        /// </summary>
        public int ExpertStateType
        {
            get
            {
                return (int)Entity.ExpertStateType;
            }
            set
            {
                if ((int)Entity.ExpertStateType != value)
                {
                    Entity.ExpertStateType = (ExpertStateType)value;
                    RaisePropertyChanged(() => ExpertStateType);
                }
            }
        }

        /// <summary>
        /// ���������� ������� ����� ��������� ��������
        /// </summary>
        public Dictionary<int, string> ExpertStateTypeList { get; private set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => ExpertStateType);
        }
    }
}