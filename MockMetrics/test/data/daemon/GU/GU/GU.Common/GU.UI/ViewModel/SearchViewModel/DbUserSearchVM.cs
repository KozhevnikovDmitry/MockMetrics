using System;
using System.Collections.Generic;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchSpecification;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;

using GU.DataModel;

namespace GU.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс ViewModel поиска сущностей Пользователь
    /// </summary>
    public class DbUserSearchVM : AbstractSearchVM<DbUser>
    {
        /// <summary>
        /// Класс ViewModel поиска сущностей Пользователь
        /// </summary>
        /// <param name="strategy">Стратегия поиска пользователей</param>
        /// <param name="dataMapper">Маппер пользователей</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        public DbUserSearchVM(ISearchStrategy<DbUser> strategy, IDomainDataMapper<DbUser> dataMapper, ISearchPresetContainer searchPresetContainer)
            : base(strategy, dataMapper, searchPresetContainer)
        {
            SearchPresetVM = UIFacade.CreateSearchPresetVM(this.SearchPreset, new List<Type> { typeof(DbUser) });
        }
    }
}
