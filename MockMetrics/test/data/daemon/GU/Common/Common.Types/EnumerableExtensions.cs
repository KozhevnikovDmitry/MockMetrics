using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Types
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Возвращает первый элемент последовательности либо null если последовательность не содержит элементов
        /// Версия для классов
        /// </summary>
        /// <typeparam name="T">Тип элементов (класс)</typeparam>
        /// <param name="items">Последовательность</param>
        /// <param name="predicate">Условие проверки элементов последовательности</param>
        /// <returns>Первый элемент, либо null</returns>
        /// <exception cref="ArgumentNullException">items is null</exception>
        public static T FirstOrNull<T>(this IEnumerable<T> items, Func<T, bool> predicate = null) where T : class
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (var item in items)
                if (predicate == null || predicate(item))
                    return item;
            return null;
        }


        /// <summary>
        /// Возвращает первый элемент последовательности либо null если последовательность не содержит элементов
        /// Версия для простых типов
        /// </summary>
        /// <typeparam name="T">Тип элементов (простой тип)</typeparam>
        /// <param name="items">Последовательность</param>
        /// <param name="predicate">Условие проверки элементов последовательности</param>
        /// <returns>Первый элемент, либо (Т?)null</returns>
        public static T? FirstOrNullable<T>(this IEnumerable<T> items, Func<T, bool> predicate = null) where T : struct 
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (var item in items)
                if (predicate == null || predicate(item))
                    return item;
            return null;
        }
    }
}
