using System;

using Common.BL.DomainContext;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.ReportMapping
{
    /// <summary>
    /// Базовый класс для классов, инкапсулирующих отчёты, получающие данные из БД
    /// </summary>
    public abstract class DbReport : DomainDependent, IReport
    {

        /// <summary>
        ///  Базовый класс для классов, инкапсулирующих отчёты, получающие данные из БД
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        protected DbReport(IDomainContext domainContext)
            : base(domainContext)
        {

        }

        /// <summary>
        /// Путь к файлу с версткой отчёта
        /// </summary>
        public string ViewPath { get; protected set; }

        /// <summary>
        /// Псевдоним данных в отчёте
        /// </summary>
        public virtual string DataAlias
        {
            get
            {
                return "data";
            }
        }

        /// <summary>
        /// Возвращает данные для отчта
        /// </summary>
        /// <returns>Данные для отчёта</returns>
        public object RetrieveData()
        {
            try
            {
                using (var dbManager = this.GetDbManager())
                {
                    return RetrieveOperation(dbManager);
                }
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ReportDataRetrieveException("Ошибка при получении данных отчёта", ex);
            }
        }

        /// <summary>
        /// Возвращает данные для отчёта
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Данные для отчёта</returns>
        protected abstract object RetrieveOperation(IDomainDbManager dbManager);
    }
}
