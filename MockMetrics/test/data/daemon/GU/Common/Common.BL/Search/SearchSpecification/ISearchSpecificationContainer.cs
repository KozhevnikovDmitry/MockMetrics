using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Search.SearchSpecification
{
    public interface ISearchPresetContainer
    {
        /// <summary>
        /// Возвращает пресет поиска доменных объектов типа T
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <exception cref="VMException">Ошибка при создании экземпляра пресета поиска для типа</exception>
        /// <returns>Пресет поиска</returns>
        SearchPreset ResolveSearchPreset<T>() where T : IPersistentObject;
    }
}