using System;
using System.Collections.Generic;

using SpecManager.BL.Interface;

using System.Linq;

using SpecManager.BL.Model;
using SpecManager.DA.Exceptions;

namespace SpecManager.DA.DataMapping
{
    public class SpecNodeDataMapper : AbstractDataMapper<SpecNode>
    {
        public SpecNodeDataMapper(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        protected override SpecNode RetrieveOperation(int id, IDomainDbManager dbManager)
        {
            var specNode = dbManager.RetrieveDomainObject<SpecNode>(id);
            specNode.ChildSpecNodes = new List<SpecNode>();

            if (specNode.SpecNodeType == SpecNodeType.RefSpec)
            {
                var refSpec = dbManager.GetDomainTable<Spec>().SingleOrDefault(t => t.Id == specNode.RefSpecId);
                if (refSpec == null)
                {
                    throw new DALException(string.Format("Внешняя спека с is {0} не найдена", specNode.RefSpecId));
                }

                specNode.RefSpec = refSpec;
                specNode.RefSpecUri = refSpec.Uri;
            }

            foreach (var childNodeId in dbManager.GetDomainTable<SpecNode>()
                                                 .Where(t => t.ParentSpecNodeId.Value == id)
                                                 .Select(t => t.Id)
                                                 .ToList())
            {
                var childNode = RetrieveOperation(childNodeId, dbManager);
                childNode.ParentSpecNode = specNode;
                childNode.ParentSpecNodeId = specNode.Id;
                specNode.ChildSpecNodes.Add(childNode);
            }

            return specNode;
        }

        protected override SpecNode SaveOperation(SpecNode obj, IDomainDbManager dbManager)
        {
            var id = obj.Id;
            if (obj.Tag == "") obj.Tag = null;
            try
            {
                obj.Id = 0;

                if (obj.SpecNodeType == SpecNodeType.RefSpec)
                {
                    var refSpec = dbManager.GetDomainTable<Spec>().SingleOrDefault(t => t.Uri == obj.RefSpecUri);
                    if (refSpec == null)
                    {
                        throw new DALException(string.Format("Внешняя спека с uri {0} не найдена", obj.RefSpecUri));
                    }

                    obj.RefSpec = refSpec;
                    obj.RefSpecId = refSpec.Id;
                }

                dbManager.SaveDomainObject(obj);


                for (int i = 0; i < obj.ChildSpecNodes.Count; i++)
                {
                    obj.ChildSpecNodes[i].ParentSpecNodeId = obj.Id;
                    obj.ChildSpecNodes[i].ParentSpecNode = obj;
                    obj.ChildSpecNodes[i].SpecId = obj.SpecId;
                    obj.ChildSpecNodes[i].Spec = obj.Spec;
                    obj.ChildSpecNodes[i].Order = i + 1;
                    obj.ChildSpecNodes[i] = this.SaveOperation(obj.ChildSpecNodes[i], dbManager);
                }

                return obj;
            }
            catch (Exception)
            {
                obj.Id = id;
                throw;
            }
        }

        protected override void DeleteOperation(SpecNode obj, IDomainDbManager dbManager)
        {
            foreach (var childSpecNode in obj.ChildSpecNodes)
            {
                DeleteOperation(childSpecNode, dbManager);
            }

            dbManager.DeleteDomainObject(obj);
        }
    }
}
