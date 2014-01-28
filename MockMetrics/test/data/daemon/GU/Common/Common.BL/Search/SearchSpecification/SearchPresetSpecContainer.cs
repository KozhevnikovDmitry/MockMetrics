using System;
using System.Collections.Generic;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс контейнер пресетов 
    /// </summary>
    [Serializable]
    public abstract class SearchPresetSpecContainer
    {
        /// <summary>
        /// Список пресетов поиска
        /// </summary>
        public List<SearchPresetSpec> PresetSpecList { get; protected set; }

        /// <summary>
        /// Дата последнего изменения контейнера
        /// </summary>
        public DateTime LastUpdate { get; protected set; }
    }
}
