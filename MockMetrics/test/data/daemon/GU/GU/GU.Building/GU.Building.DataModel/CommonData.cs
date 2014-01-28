using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA.Interface;
using Common.DA;
using BLToolkit.Data.Sql.SqlProvider;

namespace GU.Building.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Реплика.
    /// </summary>
    [Serializable]
    [TableName("gu_building.hist_data")]
    public class CommonData : ICommonData
    {
        /// <summary>
        /// Класс, предназначенный для хранения данных сущности Реплика.
        /// </summary>
        /// <remarks>
        /// Используется для управления репликацией доменного объекта, реализующего интерфейс <c>IPersistentObject</c>.
        /// </remarks>
        public CommonData()
        {
        }

        /// <summary>
        /// Первичный ключ.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_building.hist_data_seq")]
        [MapField("hist_data_id")]
        public int Id { get; set; }

        /// <summary>
        /// Имя реплицируемого доменного объекта.
        /// </summary>
        [MapField("entity")]
        public string Entity { get; set; }

        /// <summary>
        /// Значение первичного ключа доменного объекта.
        /// </summary>
        [MapField("key_value")]
        public string KeyValue { get; set; }

        /// <summary>
        /// Тип действия при репликации.
        /// </summary>
        [MapField("action_type")]
        public ReplicaActionType Action { get; set; }

        /// <summary>
        /// Дата помещения реплики в БД.
        /// </summary>
        [MapField("stamp")]
        public DateTime Stamp { get; set; }

        /// <summary>
        /// Идентификатор базы, на которой доменный объект появился или изменились его данные.
        /// </summary>
        [MapField("host")]
        public string Host { get; set; }

        /// <summary>
        /// Идентификатор базы, на котором данные доменного объекта должны появиться.
        /// </summary>
        [MapField("targethost")]
        public string TargetHost { get; set; }
        
        /// <summary>
        /// Имя пользователя, во время работы которого доменный объект появился или изменились его данные.
        /// </summary>
        [MapField("user_name")]
        public string User { get; set; }

        /// <summary>
        /// Версия приложения, в которой доменный объект появился или изменились его данные.
        /// </summary>
        [MapField("app_version")]
        public string AppVersion { get; set; }

        /// <summary>
        /// Флаг указывающий на то, была ли реплика обработана.
        /// </summary>
        [MapField("is_sended")]
        public bool IsSended { get; set; }

        /// <summary>
        /// Имя конфигурации подключения к БД, на которой был заведён доменный объект
        /// </summary>
        [MapIgnore]
        public string ConfigurationString { get; set; }

        string IDomainObject.GetKeyValue()
        {
            throw new NotImplementedException();
        }

        void IDomainObject.SetKeyValue(object val)
        {
            throw new NotImplementedException();
        }
    }
}
