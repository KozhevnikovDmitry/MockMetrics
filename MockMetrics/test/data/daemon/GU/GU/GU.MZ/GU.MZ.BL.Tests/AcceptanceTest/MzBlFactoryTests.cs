using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using GU.BL;
using GU.DataModel;
using GU.MZ.BL.DomainLogic;
using GU.MZ.BL.Modules;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class MzBlFactoryTests
    {
        public MzTestBlFactory MzTestBlFactory { get; set; }

        [SetUp]
        public void Setup()
        {
            var dalInit = new DataAccessLayerInitializer();
            var conf = new PostgreConfiguration("Postgre", "Server={0};Port={1};Database={2};User Id={3};Password={4};", "172.25.253.154", 5432, "gosus") { User = "test_mz_lic", Password = "test" };
            dalInit.Initialize(conf, new Dictionary<Assembly, IDomainObjectInitializer> 
                        {
                            {typeof(Task).Assembly, new GuDomainObjectInitializer("test_mz_lic")}, 
                            {typeof(License).Assembly, new GumzDomainObjectInitializer("test_mz_lic")} 
                        });
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MzBlModule(Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"])));

            var container = builder.Build();
            builder = new ContainerBuilder();
            builder.RegisterInstance(container);
            builder.Update(container);

            MzTestBlFactory = new MzTestBlFactory(container);
        }

        [Test]
        public void GetDictionaryManagerTest()
        {
            // Act
            var dictMan = MzTestBlFactory.GetDictionaryManager() as GumzDictionaryManager;

            // Assert
            Assert.IsNotNull(dictMan.GetDb);
        }

        [Test]
        public void OrderSaveTest()
        {
            // Arrange
            var order = StandartOrder.CreateInstance();
            order.RegNumber = "100500";
            order.FileScenarioStepId = 1;
            order.OrderOptionId = 1;

            var detail = StandartOrderDetail.CreateInstance();
            detail.SubjectId = "500100";
            order.DetailList.Add(detail);

            // Act
            var saved = MzTestBlFactory.ResolveDataMapper<StandartOrder>().Save(order);

            // Assert
            Assert.IsNotNull(MzTestBlFactory.ResolveDataMapper<StandartOrder>().Retrieve(saved.Id));
        }
    }
}