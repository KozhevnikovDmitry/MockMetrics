using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DataMapping
{
    public class PersonDataMapper : AbstractDataMapper<Person>
    {
        private readonly IDomainDataMapper<PersonAddress> _personAddressDataMapper;

        public PersonDataMapper(IDomainContext domainContext, 
                                IDomainDataMapper<PersonAddress> personAddressDataMapper)
            : base(domainContext)
        {
            _personAddressDataMapper = personAddressDataMapper;
        }

        /// <summary>
        /// Получить данные о Person
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="dbManager">Объект для работы с базой</param>
        /// <returns>Объект Person</returns>
        protected override Person RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Person>(id);

            // Контактная информация
            if ((from pc in dbManager.GetDomainTable<PersonContactInfo>()
                 where pc.Id == obj.Id
                 select pc.Id).Any())
            {
                obj.ContactInfo = dbManager.RetrieveDomainObject<PersonContactInfo>(obj.Id);
            }

            // загрузили список документов
            var pdList = (from pd in dbManager.GetDomainTable<PersonDoc>()
                            where pd.PersonId == obj.Id
                            select pd.Id).ToList()
                .Select(val => dbManager.RetrieveDomainObject<PersonDoc>(val)).ToList();
            obj.Documents = new EditableList<PersonDoc>(pdList);

            // Адреса регистрации/проживания
            var paList = (from pa in dbManager.GetDomainTable<PersonAddress>()
                            where pa.PersonId == obj.Id
                            select pa.Id).ToList()
                .Select(val => _personAddressDataMapper.Retrieve(val, dbManager)).ToList();

            obj.Addresses = new EditableList<PersonAddress>(paList);

            // Информация об инвалидности 
            if ((from pd in dbManager.GetDomainTable<PersonDisability>()
                 where pd.PersonId == obj.Id
                 select pd.Id).Any())
            {
                var pdId = (from pd in dbManager.GetDomainTable<PersonDisability>()
                          where pd.PersonId == obj.Id
                          select pd.Id).Single();
                obj.Disability = dbManager.RetrieveDomainObject<PersonDisability>(pdId);
            }

            return obj;
        }

        /// <summary>
        /// Сохранить объект
        /// </summary>
        /// <param name="obj">Идентификатор объекта</param>
        /// <param name="dbManager">Объект для работы с базой</param>
        /// <param name="forceSave"></param>
        /// <returns>Сохраненный объект</returns>
        protected override Person SaveOperation(Person obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var personClone = obj.Clone();

            dbManager.BeginDomainTransaction();

            //сохраняем информацию о персоне 
            dbManager.SaveDomainObject(personClone);

            // сохраняем контактнуюинформацию
            if(personClone.ContactInfo != null)
            {
                personClone.ContactInfo.Id = personClone.Id;
                dbManager.SaveDomainObject(personClone.ContactInfo);
            }

            // сохраняем информацию о документах
            if (personClone.Documents != null)
            {
                foreach (var pd in personClone.Documents)
                {
                    pd.PersonId = personClone.Id;
                    dbManager.SaveDomainObject(pd);
                }
            }

            // сохраняем адреса
            if (personClone.Addresses != null)
            {
                foreach (var pa in personClone.Addresses)
                {
                    pa.PersonId = personClone.Id;
                    _personAddressDataMapper.Save(pa, dbManager);
                }
            }

            // сохраняем информацию об инвалидности
            if (personClone.Disability != null)
            {
                personClone.Disability.PersonId = personClone.Id;
                dbManager.SaveDomainObject(personClone.Disability);
            }

            personClone.AcceptChanges();
            dbManager.CommitDomainTransaction();
            return personClone;
        }

        protected override void FillAssociationsOperation(Person obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
