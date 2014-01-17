using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.Core.BL
{
    public interface IDossierFileBuilder
    {
        /// <summary>
        /// Задаёт заявку, для которой будет создаваться том
        /// </summary>
        /// <param name="task">Объект заявка</param>
        /// <returns>Сборщик тома</returns>
        IDossierFileBuilder FromTask(Task task);

        /// <summary>
        /// Добавляет регистрационный номер описи.
        /// </summary>
        /// <param name="inventoryRegNumber">Регистрационный номер описи</param>
        /// <returns>Сборщик тома</returns>
        IDossierFileBuilder WithInventoryRegNumber(int? inventoryRegNumber);

        /// <summary>
        /// Добавляет отвественного сотрудника.
        /// </summary>
        /// <param name="responsibleEmployee">Отвественный сотрудник</param>
        /// <returns>Сборщик тома</returns>
        IDossierFileBuilder ToEmployee(Employee responsibleEmployee);

        /// <summary>
        /// Добавляет статус "Принято к рассмотрению"
        /// </summary>
        /// <param name="notice">Комментарий к статусу</param>
        /// <returns>Сборщик тома</returns>
        IDossierFileBuilder WithAcceptedStatus(string notice);

        /// <summary>
        /// Добавляет прилагаемый документ
        /// </summary>
        /// <param name="documentName">Имя документа</param>
        /// <param name="quantity">Количество</param>
        /// <returns>Сборщик тома</returns>
        /// <exception cref="CantAddProvidedDocumentWithEmptyNameException"></exception>
        /// <exception cref="CantAddProvidedDocumentWithNegativeQuantityException"></exception>
        IDossierFileBuilder AddProvidedDocument(string documentName, int quantity);

        /// <summary>
        /// Добавляет статус "Отклоненно"
        /// </summary>
        /// <param name="notice">Комментарий к статусу</param>
        /// <returns>Сборщик тома</returns>
        IDossierFileBuilder WithRejectedStatus(string notice);

        /// <summary>
        /// Возвращает собранный объект Том лицензионного дела
        /// </summary>
        /// <returns> собранный том</returns>
        /// <exception cref="BuildingDataNotCompleteException">Недостаточно данных для приёма или отклонения заявки</exception>
        /// <exception cref="CantSetStatusException">Невозможно установить статус для заявки</exception>
        DossierFile Build();
    }
}