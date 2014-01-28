using System;
using System.Collections.Generic;

using Common.BL.Validation;

using GU.DataModel;

namespace GU.BL.Policy.Interface
{
    public interface ITaskPolicy : ITaskStatusPolicy
    {
        bool CanSetAgency(Agency agency, Task task);

        ValidationErrorInfo ValidateDocumentsData(Task task);

        ValidationErrorInfo ValidateBasic(Task task);

        ValidationErrorInfo Validate(Task task);

        ValidationErrorInfo CanSave(Task task);

        Task CreateDefaultTask(Service service, Agency agency = null);

        /// <summary>
        /// Создание пустой заявки
        /// </summary>
        /// <param name="service">Оказываемая услуга</param>
        /// <param name="agency">
        ///     Ведомство, которому направляется заявка.
        ///     Должно быть подведомством service.ServiceGroup.Agency
        ///     Если null, то в заявке используется service.ServiceGroup.Agency
        /// </param>
        /// <returns>Созданная заявка</returns>
        Task CreateEmptyTask(Service service, Agency agency = null);

        bool IsEditable(Task task);
    }
}
