using Common.DA.Interface;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс классов моделей-представления для вкладок панели поиска.
    /// </summary>
    public interface ISearchTemplateVM
    {
        /// <summary>
        /// Вовзращает доменный объект, который служит шаблоном для поиска.
        /// </summary>
        IDomainObject SearchObject { get; set; }
    }
}