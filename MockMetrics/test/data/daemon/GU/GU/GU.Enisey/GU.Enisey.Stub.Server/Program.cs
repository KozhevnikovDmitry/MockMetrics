using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace GU.Enisey.Stub.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("StartingService");

            //using (ServiceHost service = new ServiceHost(typeof(AppQuiryServiceStub)))
            //{
            //    service.Open();
            //    System.Console.ReadKey();
            //}
        }
    }
}
