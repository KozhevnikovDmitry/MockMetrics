using System.Collections.Generic;

using Common.Types.Exceptions;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс содержащий русские названия месяцев в родительном падеже
    /// </summary>
    public class MonthDataHelper
    {
        /// <summary>
        /// Словарь с именами месяцов.
        /// </summary>
        private static readonly Dictionary<int, string> MonthNames = new Dictionary<int, string>();

        /// <summary>
        /// Класс содержащий русские названия месяцев в родительном падеже
        /// </summary>
        static MonthDataHelper()
        {
            MonthNames[1] = "января";
            MonthNames[2] = "февраля";
            MonthNames[3] = "марта";
            MonthNames[4] = "апреля";
            MonthNames[5] = "мая";
            MonthNames[6] = "июня";
            MonthNames[7] = "июля";
            MonthNames[8] = "августа";
            MonthNames[9] = "сентября";
            MonthNames[10] = "октября";
            MonthNames[11] = "ноября";
            MonthNames[12] = "декабря";
        }

        /// <summary>
        /// Возвращает русское название месяца в родительном падеже по порядковому номеру
        /// </summary>
        /// <param name="number">Порядковый номер месяца</param>
        /// <exception cref="NoSuchMonthException">Не существует месяца с таким порядковым номером</exception>
        /// <returns>Русское название месяца в родительном падеже</returns>
        public static string GetMonth(int number)
        {
            if (MonthNames.ContainsKey(number))
            {
                return MonthNames[number];
            }

            throw new NoSuchMonthException(number);
        }
    }

    /// <summary>
    /// Класс исключение для ошибок "Не существует месяца с таким порядковым номером"
    /// </summary>
    public class NoSuchMonthException : BLLException
    {
        public NoSuchMonthException(int number)
            : base(string.Format("Не существует месяца с порядковым номером {0}", number))
        {
        }
    }
}
