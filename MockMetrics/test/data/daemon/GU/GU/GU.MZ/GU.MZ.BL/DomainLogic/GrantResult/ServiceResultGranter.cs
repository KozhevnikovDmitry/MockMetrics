using System;
using System.Linq;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;

using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GrantResult.GrantResultException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;

namespace GU.MZ.BL.DomainLogic.GrantResult
{
    /// <summary>
    /// Класс, отвественный за назначение результата предоставления услуги
    /// </summary>
    public class ServiceResultGranter
    {
        /// <summary>
        /// Заводит результат предоставления услуги для тома    
        /// </summary>
        /// <exception cref="NotSingleResulstOnLightScenarioException">Неопределённый результат предоставления услуги для облегчённого сценария</exception>
        /// <exception cref="GranterWorksWithTaskWithWrongStatusException">Попытка заведения результата на том с заявкой в неправильном статусе</exception>
        /// <returns>Результат тома</returns>
        public DossierFileServiceResult GrantServiseResult(DossierFile dossierFile, IDictionaryManager dictionaryManager)
        {
            var result = DossierFileServiceResult.CreateInstance();
            result.Stamp = DateTime.Now;

            dossierFile.DossierFileServiceResult = result;

            SetServiceResult(dossierFile, dictionaryManager);

            return result;
        }

        /// <summary>
        /// Присваивает результат предоставления услуги результату тома
        /// </summary>
        /// <exception cref="NotSingleResulstOnLightScenarioException">Неопределённый результат предоставления услуги для облегчённого сценария</exception>
        private void SetServiceResult(DossierFile dossierFile, IDictionaryManager dictionaryManager)
        {
            var scenario = dictionaryManager.GetDictionaryItem<Scenario>(dossierFile.ScenarioId);

            var serviceResults =
                dictionaryManager.GetDictionary<ServiceResult>().Where(t => t.ScenarioId == scenario.Id);

            try
            {
                if (scenario.ScenarioType == ScenarioType.Light)
                {
                    dossierFile.DossierFileServiceResult.ServiceResult = serviceResults.Single();
                    dossierFile.DossierFileServiceResult.ServiceResultId = dossierFile.DossierFileServiceResult.ServiceResult.Id;
                }
                else
                {
                    var isPositive = IsResultPositive(dossierFile);
                    dossierFile.DossierFileServiceResult.ServiceResult = serviceResults.Single(t => t.IsPositive == isPositive);
                    dossierFile.DossierFileServiceResult.ServiceResultId = dossierFile.DossierFileServiceResult.ServiceResult.Id;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new NotSingleResulstOnLightScenarioException(ex);
            }
        }

        /// <summary>
        /// Сохраняет результат ведения тома        
        /// </summary>
        /// <exception cref="GranterWorksWithTaskWithWrongStatusException">Попытка работы с результатом тома по заявкой в неправильном статусе</exception>
        /// <returns>Сохранённый том</returns>
        public DossierFile SaveServiceResult(DossierFile dossierFile, ITaskStatusPolicy taskPolicy, IDomainDataMapper<DossierFile> fileMapper)
        {
            var tmp = dossierFile.Clone();

            var isPositive = IsResultPositive(tmp);

            if (isPositive)
            {
                taskPolicy.SetStatus(TaskStatusType.Done, string.Empty, tmp.Task);
            }

            return fileMapper.Save(tmp);
        }

        /// <summary>
        /// Возвращает true, если можно присвоить положительный результат тома
        /// </summary>
        /// <exception cref="GranterWorksWithTaskWithWrongStatusException">Попытка работы с результатом тома по заявкой в неправильном статусе</exception>
        /// <returns>True, если можно присвоить положительный результат тома</returns>
        private bool IsResultPositive(DossierFile dossierFile)
        {
            if (dossierFile.TaskState == TaskStatusType.Ready)
            {
                return true;
            }

            if (dossierFile.TaskState == TaskStatusType.Rejected)
            {
                return false;
            }

            throw new GranterWorksWithTaskWithWrongStatusException();
        }
    }
}
