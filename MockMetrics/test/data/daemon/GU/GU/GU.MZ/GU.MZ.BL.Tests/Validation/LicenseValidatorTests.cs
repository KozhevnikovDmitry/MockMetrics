using System;
using System.Collections.Generic;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using Common.BL.Validation;
using GU.MZ.BL.Validation;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Validation
{
    /// <summary>
    /// Тесты на методы класса LicenseValidator
    /// </summary>
    [TestFixture]
    public class LicenseValidatorTests
    {
        private LicenseValidator _licenseValidator;

        #region TestData

        private IDomainValidator<LicenseObject> _licenseObjectValidator;

        private IDomainValidator<LicenseRequisites> _licenseRequisitesValidator;

        private IDictionaryManager _dictionaryManager;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _licenseObjectValidator = Mock.Of<IDomainValidator<LicenseObject>>();
            _licenseRequisitesValidator = Mock.Of<IDomainValidator<LicenseRequisites>>();
            _dictionaryManager =
                Mock.Of<IDictionaryManager>(
                    t =>
                    t.GetDictionary<LicensedSubactivity>()
                    == new List<LicensedSubactivity>
                           {
                               Mock.Of<LicensedSubactivity>(
                                   s => s.LicensedActivityId == 1 && s.Id == 1),
                               Mock.Of<LicensedSubactivity>(
                                   s => s.LicensedActivityId == 2 && s.Id == 2)
                           });

            _licenseValidator = BuildLicenseValidator();
        }

        private LicenseValidator BuildLicenseValidator()
        {
            var licenseValidator = new LicenseValidator(_licenseObjectValidator, _licenseRequisitesValidator, _dictionaryManager);
            licenseValidator.AllowSinglePropertyValidate = true;
            return licenseValidator;
        }

        /// <summary>
        /// Тест на валидацию поля RegNumber
        /// </summary>
        [Test]
        public void RegNumberValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            var negResult = _licenseValidator.ValidateProperty(license, "RegNumber");
            license.RegNumber = "100500";
            var posResult = _licenseValidator.ValidateProperty(license, "RegNumber");

            // Assert
            Assert.AreEqual(negResult, "Поле регистрационный номер должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля GrantDate
        /// </summary>
        [Test]
        public void GrantDateValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            license.GrantDate = null;
            var negResult = _licenseValidator.ValidateProperty(license, "GrantDate");
            license.GrantDate = DateTime.Today;
            var posResult = _licenseValidator.ValidateProperty(license, "GrantDate");

            // Assert
            Assert.AreEqual(negResult, "Поле дата предоставления должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля BlankNumber
        /// </summary>
        [Test]
        public void BlankNumberValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            var negResult = _licenseValidator.ValidateProperty(license, "BlankNumber");
            license.BlankNumber = "100500";
            var posResult = _licenseValidator.ValidateProperty(license, "BlankNumber");

            // Assert
            Assert.AreEqual(negResult, "Поле номер бланка должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля GrantOrderRegNumber
        /// </summary>
        [Test]
        public void GrantOrderRegNumberValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            var negResult = _licenseValidator.ValidateProperty(license, "GrantOrderRegNumber");
            license.GrantOrderRegNumber = "100500";
            var posResult = _licenseValidator.ValidateProperty(license, "GrantOrderRegNumber");

            // Assert
            Assert.AreEqual(negResult, "Поле регистрационный номер решения о предоставлении лицензии должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля GrantOrderStamp
        /// </summary>
        [Test]
        public void GrantOrderStampValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            license.GrantOrderStamp = null;
            var negResult = _licenseValidator.ValidateProperty(license, "GrantOrderStamp");
            license.GrantOrderStamp = DateTime.Today;
            var posResult = _licenseValidator.ValidateProperty(license, "GrantOrderStamp");

            // Assert
            Assert.AreEqual(negResult, "Поле дата решения о предоставлении лицензии должно быть заполнено");
            Assert.Null(posResult);
        }
        
        /// <summary>
        /// Тест на валидацию поля LicensedActivityId
        /// </summary>
        [Test]
        public void HolderLicensedActivityValidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            license.LicensedActivityId = 0;
            var negResult = _licenseValidator.ValidateProperty(license, "LicensedActivityId");
            license.LicensedActivityId = 1;
            var posResult = _licenseValidator.ValidateProperty(license, "LicensedActivityId");

            // Assert
            Assert.AreEqual(negResult, "Поле лицензируемая деятельность должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля LicenseStatusId
        /// </summary>
        [Test]
        public void HolderLicenseStatusvalidationTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            license.CurrentStatus = 0;
            var negResult = _licenseValidator.ValidateProperty(license, "CurrentStatus");
            license.CurrentStatus = LicenseStatusType.Active;
            var posResult = _licenseValidator.ValidateProperty(license, "CurrentStatus");

            // Assert
            Assert.AreEqual(negResult, "Поле статус лицензии должно быть заполнено");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на валидацию поля LicenseHolderId
        /// </summary>
        [Test]
        public void HolderLicenseHolderValiadtionTest()
        {
            // Arrange
            var license = License.CreateInstance();

            // Act
            var negResult = _licenseValidator.ValidateProperty(license, "LicenseDossierId");
            license.LicenseDossierId = 1;
            var posResult = _licenseValidator.ValidateProperty(license, "LicenseDossierId");

            // Assert
            Assert.AreEqual(negResult, "C лицензией должно быть ассоциировано лицензионное дело");
            Assert.Null(posResult);
        }

        /// <summary>
        /// Тест на проверку соотвествия между видом деятельности лицензии и объектов с номенклатурой
        /// </summary>
        [Test]
        public void LicensedActivityAndObjectSubactivitiesConsistentTest()
        {
            // Arrange
            var license = License.CreateInstance();
            license.LicensedActivityId = 1;

            // Act
            var posResult1 = _licenseValidator.ValidateProperty(license, "LicensedActivityId");
            license.LicenseObjectList 
                = new EditableList<LicenseObject>
                    {
                        Mock.Of<LicenseObject>(t => t.ObjectSubactivityList 
                            == new EditableList<ObjectSubactivity>
                                    {
                                        Mock.Of<ObjectSubactivity>(o => o.LicensedSubactivityId == 1)
                                    })
                    };

            var posResult2 = _licenseValidator.ValidateProperty(license, "LicensedActivityId");

            license.LicenseObjectList
                = new EditableList<LicenseObject>
                    {
                        Mock.Of<LicenseObject>(t => t.ObjectSubactivityList 
                            == new EditableList<ObjectSubactivity>
                                    {
                                        Mock.Of<ObjectSubactivity>(o => o.LicensedSubactivityId == 2)
                                    })
                    };

            var negResult = _licenseValidator.ValidateProperty(license, "LicensedActivityId");


            // Assert
            Assert.AreEqual(negResult, @"Вид лицензируемой деятельности в лицензии не соответствует набору работ\услуг в объектах с номенклатурой.");
            Assert.Null(posResult1);
            Assert.Null(posResult2);
        }


        // TODO Перенести в тесты валидатора реквизитов лицензии
        ///// <summary>
        ///// Тест на валидацию поля HolderFullName
        ///// </summary>
        //[Test]
        //public void HolderFullNameValidationTest()
        //{
        //    // Arrange
        //    var license = License.CreateInstance();

        //    // Act
        //    var negResult = _licenseValidator.ValidateProperty(license, "HolderFullName");
        //    license.HolderFullName = "100500";
        //    var posResult = _licenseValidator.ValidateProperty(license, "HolderFullName");

        //    // Assert
        //    Assert.AreEqual(negResult, "Поле имя лицензиата должно быть заполнено");
        //    Assert.Null(posResult);
        //}


        ///// <summary>
        ///// Тест на валидацию поля LicensiarHeadName
        ///// </summary>
        //[Test]
        //public void LicensiarHeadNameValidationTest()
        //{
        //    // Arrange
        //    var license = License.CreateInstance();

        //    // Act
        //    var negResult = _licenseValidator.ValidateProperty(license, "LicensiarHeadName");
        //    license.LicensiarHeadName = "100500";
        //    var posResult = _licenseValidator.ValidateProperty(license, "LicensiarHeadName");

        //    // Assert
        //    Assert.AreEqual(negResult, "Поле наименование должности главы лицензирующей организации должно быть заполнено");
        //    Assert.Null(posResult);
        //}


        ///// <summary>
        ///// Тест на валидацию поля LicensiarHeadPosition
        ///// </summary>
        //[Test]
        //public void LicensiarHeadPositionValidationTest()
        //{
        //    // Arrange
        //    var license = License.CreateInstance();

        //    // Act
        //    var negResult = _licenseValidator.ValidateProperty(license, "LicensiarHeadPosition");
        //    license.LicensiarHeadPosition = "100500";
        //    var posResult = _licenseValidator.ValidateProperty(license, "LicensiarHeadPosition");

        //    // Assert
        //    Assert.AreEqual(negResult, "Поле ФИО главы лицензирующей организации должно быть заполнено");
        //    Assert.Null(posResult);
        //}
    }
}
