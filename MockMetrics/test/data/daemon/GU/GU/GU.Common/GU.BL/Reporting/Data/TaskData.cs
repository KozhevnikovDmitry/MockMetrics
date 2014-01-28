using System;
using System.Collections.Generic;

namespace GU.BL.Reporting.Data
{
    public class TaskData
    {
        public string Agency { get; set; }
        public string Service { get; set; }
        public string Customer { get; set; }
        public string CustomerContacts { get; set; }
        public int TaskId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }

        public string Username { get; set; }

        public List<TaskDataItem> Items { get; set; }
    }
}
