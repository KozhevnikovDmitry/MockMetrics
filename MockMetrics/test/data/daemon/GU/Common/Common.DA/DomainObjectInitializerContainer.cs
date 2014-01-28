using System;
using System.Collections.Generic;
using System.Reflection;

using Common.DA.Interface;

namespace Common.DA
{   
    /// <summary>
    /// Класс-контейнер инициализаторов для доменных объектов
    /// </summary>
    /// <remarks>
    /// Контейнер хранит инициализаторы по принципу: один инициализатор на сборку доменных объектов.
    /// </remarks>
    internal class DomainObjectInitializerContainer
    {
        #region Singleton

        private static DomainObjectInitializerContainer _instance;
        private static readonly object _locker = new object();

        public static DomainObjectInitializerContainer Instance
        {
            get 
            {
                if (_instance == null)
                    lock (_locker)
                        if (_instance == null)
                            _instance = new DomainObjectInitializerContainer();

                return _instance;
            }
        }

        private DomainObjectInitializerContainer()
        {
            _initializers = new Dictionary<string, IDomainObjectInitializer>();
        }

        #endregion

        /// <summary>
        /// Словарь инициализаторов. В качестве ключа - полное имя сборки доменных объектов
        /// </summary>
        private readonly Dictionary<string, IDomainObjectInitializer> _initializers;

        /// <summary>
        /// Регистрирует инициализатор, ассоциируя его со сборкой доменных объектов.
        /// </summary>
        /// <param name="dataModelAssembly">Сборка доменных объектов</param>
        /// <param name="objectInitializer">Инициализатор доменных объектов</param>
        public void RegisterInitializer(Assembly dataModelAssembly, IDomainObjectInitializer objectInitializer)
        {
            _initializers[dataModelAssembly.FullName] = objectInitializer;
        }

        /// <summary>
        /// Возврвщает инициализатор для типа доменных объектов.
        /// </summary>
        /// <param name="domainObjectType">Тип доменных объектов</param>
        /// <returns>>Инициализатор доменных объектов</returns>
        public IDomainObjectInitializer ResolveInitializer(Type domainObjectType)
        {
            string key = domainObjectType.BaseType.Assembly.FullName;
            if(_initializers.ContainsKey(key))
            {
                return _initializers[key];
            }
            else
            {
                return new StubDomainObjectInitializer();
            }
        }
    }

    /// <summary>
    /// Класс инициализатор-заглушка.
    /// </summary>
    /// <remarks>
    /// Класс предназначен для подстановки в инициализацию доменных объектов до завершения инициализации приложения.
    /// </remarks>
    internal class StubDomainObjectInitializer : IDomainObjectInitializer
    {
        public void InitializeObject<T>(T obj) where T : IPersistentObject
        {

        }

        public void InitializeObjectCommonData<T>(T obj, ReplicaActionType action) where T : IPersistentObject
        {

        }
    }
}
