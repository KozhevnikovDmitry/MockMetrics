//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using Common.BL;
//using Common.BL.DataMapping;
//using Common.DA;
//using Common.DA.Interface;
//using Common.Types.Exceptions;

//namespace GU.MZ.BL.DomainLogic.Entity
//{
//    public interface IEntityManager
//    {
//        T Get<T>(object id, INotifyPropertyChanged client, LoadLevel loadLevel = LoadLevel.Poor) where T : IdentityDomainObject<T>, IPersistentObject;

//        T Save<T>(object id) where T : IdentityDomainObject<T>, IPersistentObject;

//        void Release<T>(T entity, INotifyPropertyChanged client) where T : IdentityDomainObject<T>, IPersistentObject;

//        event Action<IPersistentObject> PropertyChanged;

//        event Action<IPersistentObject> Reloaded;
//    }

//    public class EntityManager : IEntityManager
//    {
//        private readonly IDomainLogicContainer _domainLogicContainer;
//        private readonly Func<IDomainDbManager> _dbFactory;

//        private HashSet<EntityWrapper> _entities;

//        public EntityManager(IDomainLogicContainer domainLogicContainer, Func<IDomainDbManager> dbFactory)
//        {
//            _domainLogicContainer = domainLogicContainer;
//            _dbFactory = dbFactory;
//            _entities = new HashSet<EntityWrapper>();
//        }

//        public T Get<T>(object id, INotifyPropertyChanged client, LoadLevel loadLevel = LoadLevel.Show) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            try
//            {
//                var entity = GetLoaded<T>(id, client, loadLevel);

//                if (entity != null)
//                {
//                    return entity;
//                }

//                var newEntity = GetFormRepository<T>(id, loadLevel);
//                var wrapper = new EntityWrapper(newEntity)
//                {
//                    LoadLevel = loadLevel
//                };
//                wrapper.Clients.Add(client);
//                _entities.Add(wrapper);

//                return newEntity;

//            }
//            catch (Exception ex)
//            {
//                throw new BLLException("EntityManaget fails to get entity", ex);
//            }
//        }

//        public T Save<T>(T entity, LoadLevel loadLevel = LoadLevel.Full) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            try
//            {
//                using (var db = _dbFactory())
//                {
//                    if (loadLevel == LoadLevel.Poor)
//                    {
//                        db.SaveDomainObject(entity);
//                        return entity;
//                    }
//                    else
//                    {
                        
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                throw new BLLException("EntityManaget fails to get entity", ex);
//            }
//        }

//        public void Release<T>(T entity, INotifyPropertyChanged client) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            throw new NotImplementedException();
//        }

//        private T GetLoaded<T>(object id, INotifyPropertyChanged client, LoadLevel loadLevel) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            var entity = _entities.Select(t => t.Entity).OfType<T>().SingleOrDefault(t => t.GetKeyValue().Equals(id));

//            if (entity == null)
//            {
//                return entity;
//            }

//            var wrapper = _entities.Single(t => t.Entity.Equals(entity));

//            if (wrapper.LoadLevel < loadLevel)
//            {
//                UpLevel(entity, loadLevel);
//            }

//            return entity;
//        }

//        private T GetFormRepository<T>(object id, LoadLevel loadLevel) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            using (var db = _dbFactory())
//            {
//                switch (loadLevel)
//                {
//                    case LoadLevel.Poor:
//                        {
//                            return db.RetrieveDomainObject<T>(id);
//                        }
//                    case LoadLevel.Show:
//                        {
//                            var entity = db.RetrieveDomainObject<T>(id);
//                            Mapper<T>().FillAssociations(entity, db);
//                            return entity;
//                        }
//                    case LoadLevel.Full:
//                        {
//                            return Mapper<T>().Retrieve(id, db);
//                        }
//                }
//            }
//        }

//        private void UpLevel<T>(T entity, LoadLevel loadLevel) where T : IdentityDomainObject<T>, IPersistentObject
//        {
//            IdentityDomainObject<T> bigger = null;
//            using (var db = _dbFactory())
//            {
//                switch (loadLevel)
//                {
//                    case LoadLevel.Show:
//                        {
//                            bigger = db.RetrieveDomainObject<T>(entity.Id);
//                            Mapper<T>().FillAssociations(entity, db);
//                            break;
//                        }
//                    case LoadLevel.Full:
//                        {
//                            bigger = Mapper<T>().Retrieve(entity.Id, db);
//                            break;
//                        }
//                }
//            }

//            bigger.CopyTo(entity);
//        }

//        private IDomainDataMapper<T> Mapper<T>() where T : IPersistentObject
//        {
//            return _domainLogicContainer.ResolveDataMapper<T>();
//        }

//        public event Action<IPersistentObject> PropertyChanged;
//        public event Action<IPersistentObject> Reloaded;
//    }

//    public class EntityWrapper
//    {
//        public IPersistentObject Entity { get; private set; }

//        public EntityWrapper(IPersistentObject entity)
//        {
//            Entity = entity;
//            if (entity == null) throw new ArgumentNullException("entity");
//            Clients = new HashSet<INotifyPropertyChanged>();
//        }

//        public LoadLevel LoadLevel { get; set; }

//        public HashSet<INotifyPropertyChanged> Clients { get; private set; }
//    }

//    public enum LoadLevel
//    {
//        Poor = 1,
//        Show = 2,
//        Full = 3
//    }
//}
