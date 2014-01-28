using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Common.BL.DomainContext.DomainContextException;
using Common.DA;
using Common.DA.Interface;

namespace Common.BL.DomainContext
{
    /// <summary>
    /// Класс, предоставляющий доменный контекст для доменного зависимых классов.
    /// </summary>
    public class DomainContext : IDomainContext
    {
        /// <summary>
        /// Словарь соотнсящий тип менеджера базы данных с именем сборки доменно-зависимых классов
        /// </summary>
        private readonly Dictionary<string, Type> _dictionaryContainer = new Dictionary<string, Type>();

        /// <summary>
        /// Класс, предоставляющий доменный контекст для доменного зависимых классов.
        /// </summary>
        /// <param name="assemblies">Сборки доменно-зависимых классов</param>
        public DomainContext(IEnumerable<Assembly> assemblies)
        {
            assemblies.ToList().ForEach(RegisterDbManager);
        }

        /// <summary>
        /// Вовзвращает менеджер базы данных для доменного контекста.
        /// </summary>
        /// <param name="contextKey">Ключ доменного контекста</param>
        /// <returns>Менеджер базы данных</returns>
        /// <exception cref="NoDbManagerFoundException">Не обнаружено ни одно менеджера базы данных для доменного контекста.</exception>
        public IDomainDbManager GetDbManager(string contextKey)
        {
            if (!_dictionaryContainer.ContainsKey(contextKey))
            {
                throw new NoDbManagerFoundException();
            }
            return (IDomainDbManager)Activator.CreateInstance(_dictionaryContainer[contextKey]);
        }

        /// <summary>
        /// Регистрирует тип менеджера базы данных
        /// </summary>
        /// <param name="blAssembly">Сборка доменно-зависисых классов</param>
        /// <exception cref="NoDbManagerFoundException">Не обнаружено ни одно менеджера базы данных для доменного контекста.</exception>
        /// <exception cref="MultipleDbManagerDetectedException">Обнаружено больше одного менеджера базы данных для доменного контекста.</exception>
        private void RegisterDbManager(Assembly blAssembly)
        {
            try
            {
                var dbType = blAssembly.GetTypes().SingleOrDefault(t => typeof(IDomainDbManager).IsAssignableFrom(t));
                if (dbType == null)
                {
                    throw new NoDbManagerFoundException();
                }

                _dictionaryContainer[blAssembly.FullName] = dbType;
            }
            catch (InvalidOperationException)
            {
                throw new MultipleDbManagerDetectedException();
            }
        }
    }
}
