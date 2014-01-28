using Common.DA.Interface;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс для ViewModel'ов с функционалом отображения валидации полей доменных объектов
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public interface IDomainValidateableVM<T> : IValidateableVM
        where T : IDomainObject
    {
        T Entity { get; }
    }
}