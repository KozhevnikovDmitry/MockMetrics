using GU.MZ.BL.Tests.AcceptanceTest.Supervision;
using NUnit.Framework;

namespace GU.MZ.UI.Tests
{
    [TestFixture]
    public class ArrangeDataForHandyTests : SupervisionFixture
    {
        [TestCase(3, "11111111111", "1111111111111111")]
        public void ActionTest(int serviceId, string inn, string ogrn)
        {
            // Arrange
            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);

            // Assert
            ActSaving();
        }
    }
}