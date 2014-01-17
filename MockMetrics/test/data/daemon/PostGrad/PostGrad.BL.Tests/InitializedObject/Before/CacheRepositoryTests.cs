using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PostGrad.BL.InitializedObject.Before;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;
using PostGrad.Core.DomainModel.Licensing;
using ICacheRepository = PostGrad.BL.InitializedObject.Before.ICacheRepository;

namespace PostGrad.BL.Tests.InitializedObject.Before
{
    [TestFixture]
    public class CacheRepositoryTests
    {
        [Test]
        public void CtorTest()
        {
            // Arrange
            var status = Mock.Of<LicenseStatus>();
            var activity = Mock.Of<LicensedActivity>();
            var serviceResult = Mock.Of<ServiceResult>();
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus> { status }.AsQueryable()
                                                 && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity> { activity }.AsQueryable()
                                                 && t.GetDomainTable<ServiceResult>() == new List<ServiceResult> { serviceResult }.AsQueryable());

            // Act
            var cacheRepository = new CacheRepository(db);

            // Assert
            Assert.NotNull(cacheRepository.MergedRepositories);
            Assert.NotNull(cacheRepository.Caches);
            Assert.AreEqual(cacheRepository.Caches[typeof(LicenseStatus)].Single(), status);
            Assert.AreEqual(cacheRepository.Caches[typeof(LicensedActivity)].Single(), activity);
            Assert.AreEqual(cacheRepository.Caches[typeof(ServiceResult)].Single(), serviceResult);
        }

        [Test]
        public void GetCasheTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                 && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                 && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var statuses = new List<LicenseStatus>().Cast<IDomainObject>().ToList();
            var cacheRepository = new CacheRepository(db);
            cacheRepository.Caches[typeof(LicenseStatus)] = statuses;

            // Act
            var result = cacheRepository.GetCache<LicenseStatus>();

            // Assert
            Assert.AreEqual(result, statuses);
        }

        [Test]
        public void GetCacheFromMergedRepositoryTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var cacheRepository = new CacheRepository(db);
            var legalForms = new List<LegalForm>();
            var mergedRepository = Mock.Of<ICacheRepository>(t => t.GetCache<LegalForm>() == legalForms);
            var failingMergedRepository = Mock.Of<ICacheRepository>();
            Mock.Get(failingMergedRepository).Setup(t => t.GetCache<LegalForm>()).Throws(new DictionaryNotFoundException(typeof(LegalForm)));
            cacheRepository.MergedRepositories.Add(failingMergedRepository);
            cacheRepository.MergedRepositories.Add(mergedRepository);

            // Act
            var result = cacheRepository.GetCache<LegalForm>();

            // Assert
            Assert.AreEqual(result, legalForms);
        }

        [Test]
        public void DictionaryNotFoundExceptionTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var cacheRepository = new CacheRepository(db);
            var failingMergedRepository = Mock.Of<ICacheRepository>();
            Mock.Get(failingMergedRepository).Setup(t => t.GetCache<LegalForm>()).Throws(new DictionaryNotFoundException(typeof(LegalForm)));
            cacheRepository.MergedRepositories.Add(failingMergedRepository);

            // Assert
            Assert.Throws<DictionaryNotFoundException>(() => cacheRepository.GetCache<LegalForm>());
        }

        [Test]
        public void GetCashedItemTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                 && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                 && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var status = Mock.Of<LicenseStatus>(t => t.GetKeyValue() == "1");
            var statuses = new List<LicenseStatus>
            {
                status, 
                Mock.Of<LicenseStatus>()
            }.Cast<IDomainObject>().ToList();

            var cacheRepository = new CacheRepository(db);
            cacheRepository.Caches[typeof(LicenseStatus)] = statuses;

            // Act
            var result = cacheRepository.GetCachedItem<LicenseStatus>(1);

            // Assert
            Assert.AreEqual(result, status);
        }

        [Test]
        public void DictionaryItemNotFoundTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                 && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                 && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var statuses = new List<LicenseStatus>
            {
                Mock.Of<LicenseStatus>()
            }.Cast<IDomainObject>().ToList();

            var cacheRepository = new CacheRepository(db);
            cacheRepository.Caches[typeof(LicenseStatus)] = statuses;

            // Assert
            Assert.Throws<DictionaryItemNotFoundException>(() => cacheRepository.GetCachedItem<LicenseStatus>(1));
        }

        [Test]
        public void MergeTest()
        {
            // Arrange
            var mergedRepository = Mock.Of<ICacheRepository>();
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseStatus>() == new List<LicenseStatus>().AsQueryable()
                                                 && t.GetDomainTable<LicensedActivity>() == new List<LicensedActivity>().AsQueryable()
                                                 && t.GetDomainTable<ServiceResult>() == new List<ServiceResult>().AsQueryable());
            var cacheRepository = new CacheRepository(db);

            // Act
            cacheRepository.Merge(mergedRepository);

            // Assert
            Assert.AreEqual(cacheRepository.MergedRepositories.Single(), mergedRepository);
        }
    }
}
