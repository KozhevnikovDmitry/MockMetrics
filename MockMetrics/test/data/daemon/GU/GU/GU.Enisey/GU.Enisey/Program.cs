using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using GU.Enisey.BL;

namespace GU.Enisey
{
    class Program
    {
        public const string WINDOWS_SERVICE_NAME = "EniseyWindowsServiceTest";

        static void Main(string[] args)
        {
            if (IsServiceMode())
            {
                ServiceBase.Run(new ServiceBase[] { new EniseyWindowsService() });
            }
            else
            {
                EniseyFacade.Initialize();
                AppQuiryPortTypeLogic.SetStateList();

                Console.WriteLine("press any key...");
                System.Console.ReadKey();
            }
        }

        public static bool IsServiceMode()
        {
            return !Environment.UserInteractive;
        }
    }
}
