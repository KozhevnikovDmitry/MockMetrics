using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Класс представляющий Тип объекта лицензии
    /// </summary>
    [TableName("license_object_type")]
    public class LicenseObjcetType : IDomainObject
    {
        #region IDomainObject

        public string GetKeyValue()
        {
            return Id.ToString();
        }

        public void SetKeyValue(object val)
        {
            Id = Convert.ToInt32(val);
        }

        #endregion

        /// <summary>
        /// Id сущности
        /// </summary>
        [PrimaryKey]
        [MapField("license_object_type_id")]
        public int Id { get; set; }

        /// <summary>
        /// Наименование типа объекта лицензии
        /// </summary>
        [MapField("license_object_type_name")]
        public string Name { get; set; }
    }
}
