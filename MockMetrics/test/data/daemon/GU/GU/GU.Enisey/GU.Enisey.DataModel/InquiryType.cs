using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GU.Enisey.DataModel
{
    /// <summary>
    /// тип межвед запроса
    /// </summary>
    public enum InquiryType
    {
        /// <summary>
        /// Исходящий запрос (который создает Енисей-ГУ автоматически по статусу) по заявке из нашей системы
        /// </summary>
        Outgoing = 1,

        /// <summary>
        /// Входящий запрос от другого ведомства - у нас нет заявки, к которой был бы привязан этот запрос
        /// </summary>
        Incoming = 2
    }
}
