using System.Collections.Generic;
using System.Linq;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;

using BLToolkit.Data.Linq;

using SpecManager.DA.Exceptions;

namespace SpecManager.DA.DataMapping
{
    internal class SpecDataMapper : AbstractDataMapper<Spec>
    {
        private readonly IDomainDataMapper<Dict> _dictDataMapper;

        private readonly IDomainDataMapper<SpecNode> _specNodeDatamapper;

        public SpecDataMapper(IDbFactory dbFactory, IDomainDataMapper<Dict> dictDataMapper, IDomainDataMapper<SpecNode> specNodeDatamapper)
            : base(dbFactory)
        {
            this._dictDataMapper = dictDataMapper;
            this._specNodeDatamapper = specNodeDatamapper;
        }

        protected override Spec RetrieveOperation(int id, IDomainDbManager dbManager)
        {
            var spec = dbManager.RetrieveDomainObject<Spec>(id);

            if (spec == null)
            {
                throw new DALException(string.Format("Спека с id=[{0}] не найдена", id));
            }

            spec.ChildSpecNodes = new List<SpecNode>();

            foreach (var childNodeId in dbManager.GetDomainTable<SpecNode>()
                                                 .Where(t => t.SpecId == id)
                                                 .Where(t => !t.ParentSpecNodeId.HasValue)
                                                 .Select(t => t.Id)
                                                 .ToList())
            {
                var childNode = _specNodeDatamapper.Retrieve(childNodeId, dbManager);
                childNode.Spec = spec;
                childNode.SpecId = spec.Id;
                spec.ChildSpecNodes.Add(childNode);
            }

            return spec;

        }

        protected override Spec SaveOperation(Spec obj, IDomainDbManager dbManager)
        {
            dbManager.SaveDomainObject(obj);

            for (int i = 0; i < obj.ChildSpecNodes.Count; i++)
            {
                obj.ChildSpecNodes[i].SpecId = obj.Id;
                obj.ChildSpecNodes[i].Order = i + 1;
                obj.ChildSpecNodes[i].ParentSpecNode = null;
                obj.ChildSpecNodes[i].ParentSpecNodeId = null;
                obj.ChildSpecNodes[i] = _specNodeDatamapper.Save(obj.ChildSpecNodes[i], dbManager);
            }

            return obj;
        }

        protected override void DeleteOperation(Spec obj, IDomainDbManager dbManager)
        {
            dbManager.GetDomainTable<SpecNode>().Delete(t => t.SpecId == obj.Id);

            dbManager.DeleteDomainObject(obj);
        }
    }
}
