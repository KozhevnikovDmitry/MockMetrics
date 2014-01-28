using System;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;


namespace GU.HQ.BL.DataMapping
{
    /// <summary>
    /// Родственник заявителя
    /// </summary>
    public class DeclarerRelativeDataMapper : AbstractDataMapper<DeclarerRelative>
    {
        public DeclarerRelativeDataMapper(IDomainContext domainContext) 
            : base(domainContext)
        {
        }

        /// <summary>
        /// Получитьобъект по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        protected override DeclarerRelative RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<DeclarerRelative>(id);

            obj.Person = dbManager.RetrieveDomainObject<Person>(obj.PersonId);

            return obj;
        }

        /// <summary>
        /// Сохранить объект
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override DeclarerRelative SaveOperation(DeclarerRelative obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var drClone = obj.Clone();

            dbManager.SaveDomainObject(drClone.Person);

            drClone.PersonId = drClone.Person.Id;

            dbManager.SaveDomainObject(drClone);

            drClone.AcceptChanges();
            return drClone;
        }

        /// <summary>
        /// Заполняет личность родственника при получении родственника для заявителя
        /// Вытаскиваем пользователя когда открываем спсок родтсвнников в заявке
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        protected override void FillAssociationsOperation(DeclarerRelative obj, IDomainDbManager dbManager)
        {
            obj.Person = dbManager.RetrieveDomainObject<Person>(obj.PersonId);
        }
    }
}
