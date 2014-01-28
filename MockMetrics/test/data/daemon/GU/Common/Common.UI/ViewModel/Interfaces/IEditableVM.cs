using System;
using Common.DA.Interface;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс для классов ViewModel для рабочих областей редактирования доменного объекта.
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public interface IEditableVM<out T> : IEditableVM where T : IDomainObject
    {
        /// <summary>
        /// Редактируемый доменный объект.
        /// </summary>
        T Entity { get; }
    }

    /// <summary>
    /// Интерфейс для классов ViewModel для рабочих областей редактирования персистентных данных.
    /// </summary>
    public interface IEditableVM
    {
        /// <summary>
        /// Флаг, указывающий на наличие несохранённых изменений.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Событие, оповещающее об изменении флага IsDirty.
        /// </summary>
        event Action IsDirtyChanged;

        /// <summary>
        /// Событие, оповещающее об изменении отображаемого имени.
        /// </summary>
        event Action<string> DisplayNameChanged;

        /// <summary>
        /// Команда сохранения изменений в редактируемом объекте.
        /// </summary>
        DelegateCommand SaveCommand { get; }

        /// <summary>
        /// Обрабатывает запрос на закрытие рабочей области редактирования. Возвращает флаг, указывающий на возможность закрытия области.
        /// </summary>
        /// <param name="displayName">Отображаемое имя сущности</param>
        /// <returns>Флаг, указывающий на возможность закрытия области</returns>
        bool OnClosing(string displayName);

        /// <summary>
        /// Возвращает значение первичного ключа сущности.
        /// </summary>
        /// <returns>Значение первичного ключа сущности</returns>
        string GetEntityKeyValue();

        /// <summary>
        /// Возвращает тип редактируемого объекта.
        /// </summary>
        /// <returns>Тип редактируемого объекта</returns>
        Type GetEntityType();

        void RaiseIsDirtyChanged();
    }
}
