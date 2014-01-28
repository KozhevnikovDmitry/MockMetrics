using System;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.DataModel;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей выездная проверка
    /// </summary>
    public class InspectionDataMapper : AbstractDataMapper<Inspection>
    {
        /// <summary>
        /// Маппер экспертов
        /// </summary>
        private readonly IDomainDataMapper<Expert> _expertMapper;

        /// <summary>
        /// Класс маппер сущностей выездная проверка
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="expertMapper">Маппер экспертов</param>
        public InspectionDataMapper(IDomainContext domainContext, IDomainDataMapper<Expert> expertMapper)
            : base(domainContext)
        {
            _expertMapper = expertMapper;
        }

        protected override Inspection RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var inspection = dbManager.RetrieveDomainObject<Inspection>(id);

            inspection.InspectionEmployeeList =
                new EditableList<InspectionEmployee>(
                    dbManager.GetDomainTable<InspectionEmployee>()
                             .Where(t => t.InspectionId == inspection.Id)
                             .Select(t => t.Id).ToList()
                             .Select(t => dbManager.RetrieveDomainObject<InspectionEmployee>(t))
                             .ToList());

            foreach (var inspectionEmployee in inspection.InspectionEmployeeList)
            {
                inspectionEmployee.Employee = dbManager.RetrieveDomainObject<Employee>(inspectionEmployee.EmployeeId);
                inspectionEmployee.Employee.DbUser =
                    dbManager.RetrieveDomainObject<DbUser>(inspectionEmployee.Employee.DbUserId);
            }

            inspection.InspectionExpertList =
                new EditableList<InspectionExpert>(
                    dbManager.GetDomainTable<InspectionExpert>()
                             .Where(t => t.InspectionId == inspection.Id)
                             .Select(t => t.Id).ToList()
                             .Select(t => dbManager.RetrieveDomainObject<InspectionExpert>(t))
                             .ToList());

            foreach (var inspectionExpert in inspection.InspectionExpertList)
            {
                inspectionExpert.Expert = _expertMapper.Retrieve(inspectionExpert.ExpertId, dbManager);
            }

            return inspection;
        }

        protected override Inspection SaveOperation(Inspection obj, IDomainDbManager dbManager, bool forceSave = false)
        {

            var tmp = obj.Clone();

            int id = tmp.Id;
            dbManager.SaveDomainObject(tmp);
            tmp.Id = id;

            foreach (var t in tmp.InspectionEmployeeList)
            {
                t.InspectionId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.InspectionEmployeeList.DelItems != null)
            {
                foreach (var delItem in tmp.InspectionEmployeeList.DelItems.Cast<InspectionEmployee>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            foreach (var t in tmp.InspectionExpertList)
            {
                t.InspectionId = tmp.Id;
                dbManager.SaveDomainObject(t);
            }

            if (tmp.InspectionExpertList.DelItems != null)
            {
                foreach (var delItem in tmp.InspectionExpertList.DelItems.Cast<InspectionExpert>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }

            return tmp;
        }

        protected override void DeleteOperation(Inspection obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            foreach (var t in tmp.InspectionEmployeeList)
            {
                t.MarkDeleted();
                dbManager.SaveDomainObject(t);
            }

            foreach (var t in tmp.InspectionExpertList)
            {
                t.MarkDeleted();
                dbManager.SaveDomainObject(t);
            }

            tmp.MarkDeleted();
            dbManager.SaveDomainObject(tmp);
        }

        protected override void FillAssociationsOperation(Inspection obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
