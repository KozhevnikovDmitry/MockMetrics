using System;
using Common.BL.Validation;
using GU.MZ.BL.Validation;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Validation
{
    /// <summary>
    /// Тесты на методы класса LicenseValidator
    /// </summary>
    [TestFixture]
    public class LicenseObjectValidatorTests
    {
        private LicenseObjectValidator _licenseObjectValidator;

        #region TestData

        private IDomainValidator<Address> _addressValidator;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _addressValidator = Mock.Of<IDomainValidator<Address>>(t => t.Validate(It.IsAny<Address>()) == new ValidationErrorInfo());
            _licenseObjectValidator = new LicenseObjectValidator(_addressValidator);
            _licenseObjectValidator.AllowSinglePropertyValidate = true;
        }

        /// <summary>
        /// Тест на валидацию поля Name
        /// </summary>
        [Test]
        public void NameValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "Name");
            licenseObject.Name = "100500";
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "Name");

            // Assert
            Assert.AreEqual(negResult, "Поле наименование должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля GrantOrderRegNumber
        /// </summary>
        [Test]
        public void GrantOrderRegNumberValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "GrantOrderRegNumber");
            licenseObject.GrantOrderRegNumber = "100500";
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "GrantOrderRegNumber");

            // Assert
            Assert.AreEqual(negResult, "Поле номер решения о предоставлении лицензии должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля GrantOrderStamp
        /// </summary>
        [Test]
        public void GrantOrderStampValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            licenseObject.GrantOrderStamp = null;
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "GrantOrderStamp");
            licenseObject.GrantOrderStamp = DateTime.Today;
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "GrantOrderStamp");

            // Assert
            Assert.AreEqual(negResult, "Поле дата принятия решения о предоставлении лицензии должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля LicenseObjectStatusId
        /// </summary>
        [Test]
        public void ObjectStatusValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            licenseObject.LicenseObjectStatusId = 0;
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "LicenseObjectStatusId");
            licenseObject.LicenseObjectStatusId = 1;
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "LicenseObjectStatusId");

            // Assert
            Assert.AreEqual(negResult, "Поле статус объекта должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля Address
        /// </summary>
        [Test]
        public void AddressValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            licenseObject.Address = null;
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "Address");
            licenseObject.Address = Address.CreateInstance();
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "Address");

            // Assert
            Assert.AreEqual(negResult, "Поле адрес должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля Address
        /// </summary>
        [Test]
        public void AddressDeepValidationTest()
        {
            // Arrange
            var valErrInfo = new ValidationErrorInfo();
            valErrInfo.AddError("Test");
            _addressValidator = Mock.Of<IDomainValidator<Address>>(t => t.Validate(It.IsAny<Address>()) == valErrInfo);
            _licenseObjectValidator = new LicenseObjectValidator(_addressValidator);
            _licenseObjectValidator.AllowSinglePropertyValidate = true;
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            licenseObject.Address = Address.CreateInstance();
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "Address");

            // Assert
            Assert.AreEqual(negResult, "Поле адрес должно быть заполнено корректными данными");
        }

        /// <summary>
        /// Тест на валидацию поля Список поддеятельностей
        /// </summary>
        [Test]
        public void ObjectSubactivityListValidationTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();

            // Act
            var negResult = _licenseObjectValidator.ValidateProperty(licenseObject, "ObjectSubactivityList");
            licenseObject.ObjectSubactivityList.Add(ObjectSubactivity.CreateInstance());
            var posResult = _licenseObjectValidator.ValidateProperty(licenseObject, "ObjectSubactivityList");

            // Assert
            Assert.AreEqual(negResult, "Должна быть выбрана хотя бы одна лицензируемая деятельность");
            Assert.Null(posResult);
        }
    }
}
