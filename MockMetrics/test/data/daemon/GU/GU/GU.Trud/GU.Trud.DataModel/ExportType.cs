using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.Trud.DataModel
{
    [TableName("gu_trud.export_type")]
    public abstract class ExportType : IdentityDomainObject<ExportType>, IPersistentObject
    {
        ///<summary>
        /// Первичный ключ сущности
        ///</summary>
        [PrimaryKey]
        [MapField("export_type_id")]
        public abstract override int Id { get; set; }

        ///<summary>
        /// Наименование типа выгрузки
        ///</summary>
        [MapField("export_type_name")]
        public abstract string Name { get; set; }

    }
}
