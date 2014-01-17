using System;
using PostGrad.Core.Common;

namespace PostGrad.Core.BL
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
