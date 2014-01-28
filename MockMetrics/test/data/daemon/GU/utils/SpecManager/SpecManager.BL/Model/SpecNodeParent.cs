using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SpecManager.BL.Model
{
    public abstract class SpecNodeParent
    {
        protected SpecNodeParent()
        {
            this.ChildSpecNodes = new List<SpecNode>();
        }

        [XmlIgnore]
        public abstract List<SpecNode> ChildSpecNodes { get; set; }

        public SpecNode AddChild(SpecNodeType specNodeType)
        {
            return AddChild(ChildSpecNodes.Count, specNodeType);
        }

        public SpecNode AddChild(int index, SpecNodeType specNodeType)
        {
            var childNode = this.CreateChild(specNodeType);
            this.ChildSpecNodes.Insert(index, childNode);
            return childNode;
        }

        protected virtual SpecNode CreateChild(SpecNodeType specNodeType)
        {
            return new SpecNode
            {
                SpecNodeType = specNodeType,
                Name = this.NameForChild(specNodeType)
            };
        }

        private string NameForChild(SpecNodeType specNodeType)
        {
            string name = specNodeType.GetDescription();
            int countSimilar = this.ChildSpecNodes.Count(t => t.SpecNodeType == specNodeType);
            if (countSimilar > 0)
            {
                name = string.Format("{0}({1})", name, countSimilar);
            }

            return name;
        }
    }
}