using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Тип корреспонденции
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// Входящие
        /// </summary>
        Incoming = 1,

        /// <summary>
        /// Исходящие
        /// </summary>
        Outgoing = 2,

        /// <summary>
        /// Напоминания
        /// </summary>
        Remander = 3,

        /// <summary>
        /// Приказы
        /// </summary>
        Order = 4,

        /// <summary>
        /// Командировки
        /// </summary>
        Assignment = 5
    }
}
