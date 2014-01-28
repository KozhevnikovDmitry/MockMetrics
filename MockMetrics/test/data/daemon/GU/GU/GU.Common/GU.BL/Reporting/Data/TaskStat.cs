using System;
using System.Collections.Generic;

namespace GU.BL.Reporting.Data
{
    public class TaskStat
    {
        public string Header { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public string Username { get; set; }
        public DateTime Stamp { get; set; }

        public string GroupNameCaption { get; set; }
        public string GroupName2Caption { get; set; }
        public string GroupName3Caption { get; set; }
        public string NameCaption { get; set; }
        
        public List<TaskStatItem> TaskStatItems { get; set; }

        public class TaskStatItem
        {
            public string GroupName { get; set; }
            public string GroupName2 { get; set; }
            public string GroupName3 { get; set; }
            public string Name { get; set; }

            public int? GroupId { get; set; }
            public int? GroupId2 { get; set; }
            public int? GroupId3 { get; set; }
            public int? Id { get; set; }

            public int? GroupOrder { get; set; }
            public int? GroupOrder2 { get; set; }
            public int? GroupOrder3 { get; set; }
            public int? Order { get; set; }
            
            public int Count { get; set; }
        }
    }
}
