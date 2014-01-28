using GU.DataModel;
using GU.MZ.BL.DomainLogic.MzTask;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.MzTask
{
    [TestFixture]
    public class MzTaskWrapperTests
    {
        [TestCase(1, Result = LicenseServiceType.Drug)]
        [TestCase(2, Result = LicenseServiceType.Drug)]
        [TestCase(3, Result = LicenseServiceType.Drug)]
        [TestCase(4, Result = LicenseServiceType.Drug)]
        [TestCase(5, Result = LicenseServiceType.Drug)]
        [TestCase(6, Result = LicenseServiceType.Med)]
        [TestCase(7, Result = LicenseServiceType.Med)]
        [TestCase(8, Result = LicenseServiceType.Med)]
        [TestCase(9, Result = LicenseServiceType.Med)]
        [TestCase(10, Result = LicenseServiceType.Med)]
        [TestCase(11, Result = LicenseServiceType.Farm)]
        [TestCase(12, Result = LicenseServiceType.Farm)]
        [TestCase(13, Result = LicenseServiceType.Farm)]
        [TestCase(14, Result = LicenseServiceType.Farm)]
        [TestCase(15, Result = LicenseServiceType.Farm)]
        public LicenseServiceType LicenseServiceTypeTest(int serviceId)
        {
            // Arrange
            var task = Mock.Of<Task>(t => t.ServiceId == serviceId);
            var mzTask = new MzTaskWrapper(task);
            
            // Assert
            return mzTask.LicenseServiceType;
        }

        [TestCase(1, Result = LicenseActionType.New)]
        [TestCase(2, Result = LicenseActionType.Renewal)]
        [TestCase(3, Result = LicenseActionType.Stop)]
        [TestCase(4, Result = LicenseActionType.Duplicate)]
        [TestCase(5, Result = LicenseActionType.Copy)]
        [TestCase(6, Result = LicenseActionType.New)]
        [TestCase(7, Result = LicenseActionType.Renewal)]
        [TestCase(8, Result = LicenseActionType.Stop)]
        [TestCase(9, Result = LicenseActionType.Duplicate)]
        [TestCase(10, Result = LicenseActionType.Copy)]
        [TestCase(11, Result = LicenseActionType.New)]
        [TestCase(12, Result = LicenseActionType.Renewal)]
        [TestCase(13, Result = LicenseActionType.Stop)]
        [TestCase(14, Result = LicenseActionType.Duplicate)]
        [TestCase(15, Result = LicenseActionType.Copy)]
        public LicenseActionType LicenseActionTypeTest(int serviceId)
        {
            // Arrange
            var task = Mock.Of<Task>(t => t.ServiceId == serviceId);
            var mzTask = new MzTaskWrapper(task);

            // Assert
            return mzTask.LicenseActionType;
        }
    }
}