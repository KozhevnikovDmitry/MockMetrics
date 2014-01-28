using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;

using Common.DA.Interface;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DataMapping
{
    /// <summary>
    /// категории заявления (таблица связи многие ко многим "категории" - "заявления")
    /// </summary>
    public class ClaimCategoryDataMapper : AbstractDataMapper<ClaimCategory>
    {
        public ClaimCategoryDataMapper(IDomainContext domainContext) : base(domainContext)
        {
        }

        /// <summary>
        /// Получить категорию заявления по идентифиатору
        /// </summary>
        /// <param name="id">Идентификатор категории заявления</param>
        /// <param name="dbManager">Объект для работы с базой</param>
        /// <returns>Категория заявления</returns>
        protected override ClaimCategory RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<ClaimCategory>(id);

            obj.Category = dbManager.RetrieveDomainObject<CategoryType>(obj.CategoryTypeId);

            return obj;
        }

        /// <summary>
        /// Сохранить категорию заявления
        /// </summary>
        /// <param name="obj">Идентификатор</param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override ClaimCategory SaveOperation(ClaimCategory obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var ссClone = obj.Clone();

            dbManager.SaveDomainObject(ссClone);

            ссClone.AcceptChanges();
            return ссClone;
        }

        protected override void FillAssociationsOperation(ClaimCategory obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
