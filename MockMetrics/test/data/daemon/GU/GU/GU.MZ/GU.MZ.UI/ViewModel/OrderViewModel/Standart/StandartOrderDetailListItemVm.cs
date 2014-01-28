using System;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Standart
{
    public class StandartOrderDetailListItemVm : SmartListItemVm<StandartOrderDetail>
    {
        #region Binding Properties

        public string SubjectIdName
        {
            get
            {
                return Entity.SubjectIdName;
            }
        }

        public string SubjectStampName
        {
            get
            {
                return Entity.SubjectStampName;
            }
        }

        public string SubjectId
        {
            get
            {
                return Entity.SubjectId;
            }
            set
            {
                if (Entity.SubjectId != value)
                {
                    Entity.SubjectId = value;
                    RaisePropertyChanged(() => SubjectId);
                }
            }
        }

        public DateTime? SubjectStamp
        {
            get
            {
                return Entity.SubjectStamp;
            }
            set
            {
                if (Entity.SubjectStamp != value)
                {
                    Entity.SubjectStamp = value;
                    RaisePropertyChanged(() => SubjectStamp);
                }
            }
        }

        public string FullName
        {
            get
            {
                return Entity.FullName;
            }
            set
            {
                if (Entity.FullName != value)
                {
                    Entity.FullName = value;
                    RaisePropertyChanged(() => FullName);
                }
            }
        }

        public string ShortName
        {
            get
            {
                return Entity.ShortName;
            }
            set
            {
                if (Entity.ShortName != value)
                {
                    Entity.ShortName = value;
                    RaisePropertyChanged(() => ShortName);
                }
            }
        }

        public string FirmName
        {
            get
            {
                return Entity.FirmName;
            }
            set
            {
                if (Entity.FirmName != value)
                {
                    Entity.FirmName = value;
                    RaisePropertyChanged(() => FirmName);
                }
            }
        }

        public string Inn
        {
            get
            {
                return Entity.Inn;
            }
            set
            {
                if (Entity.Inn != value)
                {
                    Entity.Inn = value;
                    RaisePropertyChanged(() => Inn);
                }
            }
        }

        public string Ogrn
        {
            get
            {
                return Entity.Ogrn;
            }
            set
            {
                if (Entity.Ogrn != value)
                {
                    Entity.Ogrn = value;
                    RaisePropertyChanged(() => Ogrn);
                }
            }
        }

        public string Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        public string Comment
        {
            get
            {
                return Entity.Comment;
            }
            set
            {
                if (Entity.Comment != value)
                {
                    Entity.Comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => SubjectIdName);
            RaisePropertyChanged(() => SubjectStampName);
            RaisePropertyChanged(() => Inn);
            RaisePropertyChanged(() => Ogrn);
            RaisePropertyChanged(() => Comment);
        }
    }
}
