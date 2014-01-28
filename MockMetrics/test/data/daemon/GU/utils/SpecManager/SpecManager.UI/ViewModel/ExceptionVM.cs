using System;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace SpecManager.UI.ViewModel
{
    /// <summary>
    /// Класс ViewModel для окна отображения информации об ошибке.
    /// </summary>
    public class ExceptionVm : NotificationObject
    {
        /// <summary>
        /// Отображаемое исключение
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Класс ViewModel для окна отображения информации об ошибке.
        /// </summary>
        /// <param name="exception">Исключение, инкапсулирующее информацию об ошибке</param>
        public ExceptionVm(Exception exception)
        {
            this.CopyToClipBoardCommand = new DelegateCommand(this.CopyToClipBoard);
            this._exception = exception;
        }

        #region Binding Properties

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message 
        {
            get
            {
                string inner = this._exception.InnerException == null ? string.Empty
                                : ": " + this._exception.InnerException.Message;
                return this._exception.Message + inner;
            }
        }

        /// <summary>
        /// Детализация по ошибке
        /// </summary>
        public string Details
        {
            get
            {
                return this._exception.ToString();
            }
        }

        #endregion

        #region Binding Command

        /// <summary>
        /// Команда копирования данных об ошибке в буфер обмена.
        /// </summary>
        public DelegateCommand CopyToClipBoardCommand { get; set; }

        /// <summary>
        /// Копирует данных об ошибке в буфер обмена.
        /// </summary>
        private void CopyToClipBoard()
        {
            System.Windows.Clipboard.SetText(this.Details);
        }

        #endregion
    }
}
