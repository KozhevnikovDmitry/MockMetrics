using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ListViewModel.WeakEventManager;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.ListViewModel
{
    /// <summary>
    /// Базовый класс для классов ViewModel отображения редактируемого списка доменных объектов
    /// </summary>
    /// <typeparam name="T">Доменный тип</typeparam>
    public abstract class AbstractListVM<T> : ValidateableVM, IListVM<T> where T : IDomainObject
    {
        /// <summary>
        /// Базовый класс для классов ViewModel отображения редактируемого списка доменных объектов
        /// </summary>
        /// <param name="inventory">Набор отображаемых объектов</param>
        protected AbstractListVM(INotifyCollectionChanged order)
        {     
            _weakOpenListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnOpenItemRequested);
            _weakRemoveListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnRemoveItemChanged);
            _weakSelectListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnSelectItemRequested);
            _weakCopyListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnCopyItemChanged);
            order.CollectionChanged += this.OnListItemsChanged;
            CreateListItems(order);
            SetListOptions();
            CreateListVmView();
        }

        protected AbstractListVM(object justforMz)
        {
            _weakOpenListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnOpenItemRequested);
            _weakRemoveListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnRemoveItemChanged);
            _weakSelectListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnSelectItemRequested);
            _weakCopyListener = new WeakEventListener<ChooseItemRequestedEventArgs>(OnCopyItemChanged);
        }

        /// <summary>
        /// Сбрасывает настройки на дефолтные.
        /// </summary>
        private void SetDefaultOptions()
        {           
            CanAddItems = true;
            CanRemoveItems = true;
            CanSort = false;
            CanGroup = false;
            SortProperties = new List<string>();
            HorisontalScrollVisibility = ScrollBarVisibility.Auto;
            IsMouseWheelIgnored = false;
            HighlightColor = Colors.LightGray;
            HighlightTextColor = Colors.Black;
            InactiveColor = SystemColors.ControlColor;
            AddItemCommand = new DelegateCommand(AddItem, () => CanAddItems);
            RemoveItemCommand = new DelegateCommand(RemoveItem, () => CanRemoveItems);
            ListItemTemplate = (DataTemplate)Application.Current.Resources[typeof(T).Name + "ItemTemplate"];
            if (ListItemTemplate == null) 
                throw new VMException(string.Format("Не найден шаблон отображения элементов списка по имени {0}", typeof(T).Name + "ItemTemplate"));
        }

        /// <summary>
        /// Устанавливает кастомные настройки списка.
        /// </summary>
        protected virtual void SetListOptions()
        {
            SetDefaultOptions();
        }

        /// <summary>
        /// Выделяет элемент списка отображающий поданный объект.
        /// </summary>
        /// <param name="item">Объект, элемент с которым надо выделить.</param>
        protected void SelectItem(T item)
        {
            SelectedItem = ListItemVMs.SingleOrDefault(t => t.Item.Equals(item));
        }

        protected virtual IListItemVM<T> CreateItemVm(T item)
        {
            return UIFacade.GetListItemVM<T>(item);
        }

        /// <summary>
        /// Создаёт список ViewModel'ов для отображения элементов списка.
        /// </summary>
        /// <param name="items"></param>
        protected void CreateListItems(INotifyCollectionChanged items)
        {
            ListItemVMs = new ObservableCollection<IListItemVM<T>>();
            ListItemVMs.CollectionChanged += OnListItemVMsChanged;
            if (items != null)
            {                
                foreach (var item in items as IEnumerable<T>)
                {
                    ListItemVMs.Add(CreateItemVm(item));
                }
            }
            RaisePropertyChanged(() => ListItemVMs);
        }

        /// <summary>
        /// Создаёт список View для отображения элементов списка.
        /// </summary>
        protected void CreateListVmView()
        {
            ListVmView = new ListCollectionView(ListItemVMs);
            if (CanGroup && !string.IsNullOrEmpty(GroupPropertyName))
            {
                ListVmView.GroupDescriptions.Clear();
                ListVmView.GroupDescriptions.Add(new PropertyGroupDescription(GroupPropertyName));
            }
            if (CanSort && SortProperties != null)
            {
                ListVmView.SortDescriptions.Clear();
                SortProperties.ForEach(sortProp => ListVmView.SortDescriptions.Add(new SortDescription(sortProp, SortDirection)));
            }
        }

        #region Event Handling

        /// <summary>
        /// Слушатель запросов на открытие элемента списка.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakOpenListener;

        /// <summary>
        /// Слушатель запросов на удаление элемента из списка.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakRemoveListener;

        /// <summary>
        /// Слушатель запросов на выделение элемента списка.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakSelectListener;

        /// <summary>
        /// Слушатель запросов на добавление копии элемента списка.
        /// </summary>
        private readonly WeakEventListener<ChooseItemRequestedEventArgs> _weakCopyListener;


        /// <summary>
        /// Обрабатывает запрос на открытие элемента списка.
        /// </summary>
        /// <param name="sender">Элемент списка</param>
        /// <param name="e">Аругменты события</param>
        protected virtual void OnOpenItemRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обрабатывает запрос на удаление элемента списка.
        /// </summary>
        /// <param name="sender">Элемент списка</param>
        /// <param name="e">Аругменты события</param>
        protected virtual void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обрабатывает запрос на выделение элемента списка.
        /// </summary>
        /// <param name="sender">Элемент списка</param>
        /// <param name="e">Аругменты события</param>
        protected virtual void OnSelectItemRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обрабатывает запрос на добавление копии элемента списка.
        /// </summary>
        /// <param name="sender">Элемент списка</param>
        /// <param name="e">Аругменты события</param>
        protected virtual void OnCopyItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обрабатывает событие изменения состава коллекции ViewModel'ов для отображения элементов списка.
        /// </summary>
        /// <param name="sender">Коллекция ViewModel'ов для отображения элементов списка</param>
        /// <param name="e">Аругменты события</param>
        protected virtual void OnListItemVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<IListItemVM>())
                {
                    OpenItemRequestedWeakEventManager.AddListener(item, _weakOpenListener);
                    RemoveItemRequestedWeakEventManager.AddListener(item, _weakRemoveListener);
                    SelectItemRequestedWeakEventManager.AddListener(item, _weakSelectListener);
                    CopyItemRequestedWeakEventManager.AddListener(item, _weakCopyListener);
                }
            }
            if (ListVmView != null)
            {
                var memento = GetHashListMemento();
                RaisePropertyChanged(() => ListVmView);
                ApplyHashListMemento(memento);
            }
        }

        /// <summary>
        /// Обрабатывает событие изменения состава коллекции отображаемых доменных объектов.
        /// </summary>
        /// <param name="sender">Коллекция доменных объектов</param>
        /// <param name="e">Аругменты события</param>
        protected void OnListItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<T>())
                    {
                        ListItemVMs.Add(CreateItemVm(item));
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<T>())
                    {
                        if (ListItemVMs.SingleOrDefault(t => t.Item.Equals(item)) != null)
                        {
                            ListItemVMs.Remove(ListItemVMs.Single(t => t.Item.Equals(item)));
                        }
                    }
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
            }
        }

        #endregion

        #region Templating

        /// <summary>
        /// Кэш шаблонов отображения элементов в списке.
        /// </summary>
        private static readonly Dictionary<string, DataTemplate> _templateCache = new Dictionary<string, DataTemplate>();

        /// <summary>
        /// Вовзвращает шаблон отображения элемента в списке по ключу.
        /// </summary>
        /// <param name="key">Ключ шаблона</param>
        /// <returns>Шаблон отображения элемента в списке</returns>
        private DataTemplate GetListTemplate(string key)
        {          
            if (_templateCache.ContainsKey(key))
            {
                return _templateCache[key];
            }
            else
            {
                var result = (DataTemplate)Application.Current.Resources[key];
                _templateCache[key] = result;
                return result;
            }
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// Наименование списка элементов.
        /// </summary>
        private string _title;

        /// <summary>
        /// Возвращает или устанавливает наименование списка элементов.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        /// <summary>
        /// Флаг игнорирования скрола мыши.
        /// </summary>
        private bool _isMouseWheelIgnored;

        /// <summary>
        /// Возвращает или устанавливает флаг игнорирования скрола мыши.
        /// </summary>
        public bool IsMouseWheelIgnored
        {
            get
            {
                return _isMouseWheelIgnored;
            }
            set
            {
                if (_isMouseWheelIgnored != value)
                {
                    _isMouseWheelIgnored = value;
                    RaisePropertyChanged(() => IsMouseWheelIgnored);
                }
            }
        }

        /// <summary>
        /// Цвет выделения элемента.
        /// </summary>
        private Color _highlightColor;

        /// <summary>
        /// Возвращает или устанавливает цвет выделения элемента.
        /// </summary>
        public Color HighlightColor
        {
            get
            {
                return _highlightColor;
            }
            set
            {
                if (_highlightColor != value)
                {
                    _highlightColor = value;
                    RaisePropertyChanged(() => HighlightColor);
                }
            }
        }

        /// <summary>
        /// Цвет неактивного выделения элемента
        /// </summary>
        private Color _inactiveColor;

        /// <summary>
        /// Возвращает или устанавливает цвет неактивного выделения элемента
        /// </summary>
        public Color InactiveColor
        {
            get
            {
                return _inactiveColor;
            }
            set
            {
                if (_inactiveColor != value)
                {
                    _inactiveColor = value;
                    RaisePropertyChanged(() => InactiveColor);
                }
            }
        }

        /// <summary>
        /// Цвет Foreground выделенного элемента.
        /// </summary>
        private Color _highlightTextColor;

        /// <summary>
        /// Возвращает или устанавливает цвет Foreground выделенного элемента.
        /// </summary>
        public Color HighlightTextColor
        {
            get
            {
                return _highlightTextColor;
            }
            set
            {
                if (_highlightTextColor != value)
                {
                    _highlightTextColor = value;
                    RaisePropertyChanged(() => HighlightTextColor);
                }
            }
        }

        /// <summary>
        /// Флаг видимости панелей скрола на списке.
        /// </summary>
        private ScrollBarVisibility _horisontalScrollvisibility;

        /// <summary>
        /// Возвращает или устанавливает флаг видимости панелей скрола на списке.
        /// </summary>
        public ScrollBarVisibility HorisontalScrollVisibility
        {
            get
            {
                return _horisontalScrollvisibility;
            }
            set
            {
                if (_horisontalScrollvisibility != value)
                {
                    _horisontalScrollvisibility = value;
                    RaisePropertyChanged(() => HorisontalScrollVisibility);
                }
            }
        }

        /// <summary>
        /// Коллекция ViewModel'ов отображения элементов в списке.
        /// </summary>
        private ObservableCollection<IListItemVM<T>> _listItemVMs;

        /// <summary>
        /// Возвращает или устанавливает коллекция ViewModel'ов отображения элементов в списке.
        /// </summary>
        public ObservableCollection<IListItemVM<T>> ListItemVMs
        {
            get
            {
                return _listItemVMs;
            }
            set
            {
                if (_listItemVMs != value)
                {
                    _listItemVMs = value;
                    RaisePropertyChanged(() => ListItemVMs);
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        #region ListView

        /// <summary>
        /// Имя свойства группировки объектов.
        /// </summary>
        private string _groupPropertyName;

        /// <summary>
        /// Возвращает или устанавливает имя свойства группировки объектов.
        /// </summary>
        public string GroupPropertyName
        {
            get
            {
                return _groupPropertyName;
            }
            set
            {
                if (_groupPropertyName != value)
                {
                    _groupPropertyName = value;
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        /// <summary>
        /// Список имён свойств сортировки объектов.
        /// </summary>
        private List<string> _sortProperties;

        /// <summary>
        /// Возвращает или устанавливает список имён свойств сортировки объектов.
        /// </summary>
        public List<string> SortProperties
        {
            get
            {
                return _sortProperties;
            }
            set
            {
                if (_sortProperties != value)
                {
                    _sortProperties = value;
                    RaisePropertyChanged(() => SortProperties);
                }
            }
        }

        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        private ListSortDirection _sortDirection;

        /// <summary>
        /// Возвращает или устанавливает порядок сортировки.
        /// </summary>
        public ListSortDirection SortDirection
        {
            get
            {
                return _sortDirection;
            }
            set
            {
                if (_sortDirection != value)
                {
                    _sortDirection = value;
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        /// <summary>
        /// Флаг возможности группировки элементов.
        /// </summary>
        private bool _canGroup;

        /// <summary>
        /// Возвращает или устанавливает флаг возможности группировки элементов.
        /// </summary>
        public bool CanGroup
        {
            get
            {
                return _canGroup;
            }
            set
            {
                if (_canGroup != value)
                {
                    _canGroup = value;
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        /// <summary>
        /// Флаг возможности сортировки элементов.
        /// </summary>
        private bool _canSort;

        /// <summary>
        /// Возвращает или устанавливает флаг возможности сортировки элементов.
        /// </summary>
        public bool CanSort
        {
            get
            {
                return _canSort;
            }
            set
            {
                if (_canSort != value)
                {
                    _canSort = value;
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        /// <summary>
        /// Объект привяки для отображения списка с группировками и сортировками.
        /// </summary>
        private ICollectionView _listVmView;

        /// <summary>
        /// Возвращает или устанавливает объект привяки для отображения списка.
        /// </summary>
        public ICollectionView ListVmView
        {
            get
            {
                return _listVmView;
            }
            set
            {
                if (_listVmView != value)
                {
                    _listVmView = value;
                    RaisePropertyChanged(() => ListVmView);
                }
            }
        }

        #region Memento

        #endregion

        private IListItemVM<T> _selectedItem;

        /// <summary>
        /// Возвращает или устанавливает 
        /// </summary>
        public virtual IListItemVM<T> SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged(() => SelectedItem);
                }
            }
        }

        /// <summary>
        /// Шаблон отображения элеметна в списке.
        /// </summary>
        private DataTemplate _listItemTemplate;

        /// <summary>
        /// Вовзвращает шаблон отображения элеметна в списке.
        /// </summary>
        public DataTemplate ListItemTemplate
        {
            get
            {
                return _listItemTemplate;
            }
            set
            {
                if (_listItemTemplate != value)
                {
                    _listItemTemplate = value;
                    RaisePropertyChanged(() => ListItemTemplate);
                }
            }
        }

        private Style _groupStyle;

        /// <summary>
        /// Возвращает или устанавливает 
        /// </summary>
        public Style GroupStyle
        {
            get
            {
                return _groupStyle;
            }
            set
            {
                if (_groupStyle != value)
                {
                    _groupStyle = value;
                    RaisePropertyChanged(() => GroupStyle);
                }
            }
        }

        private bool _canAddItems;

        /// <summary>
        /// Возвращает или устанавливает 
        /// </summary>
        public bool CanAddItems
        {
            get
            {
                return _canAddItems;
            }
            set
            {
                if (_canAddItems != value)
                {
                    _canAddItems = value;
                    RaisePropertyChanged(() => CanAddItems);
                }
            }
        }

        private bool _canRemoveItems;

        /// <summary>
        /// Возвращает или устанавливает 
        /// </summary>
        public bool CanRemoveItems
        {
            get
            {
                return _canRemoveItems;
            }
            set
            {
                if (_canRemoveItems != value)
                {
                    _canRemoveItems = value;
                    RaisePropertyChanged(() => CanRemoveItems);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Возвращает команду добавления нового элемента списка.
        /// </summary>
        public DelegateCommand AddItemCommand { get; protected set; }

        /// <summary>
        /// Возвращает команду удаления элемента из списка.
        /// </summary>
        public DelegateCommand RemoveItemCommand { get; protected set; }

        /// <summary>
        /// Добавляет новый элемент в список.
        /// </summary>
        protected virtual void AddItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет активный элемент из списка.
        /// </summary>
        protected virtual void RemoveItem()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Memento

        /// <summary>
        /// Класс хранитель состояния списка.
        /// </summary>
        public class ListMemento
        {
            public ListMemento()
            {
                this.ViewStates = new Dictionary<int, ListItemMemento>();
            }

            public Dictionary<int, ListItemMemento> ViewStates { get; set; }
        }

        /// <summary>
        /// Класс хранитель состояния элемента списка.
        /// </summary>
        public class ListItemMemento
        {
            public ListItemMemento()
            {
                this.ItemStates = new Dictionary<Action<object>, object>();
            }
            public Dictionary<Action<object>, object> ItemStates { get; set; }
        }

        /// <summary>
        /// Возвращает хранителя состояния списка.
        /// </summary>
        /// <returns>Хранитель состояния списка</returns>
        protected ListMemento GetHashListMemento()
        {
            try
            {
                ListMemento memento = new ListMemento();
                if (this.ListItemVMs != null)
                {
                    foreach (var t in this.ListItemVMs) 
                    {
                        memento.ViewStates[t.GetHashCode()] = GetListItemMemento(t);
                    }
                }
                return memento;
            }
            catch (Exception ex)
            {
                throw new VMException("Ошибка сохранения состояния ListView", ex);
            }
        }

        /// <summary>
        /// Возвращает хранителя состояния элемента списка.
        /// </summary>
        /// <param name="item">ViewModel элемента списка</param>
        /// <returns>Хранитель состояния элемента списка</returns>
        protected virtual ListItemMemento GetListItemMemento(IListItemVM<T> item)
        {
            return new ListItemMemento();        
        }

        /// <summary>
        /// Восстанавливает состояние списка по объекту хранителю.
        /// </summary>
        /// <param name="memento">Хранитель состояния списка</param>
        protected void ApplyHashListMemento(ListMemento memento)
        {
            try
            {
                if (this.ListItemVMs != null)
                {
                    foreach (var t in this.ListItemVMs) 
                    {
                        if (memento.ViewStates.ContainsKey(t.GetHashCode()))
                        {
                            ApplyListItemMemento(t, memento.ViewStates[t.GetHashCode()]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new VMException("Ошибка применения состояния ListView", ex);
            }
        }

        /// <summary>
        /// Восстанавливает состояние элемента списка по объекту хранителю.
        /// </summary>
        /// <param name="item">ViewModel элемента списка</param>
        /// <param name="listItemMemento">Хранитель состояния элемента списка</param>
        protected virtual void ApplyListItemMemento(IListItemVM<T> item, ListItemMemento listItemMemento)
        {
            foreach (var t in listItemMemento.ItemStates) 
            {
                t.Key(listItemMemento.ItemStates[t.Key]);
            }
        }

        #endregion

        #endregion

        #region Validateable

        public virtual void RaiseItemsValidatingPropertyChanged()
        {
            foreach (var listItemVM in ListItemVMs)
            {
                listItemVM.RaiseValidatingPropertyChanged();
            }
        }

        protected override void ReadyToValidate()
        {
            foreach (var listItemVM in ListItemVMs)
            {
                listItemVM.ReadyToValidateCommand.Execute();
            }
            base.ReadyToValidate();
        }


        #endregion
    }
}
