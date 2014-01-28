using System;
using System.Linq;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.DataModel.Licensing;
using Common.Types.Exceptions;
using GU.MZ.BL.Reporting.Data;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Отчёт по выданным лицензиям в разрезе видов деятельности".
    /// </summary>
    public class LicenseByActivityReport : BaseReport
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        private readonly string _username;

        private readonly Func<IDomainDbManager> _getDb;

        /// <summary>
        /// Класс отчёт "Отчёт по выданным лицензиям в разрезе видов деятельности".
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        public LicenseByActivityReport(DbUser dbUser, Func<IDomainDbManager> getDb)
        {
            _username = dbUser.UserText;
            _getDb = getDb;
            ViewPath = "Reporting/View/GU.MZ/LicenseByActivityReport.mrt";
        }

        /// <summary>
        /// Возвращает данные для отчёта "Отчёт по выданым лицензиям в разрезе видов деятельности".
        /// </summary>
        /// <returns>Объект с информацией для отчёта</returns>
        /// <exception cref="GUException">Пробрасываемая ошибка</exception>
        /// <exception cref="BLLException">Ошибка при формировании данных отчёта по лицензия в разрезе видов деятельности</exception>
        public override object RetrieveData()
        {
            try
            {
                using (var dbManager = _getDb())
                {
                    var licenses = dbManager.GetDomainTable<License>();
                    return new LicenseByActivity
                    {
                        LicenseByActivityStatList = (from license in licenses.GroupBy(l => l.LicensedActivityId).Select(l => new { activityId = l.Key, count = l.Count() })
                                                     join activity in dbManager.GetDomainTable<LicensedActivity>() on license.activityId equals activity.Id
                                                     select new LicenseByActivity.LicenseByActivityStat
                                                     {
                                                         ActivityName = activity.Name,
                                                         Count = license.count
                                                     }).ToList(),
                        Username = _username
                    };
                }
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ReportDataRetrieveException("Ошибка при формировании данных отчёта по лицензия в разрезе видов деятельности", ex);
            }
        }
    }
}
