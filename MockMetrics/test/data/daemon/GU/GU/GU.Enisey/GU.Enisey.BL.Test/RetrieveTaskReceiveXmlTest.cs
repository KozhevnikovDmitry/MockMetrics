using System;
using NUnit.Framework;

namespace GU.Enisey.BL.Test
{
    public class RetrieveTaskReceiveXmlTest : IntegrationFixture
    {
        [Test]
        public void RetrieveTaskReceiveXml()
        {
            Utils.RetrieveTaskReceiveXml("task_receive", DateTime.Now.AddDays(-7));
        }
    }
}
