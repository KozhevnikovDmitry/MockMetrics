using System;
using System.Collections.Generic;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Inspect.ExpertiseException;
using GU.MZ.DataModel.Dossier;

using System.Linq;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.BL.DomainLogic.Inspect
{
    /// <summary>
    /// Класс, ответсвенный за занесение данных документарной проверки
    /// </summary>
    public class DocumentExpert
    {
        public DocumentExpert()
        {
            
        }

        public DocumentExpertise PrepareExpertise(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            if (dossierFile.StepDocumentExpertise(scenarioStep) != null)
            {
                throw new PrepareMoreThanOneExpertiseException();
            }

            var expertise = DocumentExpertise.CreateInstance();
            expertise.Id = dossierFile.GetStep(scenarioStep).Id;
            expertise.ActStamp = DateTime.Now.AddDays(2);
            expertise.StartStamp = DateTime.Now;
            expertise.EndStamp = DateTime.Now.AddDays(1);
            expertise.ExperiseResultList = new EditableList<DocumentExpertiseResult>();
            dossierFile.GetStep(scenarioStep).DocumentExpertise = expertise;
            return expertise;
        }

        public DocumentExpertiseResult AddExpertiseResult(DossierFile dossierFile, ScenarioStep scenarioStep, ExpertedDocument expertedDocument, IDictionaryManager dictionaryManager)
        {
            if (!GetAvailableDocs(dictionaryManager, dossierFile.Service).Contains(expertedDocument))
            {
                throw new UnavailableDocumentToAddResultException();
            }


            if (dossierFile.StepDocumentExpertise(scenarioStep) == null)
            {
                throw new AddResultBeforePrepareExpertiseException();
            }

            var expertiseResult = DocumentExpertiseResult.CreateInstance();
            expertiseResult.DocumentExpertiseId = dossierFile.StepDocumentExpertise(scenarioStep).Id;
            expertiseResult.ExpertedDocumentId = expertedDocument.Id;
            expertiseResult.IsDocumentValid = true;

            dossierFile.StepDocumentExpertise(scenarioStep).ExperiseResultList.Add(expertiseResult);

            return expertiseResult;
        }

        public virtual List<ExpertedDocument> GetAvailableDocs(IDictionaryManager dictionaryManager, Service service)
        {
            return
               dictionaryManager.GetDictionary<ExpertedDocument>()
                                 .Where(t => t.ServiceId == service.Id)
                                 .ToList();
        }
    }
}
