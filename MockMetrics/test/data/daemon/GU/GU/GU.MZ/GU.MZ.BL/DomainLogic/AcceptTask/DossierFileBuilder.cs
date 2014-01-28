using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DictionaryManagement;

using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask.AcceptException;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DomainLogic.AcceptTask
{
    /// <summary>
    /// Класс, предназаначенный для конструирования объекта Том лицензионного дела.
    /// </summary>
    public class DossierFileBuilder
    {
        /// <summary>
        /// Заявка - основание завденения тома.
        /// </summary>
        private Task _task;

        /// <summary>
        /// Менеджер кэша справочников
        /// </summary>
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Политика управления статусами заявок.
        /// </summary>
        private readonly ITaskStatusPolicy _taskStatusPolicy;

        private readonly IParser _parser;


        /// <summary>
        /// Регистрационный номер описи.
        /// </summary>
        private int? _inventoryRegNumber;

        /// <summary>
        /// Отвественный сотрудник.
        /// </summary>
        private Employee _responsibleEmployee;

        /// <summary>
        /// Тип статуса.
        /// </summary>
        private TaskStatusType? _taskStatus;

        /// <summary>
        /// Комментарий к статусу.
        /// </summary>
        private string _statusNotice;

        /// <summary>
        /// Список прилагаемых документов
        /// </summary>
        private List<ProvidedDocument> _providedDocumentList;

        /// <summary>
        /// Класс, предназаначенный для конструирования объекта Том лицензионного дела.
        /// </summary>
        /// <param name="task">Заявка - основание завденения тома</param>
        /// <param name="taskStatusPolicy">Политика управления статусами заявок</param>
        public DossierFileBuilder(IDictionaryManager dictionaryManager, ITaskStatusPolicy taskStatusPolicy, IParser parser)
        {
            _dictionaryManager = dictionaryManager;
            _taskStatusPolicy = taskStatusPolicy;
            _parser = parser;
            _providedDocumentList = new List<ProvidedDocument>();
        }

        /// <summary>
        /// Задаёт заявку, для которой будет создаваться том
        /// </summary>
        /// <param name="task">Объект заявка</param>
        /// <returns>Сборщик тома</returns>
        public virtual DossierFileBuilder FromTask(Task task)
        {
            _task = task;
            return this;
        }

        /// <summary>
        /// Добавляет регистрационный номер описи.
        /// </summary>
        /// <param name="inventoryRegNumber">Регистрационный номер описи</param>
        /// <returns>Сборщик тома</returns>
        public virtual DossierFileBuilder WithInventoryRegNumber(int? inventoryRegNumber)
        {
            _inventoryRegNumber = inventoryRegNumber;
            return this;
        }

        /// <summary>
        /// Добавляет отвественного сотрудника.
        /// </summary>
        /// <param name="responsibleEmployee">Отвественный сотрудник</param>
        /// <returns>Сборщик тома</returns>
        public virtual DossierFileBuilder ToEmployee(Employee responsibleEmployee)
        {
            _responsibleEmployee = responsibleEmployee;
            return this;
        }

        /// <summary>
        /// Добавляет статус "Принято к рассмотрению"
        /// </summary>
        /// <param name="notice">Комментарий к статусу</param>
        /// <returns>Сборщик тома</returns>
        public DossierFileBuilder WithAcceptedStatus(string notice)
        {
            _taskStatus = TaskStatusType.Accepted;
            _statusNotice = notice;
            return this;
        }

        /// <summary>
        /// Добавляет прилагаемый документ
        /// </summary>
        /// <param name="documentName">Имя документа</param>
        /// <param name="quantity">Количество</param>
        /// <returns>Сборщик тома</returns>
        /// <exception cref="CantAddProvidedDocumentWithEmptyNameException"></exception>
        /// <exception cref="CantAddProvidedDocumentWithNegativeQuantityException"></exception>
        public virtual DossierFileBuilder AddProvidedDocument(string documentName, int quantity)
        {
            var doc = ProvidedDocument.CreateInstance();
            doc.Name = documentName.Trim();
            doc.Quantity = quantity;

            if (string.IsNullOrEmpty(doc.Name))
            {
                throw new CantAddProvidedDocumentWithEmptyNameException();
            }

            if (doc.Quantity <= 0)
            {
                throw new CantAddProvidedDocumentWithNegativeQuantityException();
            }

            _providedDocumentList.Add(doc);

            return this;
        }

        /// <summary>
        /// Добавляет статус "Отклоненно"
        /// </summary>
        /// <param name="notice">Комментарий к статусу</param>
        /// <returns>Сборщик тома</returns>
        public virtual DossierFileBuilder WithRejectedStatus(string notice)
        {
            _taskStatus = TaskStatusType.Rejected;
            _statusNotice = notice;
            return this;
        }

        /// <summary>
        /// Возвращает собранный объект Том лицензионного дела
        /// </summary>
        /// <returns> собранный том</returns>
        /// <exception cref="BuildingDataNotCompleteException">Недостаточно данных для приёма или отклонения заявки</exception>
        /// <exception cref="CantSetStatusException">Невозможно установить статус для заявки</exception>
        public DossierFile Create()
        {
            if (!IsDataComplete())
            {
                throw new BuildingDataNotCompleteException();
            }

            var dossierFile = DossierFile.CreateInstance();

            dossierFile.CreateDate = DateTime.Today;

            dossierFile.Task = SetBuildStatusToTask();
            dossierFile.TaskId = dossierFile.Task.Id;

            if (_taskStatus == TaskStatusType.Accepted)
            {
                var activity =
                    _dictionaryManager.GetDictionary<LicensedActivity>()
                        .Single(t => t.ServiceGroupId == dossierFile.ServiceGroupId);

                dossierFile.CurrentStatus = DossierFileStatus.Unbounded;

                dossierFile.DocumentInventory = DocumentInventory.CreateInstance();
                dossierFile.DocumentInventory.Stamp = DateTime.Today;
                dossierFile.DocumentInventory.RegNumber = _inventoryRegNumber.Value;
                dossierFile.DocumentInventory.LicenseHolder = _parser.ParseHolderInfo(_task).FullName;
                dossierFile.DocumentInventory.EmployeeName = _responsibleEmployee.Name;
                dossierFile.DocumentInventory.EmployeePosition = _responsibleEmployee.Position;
                dossierFile.DocumentInventory.LicensedActivity = activity.Name;

                dossierFile.DocumentInventory.ProvidedDocumentList 
                    = new EditableList<ProvidedDocument>(_providedDocumentList);

                dossierFile.Employee = _responsibleEmployee;
                dossierFile.EmployeeId = _responsibleEmployee.Id;
                dossierFile.Scenario = GetScenario();
                dossierFile.ScenarioId = dossierFile.Scenario.Id;
                dossierFile.CurrentScenarioStepId =
                    dossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[1].Id;
                AddFirstSteps(dossierFile);

                dossierFile.LicensedActivity = activity;
            }

            return dossierFile;
        }

        /// <summary>
        /// Добавляет первые два этапа ведения тома - "Назначение исполнителя" и "Привязка тома к делу"
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        private void AddFirstSteps(DossierFile dossierFile)
        {
            dossierFile.DossierFileStepList = new EditableList<DossierFileScenarioStep>();
            for (int i = 0; i < 2; i++)
            {
                var step = DossierFileScenarioStep.CreateInstance();
                step.ScenarioStepId = dossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList()[i].Id;
                step.EmployeeId = dossierFile.EmployeeId;
                step.StartDate = DateTime.Now;
                dossierFile.DossierFileStepList.Add(step);
            }
        }

        /// <summary>
        /// Устанавливает статус заявке. Возврвщает заявку со статусом
        /// </summary>
        /// <returns>Заявка со статусом</returns>
        /// <exception cref="CantSetStatusException">Невозможно установить статус для заявки</exception>
        private Task SetBuildStatusToTask()
        {
            
            if (_taskStatusPolicy.CanSetStatus(_taskStatus.Value, _task))
            {
                _task = _taskStatusPolicy.SetStatus(_taskStatus.Value, _statusNotice, _task);
            }
            else
            {
                throw new CantSetStatusException(_taskStatus.Value);
            }

            return _task;
        }

        /// <summary>
        /// Возвращает сценарий ведения тома.
        /// </summary>
        /// <returns>Сценарий ведения</returns>
        /// <exception cref="WrongScenarioSteplistException">Сценарий ведения тома не сформирован</exception>
        private Scenario GetScenario()
        {
            var scenario = _dictionaryManager.GetDictionary<Scenario>().SingleOrDefault(s => s.ServiceId == _task.ServiceId);

            if (scenario == null ||
                scenario.ScenarioStepList == null ||
                scenario.ScenarioStepList.Count < 2)
            {
                throw new WrongScenarioSteplistException();
            }

            return scenario;
        }

        /// <summary>
        /// Возвращет флаг полноты заведённых данных для сборки тома.
        /// </summary>
        /// <returns>Флаг полноты заведённых данных для сборки тома</returns>
        private bool IsDataComplete()
        {
            if (_task == null)
            {
                return false;
            }

            if (!_taskStatus.HasValue)
            {
                return false;
            }

            if (_taskStatus.Value == TaskStatusType.Accepted)
            {
                return _inventoryRegNumber.HasValue && _responsibleEmployee != null;
            }
            else
            {
                return !string.IsNullOrEmpty(_statusNotice);
            }
        }
    }
}
