using Common.BL.Authentification;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Authentification
{
    /// <summary>
    /// Тесты на методы класса Authentificator
    /// </summary>
    [TestFixture(Ignore = true)]
    public class AuthentificatorTests
    {
        /// <summary>
        /// Тестируемый объект
        /// </summary>
        private Authentificator _authentificator;

        #region TestData
        
        /// <summary>
        /// Мок тестера подключений
        /// </summary>
        private Mock<IConfigurationTester> _mockTester;

        /// <summary>
        /// Мок конфигурации
        /// </summary>
        private Mock<IProviderConfiguration> _mockConfig;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IProviderConfiguration>();
            _mockTester = new Mock<IConfigurationTester>();
        }

        /// <summary>
        /// Тест проверяющий проталкивание логина и пароля в процедуру проверки соединения
        /// </summary>
        [Test]
        public void CorrectUsernameAndLoginAssignmentTest()
        {
            // Arrange
            _mockTester.Setup(t => t.FullTestConfiguration(It.IsAny<IProviderConfiguration>())).Returns(new ConfigurationTestResult { IsSuccessful = true });
            _authentificator = new Authentificator(_mockConfig.Object, _mockTester.Object);

            // Act
            _authentificator.AuthentificateUser("username", "password");

            // Assert
            _mockConfig.VerifySet(t => t.User = "username", Times.Once());
            _mockConfig.VerifySet(t => t.Password = "password", Times.Once());
            _mockTester.Verify(t => t.FullTestConfiguration(_mockConfig.Object), Times.Once());
        }

        /// <summary>
        /// Тест на удачную попытку аутентификации
        /// </summary>
        [Test]
        public void CorrectAuthentificationTest()
        {
            // Arrange
            _mockTester.Setup(t => t.FullTestConfiguration(It.IsAny<IProviderConfiguration>())).Returns(new ConfigurationTestResult {IsSuccessful = true});
            _authentificator = new Authentificator(_mockConfig.Object, _mockTester.Object);

            // Assert
            Assert.That(_authentificator.AuthentificateUser("username", "password"));
            Assert.IsNullOrEmpty(_authentificator.ErrorMessage);

        }

        /// <summary>
        /// Тест на неудачную попытку аутентификации
        /// </summary>
        [Test]
        public void FailedAuthentificationTest()
        {
            // Arrange
            _mockTester.Setup(t => t.FullTestConfiguration(It.IsAny<IProviderConfiguration>()))
                       .Returns(
                           new ConfigurationTestResult
                               {
                                   IsSuccessful = false,
                                   ErrorMessage = "Неверный логин или пароль"
                               });
            _authentificator = new Authentificator(_mockConfig.Object, _mockTester.Object);

            // Assert
            Assert.False(_authentificator.AuthentificateUser("username", "password"));
            Assert.AreEqual(_authentificator.ErrorMessage, "Неверный логин или пароль");
        }
    }
}
