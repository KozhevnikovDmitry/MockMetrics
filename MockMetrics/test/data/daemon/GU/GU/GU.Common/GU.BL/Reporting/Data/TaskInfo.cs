using System;

namespace GU.BL.Reporting.Data
{
    public class TaskInfo
    {
        public string Agency { get; set; }

        public string Service { get; set; }

        public string Customer { get; set; }

        public int TaskId { get; set; }

        public DateTime CreateDate { get; set; }

        public string AuthCode { get; set; }
    }
}
