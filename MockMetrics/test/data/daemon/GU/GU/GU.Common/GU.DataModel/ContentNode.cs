using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace GU.DataModel
{
    [TableName("gu.content_node")]
    public abstract class ContentNode : IdentityDomainObject<ContentNode>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.content_node_content_node_id_seq")]
        [MapField("content_node_id")]
        public abstract override int Id { get; set; }

        [MapField("content_id")]
        public abstract int ContentId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ContentId", OtherKey = "Id", CanBeNull = false)]
        public Content Content { get; set; }

        [MapField("parent_content_node_id")]
        public abstract int? ParentContentNodeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ParentContentNodeId", OtherKey = "Id", CanBeNull = false)]
        public ContentNode ParentContentNode { get; set; }

        [MapField("spec_node_id")]
        public abstract int SpecNodeId { get; set; }

        [NoInstance]
        [Association(ThisKey = "SpecNodeId", OtherKey = "Id", CanBeNull = false)]
        public virtual SpecNode SpecNode { get; set; }

        [MapField("node_num")]
        public abstract int? NodeNum { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentContentNodeId")]
        public abstract EditableList<ContentNode> ChildContentNodes { get; set; }

        [MapField("str_value")]
        public abstract string StrValue { get; set; }

        [MapField("num_value")]
        public abstract decimal? NumValue { get; set; }

        [MapField("date_value")]
        public abstract DateTime? DateValue { get; set; }

        [MapField("bool_value")]
        public abstract bool? BoolValue { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        [MapField("blob_value")]
        public abstract byte[] BlobValue { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [MapField("blob_name")]
        public abstract string BlobName { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        [MapField("blob_size")]
        public abstract int BlobSize { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        [MapField("blob_type")]
        public abstract string BlobType { get; set; }

        /// <summary>
        /// Ключ справочника (только для типа данных список)
        /// </summary>
        [MapField("dict_key")]
        public abstract string DictKey { get; set; }

        /// <summary>
        /// Возвращает список дочерних нод, к которому принадлежит данная нода.
        /// </summary>
        [MapIgnore]
        [CloneIgnore]
        public IEnumerable<ContentNode> ContainedNodeList
        {
            get
            {
                if (this.ParentContentNode != null 
                    && ParentContentNode.ChildContentNodes != null 
                    && ParentContentNode.ChildContentNodes.Contains(this))
                {
                    return ParentContentNode.ChildContentNodes;
                }

                if (this.Content != null
                    && Content.RootContentNodes != null
                    && Content.RootContentNodes.Contains(this))
                {
                    return Content.RootContentNodes;
                }

                return null;
            }
        }

        /// <summary>
        /// Возвращает все ноды зависимые от данной
        /// </summary>
        [MapIgnore]
        [CloneIgnore]
        public IEnumerable<ContentNode> Descendants
        {
            get
            {
                if (ChildContentNodes != null)
                {
                    var descendants = this.ChildContentNodes.ToList();

                    foreach (var childContentNode in ChildContentNodes)
                    {
                        descendants.AddRange(childContentNode.Descendants);
                    }

                    return descendants;
                }

                return new EditableList<ContentNode>();
            }
        }
    }
}
