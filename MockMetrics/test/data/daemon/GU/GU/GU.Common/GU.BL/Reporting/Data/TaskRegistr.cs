using System;
using System.Collections.Generic;

namespace GU.BL.Reporting.Data
{
    public class TaskRegistr
    {
        public string Username { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<TaskRegistrStat> TaskRegistrStatList { get; set; }

        public class TaskRegistrStat
        {
            public string StatusName { get; set; }

            public DateTime Stamp { get; set; }

            public int TaskId { get; set; }
        }
    }
}
