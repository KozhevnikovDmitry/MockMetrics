using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Common.BL.DataMapping;
using Common.BL.Search;
using Common.BL.Search.SearchModification;
using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.SearchViewModel.WeakEventManager;
using Common.UI.WeakEvent;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Базовый класс ViewModel поиска сущностей по базе данных. 
    /// </summary>
    public abstract class AbstractSearchVM<T> : NotificationObject, ISearchVM<T>, IAvalonDockCaller where T : IPersistentObject
    {
        /// <summary>
        /// Стратегия поиска доменных объектов.
        /// </summary>
        private readonly ISearchStrategy<T> _strategy;

        /// <summary>
        /// Маппер доменных объектов.
        /// </summary>
        private readonly IDomainDataMapper<T> _dataMapper;

        /// <summary>
        /// Контейнер пресетов поиска.
        /// </summary>
        private readonly ISearchPresetContainer _searchPresetContainer;

        /// <summary>
        /// Объект захвата блокировки.
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// Базовый класс ViewModel поиска сущностей по базе данных. 
        /// </summary>
        /// <param name="strategy">Объект стратегия поиска</param>
        /// <param name="dataMapper">Маппер для искомых доменных объектов</param>
        /// <param name="searchPresetContainer">Контейнер пресетов поиска</param>
        protected AbstractSearchVM(ISearchStrategy<T> strategy, IDomainDataMapper<T> dataMapper, ISearchPresetContainer searchPresetContainer)
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            _strategy = strategy;
            _strategy.SearchMode = SearchMode.Templated;
            _dataMapper = dataMapper;
            this._searchPresetContainer = searchPresetContainer;
            _strategy.SearchResultReady += OnSearchResultReady;            
            CancelOperationCommand = new DelegateCommand(CancelOperation, () => !IsCancelationRequested);
            DefaultSearchCommand = new DelegateCommand(this.DefaultSearch, () => IsDefaultSearchEnabled);
            SearchCommand = new DelegateCommand<ISearchTemplateVM>(Search, CanSearch);
            SearchNextPageCommand = new DelegateCommand<ISearchTemplateVM>(SearchNextPage, CanSearchNextPage);
            SearchPreviousPageCommand = new DelegateCommand<ISearchTemplateVM>(SearchPreviousPage, CanSearchPreviousPage);
            ChooseItemCommand = new DelegateCommand<AbstractSearchResultVM<T>>(ChooseItem);
            IsCancelationRequested = false;
            IsDefaultSearchEnabled = true;
            SearchPage = 1;
            _weakChooseListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnChooseResultRequested);
            _weakSelectListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnSelectedResultChanged);
            SearchTemplateVMs = new ObservableCollection<ISearchTemplateVM>();
            SearchTemplateVMs.CollectionChanged += (s, e) => SetAvailableFlags();
            GetPreset();
        }

        /// <summary>
        /// Выставляет флаги доступности команд поиска
        /// </summary>
        protected void SetAvailableFlags()
        {
            IsTemplatedSearchAvailable = SearchTemplateVMs.Count > 0;
            IsCustomSearchAvailable = SearchPresetVM != null;
            SearchCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Формирует пресет поиска.
        /// </summary>
        private void GetPreset()
        {
            SearchPreset = this._searchPresetContainer.ResolveSearchPreset<T>();
            if (!IsSearchPresetEmpty())
            {
                this.SetCustomModeCommand.Execute();
            }
        }

        /// <summary>
        /// Возвращает доменныый объект - результат поиска.
        /// </summary>
        public T Result { get; protected set; }

        /// <summary>
        /// Устанавливает значение для поля привязки SearchResultInfo, отображающего данные о странице поиска.
        /// </summary>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="resultCount">Количество результатов</param>
        /// <param name="position">Начальная позиция страницы</param>
        protected void SetSearchResultInfo(int pageSize, int resultCount, int position)
        {
            if (resultCount != 0)
            {
                SearchResultInfo = string.Format("Показаны {0} - {1} результатов из {2}", position + 1, position + pageSize, resultCount);               
            }
            else
            {
                SearchResultInfo = "Поиск результатов не дал";
            }
        }

        /// <summary>
        /// Возвращает максимальный размер страницы поиска.
        /// </summary>
        /// <returns></returns>
        private int GetLoadingRowCount()
        {
            return 20;
        }

        /// <summary>
        /// Возвращает true, если пресет поиска пуст.
        /// </summary>
        /// <returns></returns>
        private bool IsSearchPresetEmpty()
        {
            return SearchPreset.SearchExpressionList.Count == 0 && SearchPreset.OrderFieldList.Count == 0;
        }

        /// <summary>
        /// Возвращает или устанавливает индекс выделенного элемента списка поиска.
        /// </summary>
        public int SelectedIndex { get; set; }

        #region Events

        /// <summary>
        /// Событие запроса на выбор одной сущности как результата поиска.
        /// </summary>
        public event ChooseItemRequestedDelegate ChooseResultRequested;

        /// <summary>
        /// Возбуждает событие ChooseResultRequested.
        /// </summary>
        /// <param name="result">Выбранный результат поиска</param>
        protected void RaiseChooseResultRequested(IDomainObject result)
        {
            if (ChooseResultRequested != null)
            {
                ChooseResultRequested(this, new ChooseItemRequestedEventArgs(result));
            }
        }

        /// <summary>
        /// Посылает запрос на открытие вкладки редактирования выбранного результата поиска
        /// </summary>
        /// <param name="result">Результат поиска</param>
        private void RaiseOpenEditableDockable(IDomainObject result)
        {
            var entity = _dataMapper.Retrieve(result.GetKeyValue());
            AvalonInteractor.RaiseOpenEditableDockable(entity.ToString(), typeof(T), entity);
        }

        #endregion

        #region Event Handling

        /// <summary>
        /// Слушатель события выбора результатирующего элемента на странице поиска.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakChooseListener;

        /// <summary>
        /// Слушатель события выделения элемента на страницу поиска.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakSelectListener;

        /// <summary>
        /// Обрабатывает событие выбора результатирующего элемента на странице поиска.
        /// </summary>
        /// <param name="sender">Выбранный элемент</param>
        /// <param name="e">Аргументы события</param>
        protected virtual void OnChooseResultRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            Result = (T)e.Result;
            RaiseChooseResultRequested(e.Result);
            RaiseOpenEditableDockable(e.Result);
        }

        /// <summary>
        /// Обрабатывает событие выделения элемента на страницу поиска.
        /// </summary>
        /// <param name="sender">Выделенный элемент</param>
        /// <param name="e">Аргументы события</param>
        protected virtual void OnSelectedResultChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            Result = (T)e.Result;
        }

        /// <summary>
        /// Обрабаотывает событие завершения поиска стратегией.
        /// </summary>
        /// <param name="e">Аргументы события с результатами поиска</param>
        private void OnSearchResultReady(SearchResultReadyEventArgs<T> e)
        {
            if (e.IsFailed)
            {
                OnSearchFailed(e.Exception);
            }
            else
            {
                _searchResultCount = e.ResultCount;
                try
                {
                    PresentResults(WrapResults(e.ResultPage), e.ResultCount, e.Position);
                }
                catch (GUException ex)
                {
                    OnSearchFailed(ex);
                    return;
                }
                catch (Exception ex)
                {
                    OnSearchFailed(new Exception("Непредвиденная ошибка", ex));
                    return;
                }
                IsLoadingProgressing = false;
                SearchPreviousPageCommand.RaiseCanExecuteChanged();
                SearchNextPageCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Обрабатывает ситуацию с ошибкой в процессе поиска.
        /// </summary>
        /// <param name="ex">исключение с ошибкой</param>
        private void OnSearchFailed(Exception ex)
        {
            NoticeUser.ShowError(ex);
            if (ResultItemsVMList != null)
            {
                ResultItemsVMList.Clear();
            }
            RaisePropertyChanged(() => ResultItemsVMList);
            SetSearchResultInfo(0, 0, 0);
            IsLoadingProgressing = false;
        }

        /// <summary>
        /// Представляет результаты поиска в соотвествующих свойствах привязки.
        /// </summary>
        /// <param name="resultPage">Страница поиска</param>
        /// <param name="resultCount">Количество резульататов</param>
        /// <param name="position">начальная позиция страницы в общем запросе</param>
        protected virtual void PresentResults(List<ISearchResultVM<T>> resultPage, int resultCount, int position)
        {
            SetSearchResultInfo(resultPage.Count, resultCount, position);
            lock (_locker)
            {
                ResultItemsVMList = new ObservableCollection<ISearchResultVM<T>>(resultPage);
                RaisePropertyChanged(() => ResultItemsVMList);
            }
        }

        /// <summary>
        /// Заворачивает результаты поиска в классы ViewModel'ы для отображения в списке результатов.
        /// </summary>
        /// <param name="resultPage">Страница поиска</param>
        /// <returns>Список ViewModel'ов для отображения результатов</returns>
        protected virtual List<ISearchResultVM<T>> WrapResults(List<T> resultPage)
        {
            var resultVMs = new List<ISearchResultVM<T>>();
            foreach (var res in resultPage)
            {
                ISearchResultVM<T> item = UIFacade.GetSearchResultVM(res);
                ChooseResultRequestedWeakEventManager.AddListener(item, _weakChooseListener);
                SelectedResultCahngedWeakEventManager.AddListener(item, _weakSelectListener);
                resultVMs.Add(item);
            }

            return resultVMs;
        }

        #endregion
        
        #region Binding Properties

        /// <summary>
        /// Отображаемое количество результатов поиска
        /// </summary>
        private int _searchResultCount;

        /// <summary>
        /// Коллекция VM'ов с шаблонами поиска
        /// </summary>
        public ObservableCollection<ISearchTemplateVM> SearchTemplateVMs { get; set; }

        /// <summary>
        /// Коллекция VM'ов с резульататами поиска
        /// </summary>
        public ObservableCollection<ISearchResultVM<T>> ResultItemsVMList { get; protected set; }

        /// <summary>
        /// Флаг указывающий на доступность дефолтного поиска
        /// </summary>
        public bool IsDefaultSearchEnabled { get; protected set; }

        /// <summary>
        /// Флаг указывающий на незавершённость процесса поиска.
        /// </summary>
        private bool _isLoadingProgressing;

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на незавершённость процесса поиска.
        /// </summary>
        public bool IsLoadingProgressing
        {
            get
            {
                return this._isLoadingProgressing;
            }
            set
            {
                if (this._isLoadingProgressing != value)
                {
                    this._isLoadingProgressing = value;
                    RaisePropertyChanged(() => IsLoadingProgressing);
                    this.SearchCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Номер текущей страницы результатов поиска.
        /// </summary>
        private int _searchPage;

        /// <summary>
        /// Возвращает или устанавливает номер текущей страницы результатов поиска.
        /// </summary>
        public int SearchPage
        {
            get 
            { 
                return this._searchPage; 
            }
            set 
            {
                if (this._searchPage != value)
                {
                    this._searchPage = value;
                    RaisePropertyChanged(() => SearchPage);
                }
            }
        }

        /// <summary>
        /// Строка с информацией о результатах поиска.
        /// </summary>
        private string _searchResultInfo;

        /// <summary>
        /// Возвращает или устанавливает информацию о результатах поиска.
        /// </summary>
        public string SearchResultInfo
        {
            get 
            { 
                return this._searchResultInfo; 
            }
            set 
            {
                if (this._searchResultInfo != value)
                {
                    this._searchResultInfo = value;
                    RaisePropertyChanged(() => SearchResultInfo);
                }
            }
        }

        /// <summary>
        /// Флаг отмены поиска.
        /// </summary>
        private bool _isCancelationRequested;

        /// <summary>
        /// Возвращает флаг отмены поиска.
        /// </summary>
        public bool IsCancelationRequested
        {
            get
            {
                return _isCancelationRequested;
            }
            protected set
            {
                if (_isCancelationRequested != value)
                {
                    _isCancelationRequested = value;
                    RaisePropertyChanged(() => IsCancelationRequested);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на открытость панели поиска.
        /// </summary>
        public bool IsSearchOpenned { get; set; }

        /// <summary>
        /// Флаг указывающий на доступность шаблонного поиска
        /// </summary>
        private bool _isTemplatedSearchAvailable;

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на доступность шаблонного поиска
        /// </summary>
        public bool IsTemplatedSearchAvailable
        {
            get
            {
                return _isTemplatedSearchAvailable;
            }
            set
            {
                if (_isTemplatedSearchAvailable != value)
                {
                    _isTemplatedSearchAvailable = value;
                    RaisePropertyChanged(() => IsTemplatedSearchAvailable);
                }
            }
        }

        /// <summary>
        /// Флаг указывающий на доступность расширенного поиска
        /// </summary>
        private bool _isCustomSearchAvailable;

        /// <summary>
        /// Возвращает или устанавливает флаг указывающий на доступность расширенного поиска
        /// </summary>
        public bool IsCustomSearchAvailable
        {
            get
            {
                return _isCustomSearchAvailable;
            }
            set
            {
                if (_isCustomSearchAvailable != value)
                {
                    _isCustomSearchAvailable = value;
                    RaisePropertyChanged(() => IsTemplatedSearchAvailable);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда отмены выполняемого поиска
        /// </summary>
        public DelegateCommand CancelOperationCommand { get; protected set; }

        /// <summary>
        /// Команда начала процедуры поиска
        /// </summary>
        public DelegateCommand<ISearchTemplateVM> SearchCommand { get; protected set; }

        /// <summary>
        /// Команда поиска следующей страницы
        /// </summary>
        public DelegateCommand<ISearchTemplateVM> SearchNextPageCommand { get; protected set; }

        /// <summary>
        /// Команда поиска предыдущей страницы
        /// </summary>
        public DelegateCommand<ISearchTemplateVM> SearchPreviousPageCommand { get; protected set; }

        /// <summary>
        /// Команда выбора одно элемента поиска
        /// </summary>
        public DelegateCommand<AbstractSearchResultVM<T>> ChooseItemCommand { get; protected set; }

        /// <summary>
        /// Команда дефолтного поиска
        /// </summary>
        public DelegateCommand DefaultSearchCommand { get; private set; }

        /// <summary>
        /// Загружает дефолтную страницу поиска.
        /// </summary>
        private void DefaultSearch()
        {
            if (ResultItemsVMList != null)
            {
                return;    
            }

            IsLoadingProgressing = true;
            try
            {
                if (!IsSearchPresetEmpty())
                {
                    _strategy.Search(new SearchData(this.SearchPreset, GetLoadingRowCount(), 0));
                }
                else
                {
                    _strategy.Search(new SearchData(GetLoadingRowCount(), 0));
                }
            }
            catch (AggregateException ex)
            {
                NoticeUser.ShowError(ex.InnerException);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex.InnerException);
            }
        }

        /// <summary>
        /// Осуществляет выбор переданного элемента в списке поиска
        /// </summary>
        /// <param name="item">Элемент в списке поиска</param>
        private void ChooseItem(AbstractSearchResultVM<T> item)
        {
            item.ChooseItemCommand.Execute();
        }

        /// <summary>
        /// Инициирует отмену текущего поиска
        /// </summary>
        private void CancelOperation()
        {
            if (_strategy.CancelSource != null)
            {
                _strategy.CancelSource.Cancel();
                IsCancelationRequested = true;
                CancelOperationCommand.RaiseCanExecuteChanged();
            }
        }

        #region Search

        /// <summary>
        /// Инициирует процедуру поиска доменных объектов
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        protected void Search(ISearchTemplateVM vm)
        {            
            IsLoadingProgressing = true;
            SearchPage = 1;
            try
            {
                var searchData = GetSearchData(vm);
                _strategy.Search(searchData);
            }
            catch (NotSupportedException ex)
            {
                NoticeUser.ShowError(new Exception("Шаблон или шаблоны для поиска не найдены", ex));
                IsLoadingProgressing = false;
            }
            catch (AggregateException ex)
            {
                NoticeUser.ShowError(ex.InnerException);
                IsLoadingProgressing = false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
                IsLoadingProgressing = false;
            }
        }

        /// <summary>
        /// Возвращает true, если возможно инициировать процедуру поиска
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        /// <returns>true, если возможно инициировать процедуру поиска</returns>
        protected bool CanSearch(ISearchTemplateVM vm)
        {
            return !this.IsLoadingProgressing && (IsCustomSearchAvailable || IsTemplatedSearchAvailable);
        }

        /// <summary>
        /// Возвращает объект с данными для поиска в стратегии
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        /// <returns>Объект с данными для поиска</returns>
        private ISearchData GetSearchData(ISearchTemplateVM vm)
        {
            if (_strategy.SearchMode == SearchMode.Custom)
            {
                return new SearchData(SearchPreset, GetLoadingRowCount(), 0);
            }
            else
            {
                return new SearchData(vm.SearchObject, GetLoadingRowCount(), 0, vm.SearchObject.GetType().IsSubclassOf(typeof(T)));
            }
        }

        #endregion

        /// <summary>
        /// Инициирует процедуру поиска следующей страницы доменных объектов
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        protected virtual void SearchNextPage(ISearchTemplateVM vm)
        {
            SearchPage++;
            GetAnotherPage();
        }

        /// <summary>
        /// Инициирует процедуру поиска предыдущей страницы доменных объектов
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        protected virtual void SearchPreviousPage(ISearchTemplateVM vm)
        {

            SearchPage--;
            GetAnotherPage();
        }

        protected void GetAnotherPage()
        {
            try
            {
                IsLoadingProgressing = true;
                _strategy.SearchPage((SearchPage - 1) * GetLoadingRowCount());
            }
            catch (AggregateException ex)
            {
                NoticeUser.ShowError(ex.InnerException);
                IsLoadingProgressing = false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
                IsLoadingProgressing = false;
            }
        }

        /// <summary>
        /// Возвращает true, если возможно инициировать процедуру поиска следующей страницы доменных объектов
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        /// <returns>true, если возможно инициировать процедуру поиска следующей страницы</returns>
        protected virtual bool CanSearchNextPage(ISearchTemplateVM vm)
        {
            if (!IsLoadingProgressing && _strategy.CanGetAnotherPage())
            {
                int amount = GetLoadingRowCount();
                if ((SearchPage) * amount < _searchResultCount)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Возвращает true, если возможно инициировать процедуру поиска предыдущей страницы доменных объектов
        /// </summary>
        /// <param name="vm">VM с шаблоном поиска</param>
        /// <returns>true, если возможно инициировать процедуру поиска предыдущей страницы</returns>
        protected virtual bool CanSearchPreviousPage(ISearchTemplateVM vm)
        {
            if (!IsLoadingProgressing && _strategy.CanGetAnotherPage())
            {
                if (SearchPage > 1)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion  

        #region Custom search

        /// <summary>
        /// Пресет для настраиваемого(расширенного) поиска
        /// </summary>
        protected SearchPreset SearchPreset;
        
        /// <summary>
        /// VM настраиваемого поиска
        /// </summary>
        private ISearchPresetVM _searchPresetVM;

        /// <summary>
        /// Вовзращает или устанавливает VM настраиваемого поиска
        /// </summary>
        public ISearchPresetVM SearchPresetVM
        {
            get
            {
                return _searchPresetVM;
            }
            set
            {
                if (_searchPresetVM != value)
                {
                    _searchPresetVM = value;
                    SetAvailableFlags();
                    RaisePropertyChanged(() => SearchPresetVM);
                }
            }
        }

        /// <summary>
        /// Команда установки шаблонного поиска в качестве рабочего режима
        /// </summary>
        public DelegateCommand _setTemplatedModeCommand;

        /// <summary>
        /// Возвращает команду установки шаблонного поиска в качестве рабочего режима
        /// </summary>
        public DelegateCommand SetTemplatedModeCommand
        {
            get
            {
                if(_setTemplatedModeCommand == null)
                    _setTemplatedModeCommand = new DelegateCommand(() =>
                    {
                        _strategy.SearchMode = SearchMode.Templated;
                        SelectedIndex = 0;
                        this.RaisePropertyChanged(() => SelectedIndex);
                    });
                return _setTemplatedModeCommand;
            }
        }

        /// <summary>
        /// Команда установки настраиваемого поиска в качестве рабочего режима
        /// </summary>
        public DelegateCommand _setCustomModeCommand;

        /// <summary>
        /// Возвращает команду установки настраиваемого поиска в качестве рабочего режима
        /// </summary>
        public DelegateCommand SetCustomModeCommand
        {
            get
            {
                if (_setCustomModeCommand == null)
                    _setCustomModeCommand = new DelegateCommand(() =>
                    {
                        _strategy.SearchMode = SearchMode.Custom;
                        SelectedIndex = 1;
                        this.RaisePropertyChanged(() => SelectedIndex);
                    });
                return _setCustomModeCommand;
            }
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
