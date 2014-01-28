using System;
using System.Reflection;
using BLToolkit.EditableObjects;

using Common.BL;
using Common.DA;
using Common.DA.ProviderConfiguration;
using GU.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using NUnit.Framework;

namespace GU.HQ.BL.Test.AcceptanceTest
{
    [TestFixture]
    public class GenerateDBTest
    {
        #region TestData
        /// <summary>
        /// Сборка GU.MZ.BL
        /// </summary>
        private readonly Assembly _hqbl = Assembly.UnsafeLoadFrom("GU.HQ.BL.dll");

        /// <summary>
        /// Сборка GU.BL
        /// </summary>
        private readonly Assembly _gubl = Assembly.UnsafeLoadFrom("GU.BL.dll");


        /// <summary>
        /// Сборка GU.MZ.DataModel
        /// </summary>
        private readonly Assembly _hqmodel = Assembly.UnsafeLoadFrom("GU.HQ.DataModel.dll");

        /// <summary>
        /// Сборка GU.DataModel
        /// </summary>
        private readonly Assembly _gumodel = Assembly.UnsafeLoadFrom("GU.DataModel.dll");


        /// <summary>
        /// Менеджер кэша справочников предметной обалсти "Работа с заявлениями"
        /// </summary>
        private HqDictionaryManager _hqDictionaryManager;

        /// <summary>
        /// Менеджер кэша справочников предметной обалсти "Работа с заявлениями"
        /// </summary>
        private GuDictionaryManager _guDictionaryManager;

        private DomainLogicContainer _domainLogicContainer;

        #endregion DataTest

        /// <summary>
        /// Setup before each test 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var dalInit = new DataAccessLayerInitializer();
            var conf = new PostgreConfiguration("Postgre", "Server={0};Port={1};Database={2};User Id={3};Password={4};", "172.16.0.181", 5432, "gosus") 
            { User = "test_mun", Password = "test" };
            
            dalInit.Initialize(conf, this._gumodel, new GuDomainObjectInitializer("volkov"));
            dalInit.Initialize(conf, this._hqmodel, new HqDomainObjectInitializer("volkov"));

            using (var db = new GuDbManager())
            {
                _guDictionaryManager = new GuDictionaryManager(db, 18);
                _hqDictionaryManager = new HqDictionaryManager(db);
            }
            
            _hqDictionaryManager.Merge(_guDictionaryManager);
            this._domainLogicContainer = new DomainLogicContainer(new[] { this._hqbl, this._gubl }, _hqDictionaryManager);
        }

        #region Arrange
        
        /// <summary>
        /// Подготавливает Заявку для теста
        /// </summary>
        /// <returns>Заявка для теста</returns>
        public  Claim ArrangeClaim()
        {
            var claim = Claim.CreateInstance();

            // статус заявления
            claim.CurrentStatusTypeId = ClaimStatusType.DataCheck;

            // заявитель
            claim.Declarer = ArragePerson("Орешников", "Эльнур", "Харитонович", "Иванов Иван Иванович", Sex.Male, DateTime.Now, 
                null, 
                new EditableList<PersonDoc>{ArragePersonDoc(1, "08 00", "649575", DateTime.Now.AddYears(-8), "УВД Ленинского р-она, г. Красноярска", "242-04", "")}, 
                new EditableList<PersonAddress> { 
                        ArragePersonAddress(AddressType.Registration, DateTime.Now, DateTime.Now.AddYears(5), DateTime.Now.AddDays(-50), 
                            ArrageAddress("660035","г.Красноярск", "г. Ленина", "45", "2а", "3", 
                                    ArrageAddressDesc(3,2, 54 ,47,"",HouseTypeComfort.Comfortable, HouseTypePrivate.HousePrivate )))
                },
                null); 
            
            // район
            claim.AgencyId = 18;

            // Дата подачи заявления
            claim.ClaimDate = DateTime.Now;

            //примечание
            claim.Note = "Тестовое заявление. Оно отсутствует в тасках.";

            claim.ClaimCategories = new EditableList<ClaimCategory>{ ArrageClaimCatagory(30), ArrageClaimCatagory(33) };

            return claim;
        }

        /// <summary>
        /// объект Person
        /// </summary>
        /// <returns></returns>
        private Person ArragePerson(string sName,string name,string patronymic,string fioCurrent,Sex sex,DateTime birthDate, 
                                      PersonContactInfo personContactInfo, EditableList<PersonDoc> documents,
                                      EditableList<PersonAddress> personAddresses, PersonDisability disability)
        {
            var person = Person.CreateInstance();

            person.Sname = sName;
            person.Name = name;
            person.Patronymic = patronymic;
            person.FioCurrent = fioCurrent;
            person.Sex = sex;
            person.BirthDate = birthDate;

            person.ContactInfo = personContactInfo;
            person.Documents = documents;
            person.Addresses = personAddresses;
            person.Disability = disability;

            return person;
        }

        /// <summary>
        /// объект PersonDoc
        /// </summary>
        /// <returns></returns>
        private PersonDoc ArragePersonDoc(int documentTypeId, string seria, string num, DateTime dateDoc, string org, string orgCode, string note)
        {
            var personDoc = PersonDoc.CreateInstance();

            personDoc.DocumentTypeId = documentTypeId;
            personDoc.Seria = seria;
            personDoc.Num = num;
            personDoc.DateDoc = dateDoc;
            personDoc.Org = org;
            personDoc.OrgCode = orgCode;
            personDoc.Note = note;

            return personDoc;
        }

        /// <summary>
        /// объект PersonDoc
        /// </summary>
        /// <returns></returns>
        private PersonAddress ArragePersonAddress(AddressType addressType, DateTime dateStart, DateTime dateEnd, DateTime dateReg, Address address )
        {
            var personAddress = PersonAddress.CreateInstance();

            personAddress.AddressTypeId = addressType;
            personAddress.DateStart = dateStart;
            personAddress.DateEnd = dateEnd;
            personAddress.DateReg = dateReg;

            personAddress.Address = address;

            return personAddress;
        }

        /// <summary>
        /// объект Address
        /// </summary>
        /// <returns></returns>
        private Address ArrageAddress(string postIndex, string city, string street, string houseNum, string korpNum, string kvNum, AddressDesc addressDesc)
        {
            var address = Address.CreateInstance();

            address.PostIndex = postIndex;
            address.City = city;
            address.Street = street;
            address.HouseNum = houseNum;
            address.KorpNum = korpNum;
            address.KvNum = kvNum;
            
            address.AddressDesc = addressDesc;

            return address;
        }

        /// <summary>
        /// Объект описание жилья
        /// </summary>
        /// <param name="floor">этаж</param>
        /// <param name="roomCount">число комнат</param>
        /// <param name="areaGeneral">общая площадь</param>
        /// <param name="areaLiving">жилая площадь</param>
        /// <param name="houseDoc">основание проживание в жилом помещении</param>
        /// <param name="houseTypeComfort">тип комфортабельности</param>
        /// <param name="houseTypePrivate">тип приватности</param>
        /// <returns></returns>
        private AddressDesc ArrageAddressDesc(int floor, int roomCount, Decimal areaGeneral, Decimal areaLiving, 
                                                string houseDoc, HouseTypeComfort houseTypeComfort, HouseTypePrivate houseTypePrivate)
        {
            var addressDesc = AddressDesc.CreateInstance();

            addressDesc.Floor = floor;
            addressDesc.RoomCount = roomCount;
            addressDesc.AreaGenegal = areaGeneral;
            addressDesc.AreaLiving = areaLiving;
            addressDesc.HouseDoc = houseDoc;
            addressDesc.HouseComfort = houseTypeComfort;
            addressDesc.HousePrivate = houseTypePrivate;

            return addressDesc;
        }


        /// <summary>
        /// Категории учета по которым подается заявление
        /// </summary>
        /// <returns></returns>
        private ClaimCategory ArrageClaimCatagory(int categoryId)
        {
            var claimCategory = ClaimCategory.CreateInstance();

            claimCategory.CategoryTypeId = categoryId;

            return claimCategory;
        }


        #endregion

        [Test]
        public void CreateClaim()
        {
            Claim c = ArrangeClaim();

            Claim claimSave = this._domainLogicContainer.ResolveDataMapper<Claim>().Save(c);

            //типа проверка
            Assert.NotNull(claimSave);
            Assert.IsNotNull(claimSave.Id);
            Assert.IsNotNull(claimSave.Declarer.Id);
        }

        /// <summary>
        /// Тест на заполнение ассоциаций сущности Claim
        /// </summary>
        [Test]
        public void ClaimFillAssociationsTest()
        {
            // Arrange
            var claimMapper = this._domainLogicContainer.ResolveDataMapper<Claim>();
            var newClaim = claimMapper.Save(ArrangeClaim());
            var reincClaim = new HqDbManager().RetrieveDomainObject<Claim>(newClaim.Id);

            // Act
            claimMapper.FillAssociations(reincClaim);

            // Assert
            Assert.NotNull(reincClaim.Declarer);
        }
    }
}