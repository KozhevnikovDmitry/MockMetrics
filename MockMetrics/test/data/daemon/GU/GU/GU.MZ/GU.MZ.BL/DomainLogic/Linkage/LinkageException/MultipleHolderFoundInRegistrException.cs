using System.Collections.Generic;
using System.Linq;
using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключений для ошибок "Найдено несколько лицензиатов с заданными ИНН и ОГРН."
    /// </summary>
    public class MultipleHolderFoundInRegistrException : BLLException
    {
        /// <summary>
        /// Номера найденных лицензиатов
        /// </summary>
        public List<int> HolderIds { get; private set; }

        /// <summary>
        /// Класс исключений для ошибок "Найдено несколько лицензиатов с заданными ИНН и ОГРН."
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        /// <param name="holderIds">Номера найденных лицензиатов</param>
        public MultipleHolderFoundInRegistrException(string inn, string ogrn, IEnumerable<int> holderIds)
            : base(string.Format("Найдено несколько лицензиатов с заданными ИНН = {0} и ОГРН = {1}.", inn, ogrn))
        {
            HolderIds = holderIds.ToList();
        }
    }
}