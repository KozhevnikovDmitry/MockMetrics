using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSti
{
    public class Task
    {
        public string Number { get; set; }
        public Service Service { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }
        public ServiceGroup ServiceGroup { get; set; }
    }

    public class ServiceGroup
    {
        public string Name { get; set; }
    }

}
