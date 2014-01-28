using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.DataModel;
using GU.HQ.DataModel;


namespace GU.HQ.BL.DataMapping
{
    /// <summary>
    /// Clim Data Mapping
    /// </summary>
    /// TODO: Разнести на несколько методов слишкомонстрообразно! Тяжело читать
    public class ClaimDataMapper : AbstractDataMapper<Claim>
    {
        private readonly IDomainDataMapper<Person>  _personDataMapper;
        private readonly IDomainDataMapper<Task>    _taskDataMapper;
        private readonly IDomainDataMapper<DeclarerBaseReg> _declarerRegBaseMapper;
        private readonly IDomainDataMapper<QueuePriv> _queuePrivMapper;
        private readonly IDomainDataMapper<Address> _addressMapper; 

        public ClaimDataMapper(IDomainContext domainContext, 
                                IDomainDataMapper<Person> personDataMapper, 
                                IDomainDataMapper<Task>  taskDataMapper,
                                IDomainDataMapper<DeclarerBaseReg> declarerRegBaseMapper,
                                IDomainDataMapper<QueuePriv> queuePrivMapper,
                                IDomainDataMapper<Address> addressMapper)
            : base(domainContext)
        {
            _personDataMapper = personDataMapper;
            _taskDataMapper = taskDataMapper;
            _declarerRegBaseMapper = declarerRegBaseMapper;
            _queuePrivMapper = queuePrivMapper;
            _addressMapper = addressMapper;
        }

        #region RetriveOperation

        /// <summary>
        ///  получить информацию о регистрации заявления
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        private void RetrieveOperationClaimReg(Claim obj, IDomainDbManager dbManager)
        {
            // информация о регистрации заявления
            if ((from cr in dbManager.GetDomainTable<ClaimQueueReg>()
                 where cr.Id == obj.Id
                 select cr.Id).Any())
                obj.QueueReg = dbManager.RetrieveDomainObject<ClaimQueueReg>(obj.Id);

            // список категорий учета
            var ccList = (from cc in dbManager.GetDomainTable<ClaimCategory>()
                          where cc.ClaimId == obj.Id
                          select cc.Id).ToList().
                Select(val => dbManager.RetrieveDomainObject<ClaimCategory>(val)).ToList();

            obj.ClaimCategories = new EditableList<ClaimCategory>(ccList);
            
            // уведомления о постоновке на учет
            var cnList = (from cn in dbManager.GetDomainTable<Notice>()
                            where cn.ClaimId == obj.Id
                            select cn.Id).ToList().
                Select(val => dbManager.RetrieveDomainObject<Notice>(val)).ToList();
            
            obj.Notices = new EditableList<Notice>(cnList);
        }


        /// <summary>
        /// Информация о снятии с регистрации
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        private void RetrieveOperationClaimDeReg(Claim obj, IDomainDbManager dbManager)
        {

            // Информация о снятии заявления с регистрации
            if ((from cqdr in dbManager.GetDomainTable<ClaimQueueDeReg>()
                 where cqdr.ClaimId == obj.Id
                 select cqdr.Id).Any())
            {

                var cqdrId = (from cqdr in dbManager.GetDomainTable<ClaimQueueDeReg>()
                              where cqdr.ClaimId == obj.Id
                              select cqdr.Id).Single();

                obj.QueueDeReg = dbManager.RetrieveDomainObject<ClaimQueueDeReg>(cqdrId);
            }

            // информация о предоставленном жилье
            if ((from chp in dbManager.GetDomainTable<HouseProvided>()
                 where chp.ClaimId == obj.Id
                 select chp.Id).Any())
            {
                var chpId = (from chp in dbManager.GetDomainTable<HouseProvided>()
                             where chp.ClaimId == obj.Id
                             select chp.Id).Single();

                obj.HouseProvided = dbManager.RetrieveDomainObject<HouseProvided>(chpId);

                if (obj.HouseProvided.AddressId != null)
                    obj.HouseProvided.Address = _addressMapper.Retrieve(obj.HouseProvided.AddressId, dbManager);
            }
        }

        /// <summary>
        /// информация об очереди
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        private void RetriveOperationQueueClaim(Claim obj, IDomainDbManager dbManager)
        {
            if((from qc in dbManager.GetDomainTable<QueueClaim>()
                    where qc.ClaimId == obj.Id
                    select qc.Id).Any())
            {
                var qiId = (from qc in dbManager.GetDomainTable<QueueClaim>()
                            where qc.ClaimId == obj.Id
                            select qc.Id).Single();

                obj.QueueClaim = dbManager.RetrieveDomainObject<QueueClaim>(qiId);

                obj.QueueClaim.Queue = dbManager.RetrieveDomainObject<Queue>(obj.QueueClaim.QueueId);
            }
        }


        #endregion RetriveOperation

        /// <summary>
        /// Get object Claim
        /// </summary>
        /// <param name="id">Object Id</param>
        /// <param name="dbManager">dbManager</param>
        /// <returns>Claim</returns>
        protected override Claim RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Claim>(id);

            //task
            if (obj.TaskId != 0)
                obj.Task = dbManager.RetrieveDomainObject<Task>(obj.TaskId);

            //заявитель
            obj.Declarer = _personDataMapper.Retrieve(obj.DeclarerId, dbManager);

            // родственники заявителя
            var cdrList = (from cdr in dbManager.GetDomainTable<DeclarerRelative>()
                           where cdr.ClaimId == obj.Id
                           select cdr.Id).ToList().
                Select(val => dbManager.RetrieveDomainObject<DeclarerRelative>(val)).ToList();

            obj.Relatives = new EditableList<DeclarerRelative>(cdrList);
            foreach (var dr in obj.Relatives)
                dr.Person = _personDataMapper.Retrieve(dr.PersonId, dbManager);

            
            // основания учета которые указал заявитель
            var dbrId = (from d in dbManager.GetDomainTable<DeclarerBaseReg>()
                       where d.ClaimId == obj.Id
                       select d.Id).SingleOrDefault();
            if (dbrId != 0 )
                obj.DeclarerBaseReg = _declarerRegBaseMapper.Retrieve(dbrId, dbManager);
            
            // регистрация заявления
            RetrieveOperationClaimReg(obj, dbManager);

            // информация о внеочереднике
            var cqpList = (from cqp in dbManager.GetDomainTable<QueuePriv>()
                            where cqp.ClaimId == obj.Id
                            select cqp.Id).ToList().
                Select(val => _queuePrivMapper.Retrieve(val)).ToList();

            obj.QueuePrivList = new EditableList<QueuePriv>(cqpList);
           
            // снятие с регистрации
            RetrieveOperationClaimDeReg(obj, dbManager);
            
            // история заявления
            var cshList = (from csh in dbManager.GetDomainTable<ClaimStatusHist>()
                           where csh.ClaimId == obj.Id
                           select csh.Id).ToList().
                Select(val => dbManager.RetrieveDomainObject<ClaimStatusHist>(val)).ToList();

            obj.ClaimStatusHist = new EditableList<ClaimStatusHist>(cshList);

            // получить информацию об очереди
            RetriveOperationQueueClaim(obj, dbManager);

            return obj;
        }

        #region Save Claim

        private void SaveOperationClaimDeclarerRelatives(Claim obj, IDomainDbManager dbManager)
        {
            // сохраняем список родственников заявителя
            if (obj.Relatives != null)
            {
                foreach (var cr in obj.Relatives)
                {
                    cr.ClaimId = obj.Id;
                    cr.Person = _personDataMapper.Save(cr.Person, dbManager);
                    cr.PersonId = cr.Person.Id;
                    dbManager.SaveDomainObject(cr);
                }
            }

            // удаление родственников удаленных из списка
            if (obj.Relatives != null && obj.Relatives.DelItems != null)
            {
                foreach (var crd in obj.Relatives.DelItems.Cast<DeclarerRelative>())
                {
                    crd.MarkDeleted();
                    crd.Person.MarkDeleted();
                    dbManager.SaveDomainObject(crd);
                    _personDataMapper.Save(crd.Person, dbManager);
                }
            }
        }


        /// <summary>
        /// Сохраняем информацию о регистрации объекта
        /// </summary>
        /// <param name="obj">Claim</param>
        /// <param name="dbManager"></param>
        private void SaveOperationClaimReg (Claim obj, IDomainDbManager dbManager)
        {
            // удаление категорий учетаиз списка
            if (obj.ClaimCategories != null && obj.ClaimCategories.DelItems != null)
            {
                foreach (var ccd in obj.ClaimCategories.DelItems.Cast<ClaimCategory>())
                {
                    ccd.MarkDeleted();
                    dbManager.SaveDomainObject(ccd);
                }
            }

            // список категорий учета
            if (obj.ClaimCategories != null)
                foreach (var cc in obj.ClaimCategories)
                {
                    cc.ClaimId = obj.Id;
                    dbManager.SaveDomainObject(cc);
                }

            // информация о регистрации заявления
            if (obj.QueueReg != null)
            {
                obj.QueueReg.Id = obj.Id;
                dbManager.SaveDomainObject(obj.QueueReg);
            }

            // удаление уведомлений о постоновке на учет удаленных из списка (базы)
            if (obj.Notices != null && obj.Notices.DelItems != null)
            {
                foreach (var crd in obj.Notices.DelItems.Cast<Notice>())
                {
                    crd.MarkDeleted();
                    dbManager.SaveDomainObject(crd);
                }
            }

            // уведомления о постоновке на учет
            if (obj.Notices != null)
            {
                foreach (var cn in obj.Notices)
                {
                    cn.ClaimId = obj.Id;
                    dbManager.SaveDomainObject(cn);
                }
            }
        }

        /// <summary>
        /// Снятие с регистрации
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        private void SaveOperationClaimDeReg(Claim obj, IDomainDbManager dbManager)
        {
            // Информация о снятии заявления с регистрации
            if (obj.QueueDeReg != null)
            {
                obj.QueueDeReg.ClaimId = obj.Id;
                dbManager.SaveDomainObject(obj.QueueDeReg);
            }

            // информация о предоставленном жилье
            if (obj.HouseProvided != null)
            {
                obj.HouseProvided.ClaimId = obj.Id;
                
                if (obj.HouseProvided.Address != null)
                {
                    obj.HouseProvided.Address = _addressMapper.Save(obj.HouseProvided.Address, dbManager);
                    obj.HouseProvided.AddressId = obj.HouseProvided.Address.Id;
                }
                dbManager.SaveDomainObject(obj.HouseProvided);
            }
        }

        /// <summary>
        /// Сохранение информаци о регистрации в очереди внеочередников
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        private void SaveOperationClaimPriv(Claim obj, IDomainDbManager dbManager)
        {
            if (obj.QueuePrivList == null) return;

            foreach (var qpClaim in obj.QueuePrivList)
            {
                qpClaim.ClaimId = obj.Id;

                if (qpClaim.QueuePrivReg != null)
                {
                    dbManager.SaveDomainObject(qpClaim.QueuePrivReg);
                    qpClaim.QueuePrivRegId = qpClaim.QueuePrivReg.Id;
                }

                if (qpClaim.QueuePrivDeReg != null)
                {
                    dbManager.SaveDomainObject(qpClaim.QueuePrivDeReg);
                    qpClaim.QueuePrivDeRegId = qpClaim.QueuePrivDeReg.Id;
                }

                dbManager.SaveDomainObject(qpClaim);
            }
        }

        #endregion

        /// <summary>
        /// Save object Claim 
        /// </summary>
        /// <param name="obj">Object Id</param>
        /// <param name="dbManager">dbManager</param>
        /// <param name="forceSave">Type of save</param>
        /// <returns>Claim</returns>
        protected override Claim SaveOperation(Claim obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var claimClone = obj.Clone();
            dbManager.BeginDomainTransaction();

            // сохраняем Task
            if (claimClone.Task != null)
            {
                _taskDataMapper.Save(claimClone.Task, dbManager);
                claimClone.TaskId = claimClone.Task.Id;
            }

            //Сначала сохраняем заявителя, т.к. есть ссылка на него из Claim
            claimClone.Declarer = _personDataMapper.Save(claimClone.Declarer, dbManager);

            // сохраняем заявление 
            claimClone.DeclarerId = claimClone.Declarer.Id;
            dbManager.SaveDomainObject(claimClone);

            // родственники
            SaveOperationClaimDeclarerRelatives(claimClone, dbManager);

            // удаление основания учета которое указал заявитель из списка
            if (obj.DeclarerBaseReg != null && obj.DeclarerBaseReg.BaseRegItems != null && obj.DeclarerBaseReg.BaseRegItems.DelItems != null)
            {
                foreach (var cdbr in obj.DeclarerBaseReg.BaseRegItems.DelItems.Cast<DeclarerBaseRegItem>())
                {
                    cdbr.MarkDeleted();
                    dbManager.SaveDomainObject(cdbr);
                }
            }
            
            // основания учета которые указал заявитель
            if (claimClone.DeclarerBaseReg != null)
            {
                claimClone.DeclarerBaseReg.ClaimId = claimClone.Id;
                claimClone.DeclarerBaseReg =_declarerRegBaseMapper.Save(claimClone.DeclarerBaseReg, dbManager);
            }

            // регистрация
            SaveOperationClaimReg(claimClone, dbManager);

            // информация о внеочередном предоставлении жилья
            SaveOperationClaimPriv(claimClone, dbManager);

            // снятия с регистрации
            SaveOperationClaimDeReg(claimClone, dbManager);
         
            // история изменения статуса заявки
            foreach (var statusHist in claimClone.ClaimStatusHist)
            {
                statusHist.ClaimId = claimClone.Id;
                dbManager.SaveDomainObject(statusHist);
            }

            dbManager.CommitDomainTransaction();
            claimClone.AcceptChanges();
            return claimClone;
        }

        protected override void FillAssociationsOperation(Claim obj, IDomainDbManager dbManager)
        {
            obj.Declarer = dbManager.RetrieveDomainObject<Person>(obj.DeclarerId);
        }
    }
}
