using System;

namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Интерфейс классов доменных объектов хранящих историю изменений других доменных объектов.
    /// </summary>
    public interface ICommonData : IDomainObject
    {
        /// <summary>
        /// Первичный ключ.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Имя реплицируемого доменного объекта.
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// Значение первичного ключа доменного объекта.
        /// </summary>
        string KeyValue { get; set; }
        
        /// <summary>
        /// Дата помещения реплики в БД.
        /// </summary>
        DateTime Stamp { get; set; }

        /// <summary>
        /// Идентификатор базы, на которой доменный объект появился или изменились его данные.
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Идентификатор базы, на котором данные доменного объекта должны появиться.
        /// </summary>
        string TargetHost { get; set; }

        /// <summary>
        /// Имя пользователя, во время работы которого доменный объект появился или изменились его данные.
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Версия приложения, в которой доменный объект появился или изменились его данные.
        /// </summary>
        string AppVersion { get; set; }

        /// <summary>
        /// Флаг указывающий на то, была ли реплика обработана.
        /// </summary>
        bool IsSended { get; set; }

        /// <summary>
        /// Имя конфигурации подключения к БД, на которой был заведён доменный объект
        /// </summary>
        string ConfigurationString { get; set; }
    }
}
