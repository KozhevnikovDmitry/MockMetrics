﻿using System;
using System.Windows;

using Common.Types;
using Common.UI.View;
using Common.UI.ViewModel;

namespace Common.UI
{
    /// <summary>
    /// Класс, предназанченный для отображения сообщений пользователю.
    /// </summary>
    public static class NoticeUser
    {
        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="ex">Исключение, инкапсулирующее информацию об ошибке</param>
        public static void ShowError(Exception ex)
        {
            LoggerContainer.LogError(ex);
            ExceptionWindow ew = new ExceptionWindow() { DataContext = new ExceptionVM(ex) };
            if(!ew.Equals(Application.Current.MainWindow))
                ew.Owner = Application.Current.MainWindow;
            ew.ShowDialog();
        }

        /// <summary>
        /// Отображаем информационное сообщение.
        /// </summary>
        /// <param name="message">Информационное сообщение</param>
        public static void ShowInformation(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Отображает предупреждение.
        /// </summary>
        /// <param name="message">Текст предупреждения</param>
        public static void ShowWarning(string message)
        {
            LoggerContainer.LogMessage(message, AppLogType.Warning);
            MessageBox.Show(message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Отображает окно с вопросом ДА\НЕТ. Возвращает ответ пользователя.
        /// </summary>
        /// <param name="message">Текст вопроса</param>
        /// <returns>Ответ пользователя</returns>
        public static MessageBoxResult ShowQuestionYesNo(string message)
        {
            return ShowQuestionYesNo(message, "Вопрос");
        }

        /// <summary>
        /// Отображает окно с вопросом ДА\НЕТ. Возвращает ответ пользователя.
        /// </summary>
        /// <param name="message">Текст вопроса</param>
        /// <param name="caption">Заголовок вопроса</param>
        /// <returns>Ответ пользователя</returns>
        public static MessageBoxResult ShowQuestionYesNo(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        /// <summary>
        /// Отображает окно с вопросом ДА\НЕТ\ОТМЕНА. Возвращает ответ пользователя.
        /// </summary>
        /// <param name="message">Текст вопроса</param>
        /// <returns>Ответ пользователя</returns>
        public static MessageBoxResult ShowQuestionYesNoCancel(string message)
        {
            return ShowQuestionYesNoCancel(message, "Вопрос");
        }

        /// <summary>
        /// Отображает окно с вопросом ДА\НЕТ\ОТМЕНА. Возвращает ответ пользователя.
        /// </summary>
        /// <param name="message">Текст вопроса</param>
        /// <param name="caption">Заголовок вопроса</param>
        /// <returns>Ответ пользователя</returns>
        public static MessageBoxResult ShowQuestionYesNoCancel(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }
    }
}
