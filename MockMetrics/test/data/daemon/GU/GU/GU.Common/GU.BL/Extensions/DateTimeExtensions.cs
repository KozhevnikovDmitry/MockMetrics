using System;

using Common.Types.Exceptions;

namespace GU.BL.Extensions
{
    public static class 
        DateTimeExtensions
    {
        #region Working days

        public static DateTime AddWorkingDays(this DateTime date, int workingDays)
        {
            int direction = workingDays < 0 ? -1 : 1;
            DateTime newDate = date;
            while (workingDays != 0)
            {
                newDate = newDate.AddDays(direction);
                if (newDate.IsWorkingDay())
                    workingDays -= direction;
            }
            return newDate;
        }

        public static bool IsWorkingDay(this DateTime date)
        {
            //TODO: учесть праздники и переносы рабочих дней на выходные
            return
                date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Exteysion для вычисления числа рабочих дней до некоторой даты 
        /// </summary>
        /// <param name="date">Данная дата</param>
        /// <param name="toDate">Дата окончания срока подсчёта</param>
        /// <exception cref="BLLException">Дата начала срока подчёта рабочих дней не может быть позднее даты окончания</exception>
        /// <returns>Число рабочих дней</returns>
        public static int GetWorkingDaysTo(this DateTime date, DateTime toDate)
        {
            if (date > toDate)
            {
                throw new BLLException("Дата начала срока подчёта рабочих дней не может быть позднее даты окончания");
            }

            var workDaysCount = 0;
            var newDate = date;
            while (newDate.Date != toDate.Date)
            {
                newDate = newDate.AddDays(1);
                workDaysCount++;
            }

            return workDaysCount;
        }

        #endregion

        #region Rounding
        //http://stackoverflow.com/questions/1393696/c-sharp-rounding-datetime-objects

        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks / span.Ticks);
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Ceil(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }
        #endregion

    }
}
