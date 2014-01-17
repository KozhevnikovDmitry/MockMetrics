using System;
using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Класс исключение для ошибки "Требуемый элемент в справочнике сущностей не найден.".
    /// </summary>
    public class DictionaryItemNotFoundException : BLLException
    {
        public DictionaryItemNotFoundException(Type domainType)
            : base(string.Format("Требуемый элемент в справочнике сущностей {0} не найден.", domainType.Name))
        {
            
        }
    }
}
