using System;
using System.Linq;
using Autofac;
using Common.DA.Interface;
using GU.DataModel;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.BltoolkitTest
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class GetTableLastTests : MzAcceptanceTests
    {
        [Test]
        public void GetTableLastTest()
        {
            // Arrange
            var getdb = MzLogicFactory.IocContainer.Resolve<Func<IDomainDbManager>>();
            using (var db = getdb())
            {
                // Act
                var requisites = db.GetDomainTable<Task>().Last();
            }

            // Assert
        }
    }
}
