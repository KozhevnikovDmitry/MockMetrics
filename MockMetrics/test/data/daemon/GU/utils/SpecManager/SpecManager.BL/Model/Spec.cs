using System.Collections.Generic;
using System.Xml.Serialization;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using SpecManager.BL.Interface;
using SpecManager.BL.SpecSource;

namespace SpecManager.BL.Model
{
    [TableName("gu.spec")]
    [XmlRoot("Spec")]
    public class Spec : SpecNodeParent, IDomainObject
    {
        public Spec()
        {
            this.DescendantNodes = new List<SpecNode>();
        }

        [PrimaryKey, Identity]
        [SequenceName("gu.spec_spec_id_seq")]
        [MapField("spec_id")]
        [XmlIgnore]
        public int Id { get; set; }

        [MapField("spec_name")]
        public string Name { get; set; }

        /// <summary>
        /// Непосредственные потомки
        /// </summary>
        [Association(ThisKey = "Id", OtherKey = "SpecId")]
        [XmlArray("Children")]
        [XmlArrayItem("SpecNode")]
        public override List<SpecNode> ChildSpecNodes { get; set; }

        /// <summary>
        /// Все подчинённый ноды
        /// </summary>
        [MapIgnore]
        [XmlIgnore]
        public List<SpecNode> DescendantNodes { get; set; }

        [MapField("spec_uri")]
        [XmlAttribute]
        public string Uri { get; set; }

        protected override SpecNode CreateChild(SpecNodeType specNodeType)
        {
            var childNode = base.CreateChild(specNodeType);
            childNode.Spec = this;
            childNode.SpecId = this.Id;
            return childNode;
        }

        internal void SetParentness()
        {
            foreach (var childSpecNode in ChildSpecNodes)
            {
                childSpecNode.Spec = this;
                childSpecNode.SpecId = this.Id;
                childSpecNode.SetParentness();
            }
        }

        public SpecDependencies GetDependencies(IDomainDbManager dbManager)
        {
            return new SpecDependencies(this.Uri, dbManager);
        }

        public SpecValidations Validate()
        {
            var result = new SpecValidations();

            GetSpecNodeValidations(result, ChildSpecNodes);

            return result;
        }

        private void GetSpecNodeValidations(SpecValidations result, List<SpecNode> childSpecNodes)
        {
            foreach (var childSpecNode in childSpecNodes)
            {
                if (string.IsNullOrEmpty(childSpecNode.Tag))
                {
                    result.Messages.Add("Нет тэга: " + childSpecNode);
                }

                if (childSpecNode.ChildSpecNodes != null)
                {
                    GetSpecNodeValidations(result,childSpecNode.ChildSpecNodes);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Спека Uri=[{0}] Name=[{1}], Id=[{2}]", Uri, Name, Id);
        }

        public SpecNode AddChild(SpecNode specNode)
        {
            specNode.ParentSpecNode = null;
            specNode.ParentSpecNodeId = null;
            specNode.Spec = this;
            specNode.SpecId = this.Id;
            specNode.SetParentness();
            this.ChildSpecNodes.Add(specNode);
            return specNode;
        }
    }
}
