using GU.MZ.BL.Validation;
using GU.MZ.DataModel;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Validation
{
    /// <summary>
    /// Тесты на методы класса AddressValidator
    /// </summary>
    [TestFixture]
    public class AddressValidatorTests
    {
        /// <summary>
        /// Тестируемый валидатор
        /// </summary>
        private AddressValidator _addressValidator;

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _addressValidator = new AddressValidator();
            _addressValidator.AllowSinglePropertyValidate = true;
        }

        /// <summary>
        /// Тест на валидацию индекса
        /// </summary>
        [Test]
        public void ZipValidationTest()
        {
            // Arrange
            var address = Address.CreateInstance();

            // Act
            var negResult1 = _addressValidator.ValidateProperty(address, "Zip");
            address.Zip = "testtest";
            var negResult2 = _addressValidator.ValidateProperty(address, "Zip");
            address.Zip = "222222";
            var posResult = _addressValidator.ValidateProperty(address, "Zip");

            // Assert
            Assert.AreEqual(negResult1, "Поле индекс должно быть заполнено");
            Assert.AreEqual(negResult2, "Поле индекс должно быть заполнено корректно : 888888");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля Город
        /// </summary>
        [Test]
        public void CityValidationTest()
        {
            // Arrange
            var address = Address.CreateInstance();

            // Act
            var negResult = _addressValidator.ValidateProperty(address, "City");
            address.City = "Test";
            var posResult = _addressValidator.ValidateProperty(address, "City");

            // Assert
            Assert.AreEqual(negResult, "Поле Город должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля Улица
        /// </summary>
        [Test]
        public void StreetValidationTest()
        {
            // Arrange
            var address = Address.CreateInstance();

            // Act
            var negResult = _addressValidator.ValidateProperty(address, "Street");
            address.Street = "Test";
            var posResult = _addressValidator.ValidateProperty(address, "Street");

            // Assert
            Assert.AreEqual(negResult, "Поле Улица должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля Дом
        /// </summary>
        [Test]
        public void HouseValidationTest()
        {
            // Arrange
            var address = Address.CreateInstance();

            // Act
            var negResult = _addressValidator.ValidateProperty(address, "House");
            address.House = "Test";
            var posResult = _addressValidator.ValidateProperty(address, "House");

            // Assert
            Assert.AreEqual(negResult, "Поле Дом должно быть заполнено");
            Assert.Null(posResult);
        }
    }
}
