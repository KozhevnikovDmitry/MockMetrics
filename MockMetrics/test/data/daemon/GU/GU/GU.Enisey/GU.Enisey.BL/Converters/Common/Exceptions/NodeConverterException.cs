using System;
using System.Linq;
using GU.DataModel;
using GU.BL.Extensions;

namespace GU.Enisey.BL.Converters.Common.Exceptions
{
    public class NodeConverterException : ConverterException
    {
        public SpecNode SpecNode { get; private set; }
        
        public NodeConverterException(SpecNode specNode)
            : base()
        {
            SpecNode = specNode;
        }

        public NodeConverterException(SpecNode specNode, string message)
            : base(message)
        {
            SpecNode = specNode;
        }

        public NodeConverterException(SpecNode specNode, string message, Exception innerException)
            : base(message, innerException)
        {
            SpecNode = specNode;
        }

        public override string Message
        {
            get
            {
                var specNodePath =
                    String.Join("/",
                                SpecNode.Ancestors(t => t.ParentSpecNode, true)
                                        .Reverse()
                                        .Select(t => t.Tag));
                return string.Format(
                    "{0} (specNode.id={1}, specNode.Path={2}, specNode.Name={3}, spec.Id={4})",
                    base.Message, SpecNode.Id, specNodePath, SpecNode.Name, SpecNode.Spec.Id);
            }
        }
    }
}
