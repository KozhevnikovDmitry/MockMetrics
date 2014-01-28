using System.Linq;
using GU.MZ.BL.Test.AcceptanceTest.TaskData;
using GU.UI.ViewModel.ContentViewModel;
using NUnit.Framework;

namespace GU.MZ.UI.Tests
{
    /// <summary>
    /// Тест на создание VM для контента заявки
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class BuildContentTest : ContentFixture
    {
        /// <summary>
        /// Тест на создание VM для заявления
        /// </summary>
        [TestCase(1)]
        public void BuildContentNodeTest(int serviceId)
        {
            // Arrange
            var task = this.ArrangeTask(serviceId);

            // Act
            var contentNodeVm = new ContentNodeVmBuilder().For(task.Content.RootContentNodes.First()).Build();

            // Assert
            Assert.NotNull(contentNodeVm);
        }
    }
}
