using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.FileScenario;

namespace PostGrad.Core.DomainModel.Dossier
{
    /// <summary>
    /// Класс предназанченный для хранения данных сущности Результат предоставления услуги по итогу ведения тома
    /// </summary>
    [TableName("gumz.file_service_result")] 
    public abstract class DossierFileServiceResult : IdentityDomainObject<DossierFileServiceResult>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.file_service_result_seq")]
        [MapField("file_service_result_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Статус предоставленности
        /// </summary>
        [MapField("grant_way")]
        public abstract string GrantWay { get; set; }

        /// <summary>
        /// Дата предоставления
        /// </summary>
        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        /// <summary>
        /// Примечение к результату
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }

        /// <summary>
        /// Id результата предоставления услуги
        /// </summary>
        [MapField("service_result_id")]
        public abstract int ServiceResultId { get; set; }

        /// <summary>
        /// Рузультат предоставления услуги
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ServiceResultId", OtherKey = "Id", CanBeNull = false)]
        public abstract ServiceResult ServiceResult { get; set; }
    }
}
