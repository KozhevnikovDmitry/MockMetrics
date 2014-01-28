using System;
using System.Linq;
using Common.BL.ReportMapping;

using GU.MZ.BL.Reporting.Data;
using Common.Types.Exceptions;
using GU.DataModel;
using Common.DA.Interface;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Отчёт по заявлениям в электронной форме в разрезе услуг".
    /// </summary>
    public class StatementByServiceReport : BaseReport
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Только заведённые в электронной форме.
        /// </summary>
        private bool _justElectronic;

        private readonly Func<IDomainDbManager> _getDb;

        /// <summary>
        /// Класс отчёт "Отчёт по заявлениям в электронной форме в разрезе услуг".
        /// </summary>
        public StatementByServiceReport(DbUser dbUser, Func<IDomainDbManager> _getDb)
        {
            _username = dbUser.UserText;
            this._getDb = _getDb;
            ViewPath = "Reporting/View/GU.MZ/StatementByServiceElcReport.mrt";
        }

        /// <summary>
        /// Возвращает данные для отчёта "Отчёт по выданым лицензиям в разрезе видов деятельности".
        /// </summary>
        /// <returns>Объект с информацией для отчёта</returns>
        /// <exception cref="BLLException">Ошибка при формировании данных отчёта по заявлениям в разрезе услуг</exception>
        /// <exception cref="GUException">Пробрасываемая ошибка</exception>
        public override object RetrieveData()
        {
            try
            {
                using (var dbManager = _getDb())
                {
                    var tasks = dbManager.GetDomainTable<Task>();
                    return new StatementByService
                    {
                        StatementByServiceStatList = (from task in tasks.GroupBy(t => t.ServiceId)
                                                                        .Select(t => new { serviceId = t.Key, count = t.Count() })
                                                      join service in dbManager.GetDomainTable<Service>() on task.serviceId equals service.Id
                                                      select new StatementByService.StatementByServiceStat
                                                      {
                                                          ServiceName = service.Name,
                                                          ServiceGroupName = service.ServiceGroup.ServiceGroupName,
                                                          Count = task.count
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
                throw new ReportDataRetrieveException("Ошибка при формировании данных отчёта по заявлениям в разрезе услуг", ex);
            }
        }

        public IReport Initialize(bool justElectronic)
        {
            _justElectronic = justElectronic;
            return this;
        }
    }
}
