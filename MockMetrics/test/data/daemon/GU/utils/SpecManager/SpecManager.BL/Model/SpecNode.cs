using System.Collections.Generic;
using System.Xml.Serialization;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Exceptions;
using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.spec_node")]
    public class SpecNode : SpecNodeParent, IDomainObject
    {
        public SpecNode()
        {
            ChildSpecNodes = new List<SpecNode>();
            MinOccurs = 1;
            MaxOccurs = 1;
        }

        [PrimaryKey, Identity]
        [SequenceName("gu.spec_node_spec_node_id_seq")]
        [MapField("spec_node_id")]
        [XmlIgnore]
        public int Id { get; set; }

        [MapField("spec_id")]
        [XmlIgnore]
        public int SpecId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecId", OtherKey = "Id", CanBeNull = false)]
        [XmlIgnore]
        public Spec Spec { get; set; }

        [MapField("parent_spec_node_id")]
        [XmlIgnore]
        public virtual int? ParentSpecNodeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ParentSpecNodeId", OtherKey = "Id", CanBeNull = false)]
        [XmlIgnore]
        public virtual SpecNode ParentSpecNode { get; set; }

        [MapField("name")]
        public string Name { get; set; }

        [MapField("tag")]
        public string Tag { get; set; }

        [MapField("spec_node_type_id")]
        [XmlAttribute]
        public  SpecNodeType SpecNodeType { get; set; }

        [MapField("attr_data_type_id")]
        public AttrDataType? AttrDataType { get; set; }

        [MapField("min_occurs")]
        public int MinOccurs { get; set; }

        [MapField("max_occurs")]
        public int? MaxOccurs { get; set; }

        [MapField("ref_spec_id")]
        [XmlIgnore]
        public int? RefSpecId { get; set; }
        
        [NoInstance]
        [Association(ThisKey = "RefSpecId", OtherKey = "Id", CanBeNull = false)]
        [XmlIgnore]
        public Spec RefSpec { get; set; }

        [MapIgnore]
        public string RefSpecUri { get; set; }

        [MapField("format_regexp")]
        public string FormatRegexp { get; set; }

        [MapField("format_description")]
        public string FormatDescription { get; set; }

        [MapField("dict_id")]
        public int? DictId { get; set; }

        [NoInstance]
        [Association(ThisKey = "DictId", OtherKey = "Id", CanBeNull = true)]
        [XmlIgnore]
        public Dict Dict { get; set; }

        [MapField("is_editable_dict")]
        public bool? IsEditableDict { get; set; }

        [MapField("sort_order")]
        [XmlIgnore]
        public int? Order { get; set; }

        [MapField("note")]
        public string Note { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentSpecNodeId")]
        [XmlArray("Children")]
        [XmlArrayItem("SpecNode")]
        public override List<SpecNode> ChildSpecNodes { get; set; }

        public void ShiftUp()
        {
            CheckParent();
            if (this.ParentSpecNode != null)
            {
                ShiftChildUp(this.ParentSpecNode.ChildSpecNodes, this);
            }
            else
            {
                ShiftChildUp(this.Spec.ChildSpecNodes, this);
            }
        }

        public void ShiftDown()
        {
            CheckParent();
            if (this.ParentSpecNode != null)
            {
                ShiftChildDown(this.ParentSpecNode.ChildSpecNodes, this);
            }
            else
            {
                ShiftChildDown(this.Spec.ChildSpecNodes, this);
            }
        }

        private void ShiftChildUp(List<SpecNode> specNodeList, SpecNode specNode)
        {
            if (specNodeList != null && specNodeList.Count != 0)
            {
                int index = specNodeList.IndexOf(specNode);

                if (index == -1)
                {
                    throw new BLLException("Элемент не принадлжит списку дочерних");
                }

                if (index > 0)
                {
                    specNodeList.RemoveAt(index);
                    specNodeList.Insert(index - 1, specNode);
                }
            }
            else
            {
                throw new BLLException("Список дочерних элементов не заполнен");
            }
        }

        private void ShiftChildDown(List<SpecNode> specNodeList, SpecNode specNode)
        {
            if (specNodeList != null && specNodeList.Count != 0)
            {
                int index = specNodeList.IndexOf(specNode);

                if (index == -1)
                {
                    throw new BLLException("Элемент не принадлжит списку дочерних");
                }

                if (index < specNodeList.Count - 1)
                {
                    specNodeList.RemoveAt(index);
                    specNodeList.Insert(index + 1, specNode);
                }
            }
            else
            {
                throw new BLLException("Список дочерних элементов не заполнен");
            }
        }
        
        public void Remove()
        {
            CheckParent();
            if (this.ParentSpecNode != null)
            {
                Remove(this.ParentSpecNode.ChildSpecNodes, this);
            }
            else
            {
                Remove(this.Spec.ChildSpecNodes, this);
            }
        }

        private void Remove(List<SpecNode> specNodeList, SpecNode specNode)
        {
            if (specNodeList != null && specNodeList.Count != 0)
            {
                if (!specNodeList.Contains(specNode))
                {
                    throw new BLLException("Элемент не принадлжит списку дочерних");
                }

                specNodeList.Remove(specNode);
            }
            else
            {
                throw new BLLException("Список дочерних элементов не заполнен");
            }
        }

        public SpecNode Copy()
        {
            CheckParent();
            var clone = this.Clone();
            if (this.ParentSpecNode != null)
            {
                var index = ParentSpecNode.ChildSpecNodes.IndexOf(this);
                this.ParentSpecNode.ChildSpecNodes.Insert(index, clone);
            }
            else
            {
                var index = Spec.ChildSpecNodes.IndexOf(this);
                this.Spec.ChildSpecNodes.Insert(index + 1, clone);
            }

            return clone;
        }

        public virtual SpecNode Clone()
        {
            var clone = new SpecNode
                            {
                                SpecId = this.SpecId,
                                Spec = this.Spec,
                                ParentSpecNodeId = this.ParentSpecNodeId,
                                ParentSpecNode = this.ParentSpecNode,
                                Name = this.Name,
                                Tag = this.Tag,
                                RefSpecUri = this.RefSpecUri,
                                SpecNodeType = this.SpecNodeType,
                                AttrDataType = this.AttrDataType,
                                MinOccurs = this.MinOccurs,
                                MaxOccurs = this.MaxOccurs,
                                RefSpecId = this.RefSpecId,
                                RefSpec = this.RefSpec,
                                FormatRegexp = this.FormatRegexp,
                                FormatDescription = this.FormatDescription,
                                DictId = this.DictId,
                                Dict = this.Dict,
                                IsEditableDict = this.IsEditableDict,
                                Order = this.Order,
                                Note = this.Note
                            };

            clone.ChildSpecNodes = new List<SpecNode>();

            foreach (var childSpecNode in ChildSpecNodes)
            {
                var childClone = childSpecNode.Clone();
                childClone.ParentSpecNode = clone;
                childClone.ParentSpecNodeId = clone.Id;
                clone.ChildSpecNodes.Add(childClone);
            }

            return clone;
        }

        protected override SpecNode CreateChild(SpecNodeType specNodeType)
        {
            var childNode = base.CreateChild(specNodeType);
            childNode.ParentSpecNode = this;
            childNode.ParentSpecNodeId = this.Id;
            childNode.Spec = this.Spec;
            childNode.SpecId = this.SpecId;
            return childNode;
        }

        public virtual void SetParentness()
        {
            foreach (var childSpecNode in ChildSpecNodes)
            {
                childSpecNode.ParentSpecNode = this;
                childSpecNode.ParentSpecNodeId = this.Id;
                childSpecNode.Spec = this.Spec;
                childSpecNode.SpecId = this.SpecId;
                childSpecNode.SetParentness();
            }
        }

        private void CheckParent()
        {
            if (Spec == null && ParentSpecNode == null)
            {
                throw new BLLException(string.Format("Нет родителя для ноды id=[{0}] name=[{1}]", Id, Name));
            }
        }

        public override string ToString()
        {
            return string.Format(
                "Спек нода id=[{0}], name=[{1}], specNodeType=[{2}]",
                this.Id,
                this.Name,
                this.SpecNodeType.GetDescription());
        }

        public SpecNode AddChild(SpecNode specNode)
        {
            specNode.ParentSpecNode = this;
            specNode.ParentSpecNodeId = this.Id;
            specNode.Spec = this.Spec;
            specNode.SpecId = this.SpecId;
            specNode.SetParentness();
            this.ChildSpecNodes.Add(specNode);
            return specNode;
        }
    }
}
