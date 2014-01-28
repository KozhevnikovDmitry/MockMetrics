using System;

namespace GU.BL.Reporting.Data
{
    public class TaskDataItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? ParentId { get; set; }
        public int Order { get; set; }

        public TaskDataItem(Guid id, Guid? parentId, string name, string value)
        {
            Name = name;
            Value = value;
            Id = id;
            ParentId = parentId;
        }
    }
}
