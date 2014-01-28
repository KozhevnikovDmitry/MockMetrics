using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Объект с номенклатурой
    /// </summary>
    public class LicenseObjectDataMapper : AbstractDataMapper<LicenseObject>
    {
        /// <summary>
        /// Класс маппер сущностей Объект с номенклатурой
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public LicenseObjectDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        #region Overrides of AbstractDataMapper<LicenseObject>

        /// <summary>
        /// Получает и возвращает объект с номенклатурой из базы данных
        /// </summary>
        /// <param name="id">Id объекта с номенклатурой</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Объект с номенклатурой</returns>
        protected override LicenseObject RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var licenseObject = dbManager.RetrieveDomainObject<LicenseObject>(id);

            licenseObject.Address = dbManager.RetrieveDomainObject<Address>(licenseObject.AddressId);

            var objectIds = dbManager.GetDomainTable<ObjectSubactivity>()
                                   .Where(d => d.LicenseObjectId == licenseObject.Id)
                                   .Select(d => d.Id)
                                   .ToList();

            licenseObject.ObjectSubactivityList =
                new EditableList<ObjectSubactivity>(
                    objectIds.Select(t => dbManager.RetrieveDomainObject<ObjectSubactivity>(t)).ToList());

            return licenseObject;
        }

        /// <summary>
        /// Сохраняет объект с номенклатурой в базу данных. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Объект с номенклатурой</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый объект с номенклатурой</returns>
        protected override LicenseObject SaveOperation(LicenseObject obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp.Address, forceSave);

            tmp.AddressId = tmp.Address.Id;

            dbManager.SaveDomainObject(tmp);

            if (tmp.ObjectSubactivityList != null)
            {
                if (tmp.ObjectSubactivityList.DelItems != null)
                {
                    foreach (var delItem in tmp.ObjectSubactivityList.DelItems.Cast<ObjectSubactivity>())
                    {
                        delItem.MarkDeleted();
                        dbManager.SaveDomainObject(delItem);
                    }
                }
                
                for (int i = 0; i < tmp.ObjectSubactivityList.Count; i++)
                {
                    tmp.ObjectSubactivityList[i].LicenseObjectId = tmp.Id;
                    dbManager.SaveDomainObject(tmp.ObjectSubactivityList[i]);
                }
            }

            return tmp;
        }

        protected override void DeleteOperation(LicenseObject obj, IDomainDbManager dbManager)
        {
            foreach (var objectSubactivity in obj.ObjectSubactivityList)
            {
                objectSubactivity.MarkDeleted();
                dbManager.SaveDomainObject(objectSubactivity);
            }

            base.DeleteOperation(obj, dbManager);
        }

        /// <summary>
        /// Заполняет отображаемые ассоциации объекта с номенклатурой
        /// </summary>
        /// <param name="obj">Объект с номенклатурой</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        protected override void FillAssociationsOperation(LicenseObject obj, IDomainDbManager dbManager)
        {
            
        }

        #endregion
    }
}
