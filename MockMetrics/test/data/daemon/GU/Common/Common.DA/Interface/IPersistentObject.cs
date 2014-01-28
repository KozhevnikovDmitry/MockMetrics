using System;

namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс для классов персистентных доменных объектов.
    /// </summary>
    /// <remarks>
    /// В условиях наличия глобальной и локальных баз с репликацией, 
    /// этому интерфейсу должны наследовать не только персистентные объекты,
    /// но и объекты из пополняемых справочников.
    /// </remarks>
    public interface IPersistentObject : IDomainObject
    {   
        /// <summary>
        /// Статус постоянства доменного объекта.
        /// </summary>
        /// <remarks>
        /// Статус используется для управления отображением объекта в БД при сохранении.
        /// </remarks>
        PersistentState PersistentState { get; set; }

        /// <summary>
        /// Объект-реплика, содержит информацию о репликации сущности.
        /// </summary>
        ICommonData CommonData { get; set; }

        /// <summary>
        /// Помечает объект на удаление.
        /// </summary>
        void MarkDeleted();

        /// <summary>
        /// Помечает поля объекта как сохранённые.
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Флаг, указывающий на наличие несохранённых изменений в объекте 
        /// </summary>
        bool IsDirty { get; }
    }
}