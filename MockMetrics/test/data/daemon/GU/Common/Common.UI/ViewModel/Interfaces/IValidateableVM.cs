using System.ComponentModel;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    public interface IBaseValidateableVm : INotifyPropertyChanged
    {
        /// <summary>
        /// Генерирует событие обновления поля валидности отображаемых данных.
        /// </summary>
        void RaiseIsValidChanged();
    }

    /// <summary>
    /// Интерфейс классов Моделей-Представления с функционалом валидации отображаемых данных.
    /// </summary>
    public interface IValidateableVM : IBaseValidateableVm, IDataErrorInfo
    {            
        #region Binding Properties

        /// <summary>
        /// Возвращает флаг валидности отображаемых данных.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Возвращает или устанавливает флаг, разрешающий проводить валидацию.
        /// </summary>
        bool AllowValidate { get; set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Возвращает команду выставления режима готовности к проведению валидации.
        /// </summary>
        DelegateCommand ReadyToValidateCommand { get; }

        /// <summary>
        /// Возвращает команду выставления режима неготовности к проведению валидации.
        /// </summary>
        DelegateCommand NotReadyToValidateCommand { get; }

        /// <summary>
        /// Генерирует события для обновления валидируемых полей привязки.
        /// </summary>
        void RaiseValidatingPropertyChanged();
        
        #endregion        
    }
}
