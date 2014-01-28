using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.MZ.BL.DomainLogic.Inspect.ExpertiseException;
using GU.MZ.BL.DomainLogic.Inspect.InspectException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DomainLogic.Inspect
{
    /// <summary>
    /// Класс инспектор, ответсвенный за занесение результатов выездной проверки
    /// </summary>
    public class HolderInspecter
    {
        public Inspection PrepareHolderInspection(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            if (dossierFile.StepInspection(scenarioStep) != null)
            {
                throw new PrepareMoreThanOneInspectionException();
            }

            var inspection = Inspection.CreateInstance();
            inspection.Id = dossierFile.GetStep(scenarioStep).Id;
            inspection.ActStamp = DateTime.Now.AddDays(2);
            inspection.StartStamp = DateTime.Now;
            inspection.EndStamp = DateTime.Now.AddDays(1);
            inspection.IsPassed = false;
            inspection.InspectionExpertList = new EditableList<InspectionExpert>();
            inspection.InspectionEmployeeList = new EditableList<InspectionEmployee>();
            dossierFile.GetStep(scenarioStep).Inspection = inspection;
            return inspection;
        }

        public List<Employee> GetAvailableEmployees(IDictionaryManager dictionaryManager,
                                                    DossierFile dossierFile,
                                                    ScenarioStep scenarioStep)
        {
            var inspection = dossierFile.StepInspection(scenarioStep);

            if (inspection == null)
            {
                throw new NoInspectionFoundException();
            }

            var employees = dictionaryManager.GetDynamicDictionary<Employee>();

            return
                employees.Where(
                    t => !inspection.InspectionEmployeeList.Select(e => e.EmployeeId).Contains(t.Id))
                    .ToList();
        }

        public List<Expert> GetAvailableExperts(IDictionaryManager dictionaryManager,
                                                DossierFile dossierFile,
                                                ScenarioStep scenarioStep)
        {
            var inspection = dossierFile.StepInspection(scenarioStep);

            if (inspection == null)
            {
                throw new NoInspectionFoundException();
            }

            var experts = dictionaryManager.GetDynamicDictionary<Expert>();

            return
                experts.Where(t => !inspection.InspectionExpertList.Select(e => e.ExpertId).Contains(t.Id))
                       .ToList();

        }

        public InspectionEmployee AddInspectionEmployee(Employee employee,
                                                        DossierFile dossierFile,
                                                        ScenarioStep scenarioStep)
        {
            var inspection = dossierFile.StepInspection(scenarioStep);

            if (inspection == null)
            {
                throw new AddEmployeeBeforePrepareInspectionException();
            }

            var inspectionEmployee = InspectionEmployee.CreateInstance();
            inspectionEmployee.InspectionId = inspection.Id;
            inspectionEmployee.Employee = employee;
            inspectionEmployee.EmployeeId = employee.Id;
            inspection.InspectionEmployeeList.Add(inspectionEmployee);

            return inspectionEmployee;
        }

        public InspectionExpert AddInspectionExpert(Expert expert,
                                                    DossierFile dossierFile,
                                                    ScenarioStep scenarioStep)
        {
            var inspection = dossierFile.StepInspection(scenarioStep);

            if (inspection == null)
            {
                throw new AddExpertBeforePrepareInspectionException();
            }

            var inspectionExpert = InspectionExpert.CreateInstance();
            inspectionExpert.InspectionId = inspection.Id;
            inspectionExpert.Expert = expert;
            inspectionExpert.ExpertId = expert.Id;
            inspection.InspectionExpertList.Add(inspectionExpert);

            return inspectionExpert;
        }
    }
}
