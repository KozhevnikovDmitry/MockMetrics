using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Notifying
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Уведомление заявителя
    /// </summary>
    [TableName("gumz.notice")] 
    public abstract class Notice : IdentityDomainObject<Notice>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("file_scenario_step_id")] 
        public abstract override int Id { get; set; }

        /// <summary>
        /// Дата отправления уведомления
        /// </summary>
        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        /// <summary>
        /// Электронный адрес уведомления
        /// </summary>
        [MapField("email")]
        public abstract string Email { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [MapField("address")]
        public abstract string Address { get; set; }

        /// <summary>
        /// Реквизиты почтового отправления
        /// </summary>
        [MapField("post_requisites")]
        public abstract string PostRequisites { get; set; }

        /// <summary>
        /// Содержание уведомления
        /// </summary>
        [MapField("body")]
        public abstract string Body { get; set; }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        [MapField("notice_type")]
        public abstract NoticeType NoticeType { get; set; }
    }
}
