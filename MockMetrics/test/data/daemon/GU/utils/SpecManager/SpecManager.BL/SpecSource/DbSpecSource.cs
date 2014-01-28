using System;

using SpecManager.BL.Exceptions;
using SpecManager.BL.Interface;
using SpecManager.BL.Model;

using System.Linq;

using BLToolkit.Data.Linq;

namespace SpecManager.BL.SpecSource
{
    public class DbSpecSource : ISpecSource
    {
        private int _id;

        private readonly string _uri;

        private readonly IDomainDataMapper<Spec> _specDataMapper;

        private readonly IDbFactory _dbFactory;

        public DbSpecSource(int id, IDomainDataMapper<Spec> specDataMapper, IDbFactory dbFactory)
        {
            _id = id;
            _specDataMapper = specDataMapper;
            _dbFactory = dbFactory;
        }

        public DbSpecSource(string uri, IDomainDataMapper<Spec> specDataMapper, IDbFactory dbFactory)
            : this(0, specDataMapper, dbFactory)
        {
            _uri = uri;
        }

        public Spec Get()
        {
            try
            {
                var id = GetSpecId();

                return _specDataMapper.Retrieve(id);
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при получении Spec из БД", ex);
            }
        }

        private int GetSpecId()
        {
            if (_id != 0)
            {
                return _id;
            }

            using (var db = _dbFactory.GetDbManager())
            {
                try
                {
                    _id = db.GetDomainTable<Spec>().Where(t => t.Uri == _uri).Select(t => t.Id).Single();
                    return _id;
                }
                catch (InvalidOperationException ex)
                {
                    throw new BLLException(string.Format("Spec с Uri=[{0}] в базе не найдена", _uri), ex);
                }
            }
        }

        public Spec Save(Spec spec)
        {
            try
            {
                using (var db = _dbFactory.GetDbManager())
                {
                    db.BeginDomainTransaction();
                    try
                    {
                        var specDependencies = spec.GetDependencies(db);

                        if (specDependencies.HasDependencies)
                        {
                            DropDependencies(specDependencies, db);
                        }

                        spec.Id = specDependencies.OverridenSpecId ?? 0;

                        var savedSpec = _specDataMapper.Save(spec);
                        _id = savedSpec.Id;

                        db.CommitDomainTransaction();

                        return savedSpec;
                    }
                    catch (GUException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        db.RollbackDomainTransaction();
                        throw new BLLException("", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при сохранении Spec в БД", ex);
            }
        }

        private void DropDependencies(SpecDependencies specDependencies, IDomainDbManager dbManager)
        {
            foreach (var task in specDependencies.Tasks)
            {
                dbManager.Execute("gu_mig.del_task", task.Id);
            }

            if (specDependencies.OverridenSpecId.HasValue)
                dbManager.GetDomainTable<SpecNode>().Delete(t => t.SpecId == specDependencies.OverridenSpecId.Value);
        }

        private string ConnectionString()
        {
            string result = string.Empty;

            this._dbFactory.ConnectionString.Split(';')
                 .Where(t => !t.ToLower().Contains("password"))
                 .ToList()
                 .ForEach(t => result += t + ";");

            return result;
        }

        public string Name
        {
            get
            {
                return string.Format("db=[{0}]; id=[{1}]", ConnectionString(), _id);
            }
        }

        public PreSaveWarning PreSave(Spec spec)
        {
            try
            {
                using (var db = _dbFactory.GetDbManager())
                {
                    var specDependencies = spec.GetDependencies(db);

                    return new PreSaveWarning(specDependencies, spec.Validate())
                               {
                                   Title = "Предупреждение перед сохранением в базу данных: найдены зависимые сущности"
                               };
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при проверке условий сохранения в базу данных", ex);
            }
        }
    }
}
