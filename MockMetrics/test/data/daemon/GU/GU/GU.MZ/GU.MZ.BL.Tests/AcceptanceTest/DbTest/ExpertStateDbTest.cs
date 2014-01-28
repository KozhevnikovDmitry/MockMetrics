using GU.MZ.BL.Tests.AcceptanceTest;
using GU.MZ.DataModel.Person;
using NUnit.Framework;

namespace GU.MZ.BL.Test.AcceptanceTest.DbTest
{
    /// <summary>
    /// Тесты на CRUD операции с экспертов
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ExpertStateDbTest : MzAcceptanceTests
    {
        /// <summary>
        /// Тест на сохранение эксперта физического лица
        /// </summary>
        [Test]
        public void SaveIndividualExpertTest()
        {
            // Arrange
            var expert = Expert.CreateInstance();
            expert.ExpertStateType = ExpertStateType.Individual;
            expert.AccreditateDocumentNumber = "АК № 100500";
            var expertState = IndividualExpertState.CreateInstance();
            expertState.Name = "Джигурда";
            expertState.OrganizationName = "ООО Джигурда";
            expertState.Surname = "Мигурда";
            expertState.Position = "Верховный жрец";
            expertState.Patronymic = "Сигурдович";
            expert.ExpertState = expertState;

            // Act
            var savedExpert = MzLogicFactory.ResolveDataMapper<Expert>().Save(expert);

            // Assert
            Assert.NotNull(savedExpert);
            Assert.NotNull(savedExpert.ExpertState);
        }

        /// <summary>
        /// Тест на сохранение эксперта физического лица
        /// </summary>
        [Test]
        public void SaveJuridicalExpertTest()
        {
            // Arrange
            var expert = Expert.CreateInstance();
            expert.ExpertStateType = ExpertStateType.Juridical;
            expert.AccreditateDocumentNumber = "АК № 100500";
            var expertState = JuridicalExpertState.CreateInstance();
            expertState.FullName = "Длинный Джигурда";
            expertState.ShortName = "Краткий Джигурда";
            expertState.FirmName = "Фирменный Джигурда";
            expertState.HeadName = "Джигурда Д.Д.";
            expertState.HeadPositionName = "Верховный жрец";
            expertState.Inn = "100500100500";
            expertState.Ogrn = "500100500100";
            expertState.Address.Zip = "100500";
            expertState.Address.City = "Улан-Джигурда";
            expertState.Address.Street = "50 лет Джигурда";
            expertState.Address.House = "3";
            expert.ExpertState = expertState;

            // Act
            var savedExpert = MzLogicFactory.ResolveDataMapper<Expert>().Save(expert);

            // Assert
            Assert.NotNull(savedExpert);
            Assert.NotNull(savedExpert.ExpertState);
            Assert.NotNull((savedExpert.ExpertState as JuridicalExpertState).Address);
        }
    }
}
