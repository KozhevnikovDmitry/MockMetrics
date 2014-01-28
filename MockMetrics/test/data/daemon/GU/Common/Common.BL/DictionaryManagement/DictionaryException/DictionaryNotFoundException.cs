using System;

using Common.Types.Exceptions;

namespace Common.BL.DictionaryManagement.DictionaryException
{
    /// <summary>
    /// Класс исключение для ошибки "Справочник сущностей не найден".
    /// </summary>
    public class DictionaryNotFoundException : BLLException
    {
        public DictionaryNotFoundException(Type domainType)
            : base(string.Format("Справочник сущностей {0} не найден", domainType.Name))
        {
            
        }

        public DictionaryNotFoundException(string domainTypeName)
            : base(string.Format("Справочник сущностей {0} не найден", domainTypeName))
        {

        }
    }
}
