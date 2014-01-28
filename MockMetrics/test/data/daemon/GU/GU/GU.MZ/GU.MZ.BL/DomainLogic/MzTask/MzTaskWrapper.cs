using Common.Types.Exceptions;
using GU.DataModel;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// MZ-обёртка заявки, маппит поля заявки в MZ-значимые данные
    /// </summary>
    public class MzTaskWrapper : IMzTaskWrapper
    {
        private readonly Task _task;

        public MzTaskWrapper(Task task)
        {
            _task = task;
        }

        /// <summary>
        /// Тип услуги лицензирования по виду лицензионной деятельности
        /// </summary>
        public LicenseServiceType LicenseServiceType
        {
            get
            {
                if (_task.ServiceId == 1 ||
                    _task.ServiceId == 2 ||
                    _task.ServiceId == 3 ||
                    _task.ServiceId == 4 ||
                    _task.ServiceId == 5)
                {
                    return LicenseServiceType.Drug;
                }

                if (_task.ServiceId == 6 ||
                    _task.ServiceId == 7 ||
                    _task.ServiceId == 8 ||
                    _task.ServiceId == 9 ||
                    _task.ServiceId == 10)
                {
                    return LicenseServiceType.Med;
                }

                if (_task.ServiceId == 11 ||
                    _task.ServiceId == 12 ||
                    _task.ServiceId == 13 ||
                    _task.ServiceId == 14 ||
                    _task.ServiceId == 15)
                {
                    return LicenseServiceType.Farm;
                }

                throw new BLLException(string.Format("Услуга не принадлежит предметной области Лицензирование. ServiceId = [{0}]", _task.ServiceId));
            }
        }

        /// <summary>
        /// Тип услуги лицензирования по типу действия над лицензией
        /// </summary>
        public LicenseActionType LicenseActionType
        {
            get
            {
                if (_task.ServiceId == 1 ||
                    _task.ServiceId == 6 ||
                    _task.ServiceId == 11)
                {
                    return LicenseActionType.New;
                }

                if (_task.ServiceId == 2 ||
                    _task.ServiceId == 7 ||
                    _task.ServiceId == 12)
                {
                    return LicenseActionType.Renewal;
                }
                if (_task.ServiceId == 3 ||
                    _task.ServiceId == 8 ||
                    _task.ServiceId == 13)
                {
                    return LicenseActionType.Stop;
                }

                if (_task.ServiceId == 4 ||
                    _task.ServiceId == 9 ||
                    _task.ServiceId == 14)
                {
                    return LicenseActionType.Duplicate;
                }

                if (_task.ServiceId == 5 ||
                    _task.ServiceId == 10 ||
                    _task.ServiceId == 15)
                {
                    return LicenseActionType.Copy;
                }

                throw new BLLException(string.Format("Услуга не принадлежит предметной области Лицензирование. ServiceId = [{0}]", _task.ServiceId));
            }
        }
    }
}