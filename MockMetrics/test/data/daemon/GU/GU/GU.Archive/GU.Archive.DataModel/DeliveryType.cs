using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA.Interface;
using Common.DA;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Способ доставки корреспонденции
    /// </summary>
    public enum DeliveryType
    {
        /// <summary>
        /// письмо
        /// </summary>
        Mail = 1,

        /// <summary>
        /// факс
        /// </summary>
        Fax = 2,

        /// <summary>
        /// телеграмма
        /// </summary>
        Telegram = 3,

        /// <summary>
        /// электронная почта
        /// </summary>
        Email = 4,

        /// <summary>
        /// лично
        /// </summary>
        Personally = 6,

        /// <summary>
        /// Гос.услуги
        /// </summary>
        GU = 7
    }
}
