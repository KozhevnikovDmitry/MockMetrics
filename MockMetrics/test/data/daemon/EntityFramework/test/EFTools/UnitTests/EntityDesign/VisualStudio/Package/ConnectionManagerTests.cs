﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Design.VisualStudio.Package
{
    using EnvDTE;
    using Microsoft.VisualStudio.Data.Core;
    using Microsoft.VisualStudio.DataTools.Interop;
    using Microsoft.VSDesigner.Data.Local;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnitTests.TestHelpers;
    using VSLangProj;
    using Xunit;

    public class ConnectionManagerTests
    {
        [Fact]
        public void InjectEFAttributesIntoConnectionString_adds_App_MARS_for_SqlServer()
        {
            Assert.Equal(
                "integrated security=SSPI;MultipleActiveResultSets=True;App=EntityFramework",
                ConnectionManager.InjectEFAttributesIntoConnectionString("Integrated Security=SSPI", "System.Data.SqlClient"));
        }

        [Fact]
        public void
            InjectEFAttributesIntoConnectionString_does_not_touch_connection_string_if_not_SqlServer()
        {
            const string connectionString = "dummy";
            Assert.Same(
                connectionString,
                ConnectionManager.InjectEFAttributesIntoConnectionString(connectionString, "fakeProvider"));
        }

        [Fact]
        public void
            InjectEFAttributesIntoConnectionString_does_not_app_App_MARS_to_connection_string_if_they_already_exist()
        {
            Assert.Equal(
                "integrated security=SSPI;multipleactiveresultsets=True;app=XYZ", 
                ConnectionManager.InjectEFAttributesIntoConnectionString(
                    "Integrated Security=SSPI;MultipleActiveResultSets=True;App=XYZ", "System.Data.SqlClient"));
        }

        [Fact]
        public void
            InjectEFAttributesIntoConnectionString_does_not_add_App_attribute_to_connection_string_if_Application_Name_attribute_already_exists()
        {
            Assert.Equal(
                "integrated security=SSPI;multipleactiveresultsets=True;application name=XYZ",
                ConnectionManager.InjectEFAttributesIntoConnectionString(
                    "Integrated Security=SSPI;MultipleActiveResultSets=True;Application Name=XYZ", "System.Data.SqlClient"));
        }

        [Fact]
        public void InjectEFAttributesIntoConnectionString_returns_same_connection_string_if_it_is_invalid()
        {
            const string connectionString = "dummy";

            Assert.Same(
                connectionString,
                ConnectionManager.InjectEFAttributesIntoConnectionString(connectionString, "System.Data.SqlClient"));
        }

        [Fact]
        public void GetMetadataFileNamesFromArtifactFileName_creates_metadata_file_names_for_non_null_edmx_ProjectItem()
        {
            var mockDte = new MockDTE(".NETFramework, Version=v4.5", references: new Reference[0]);
            mockDte.SetProjectProperties(new Dictionary<string, object> { { "FullPath", @"D:\Projects\Project\Folder" } });
            var mockParentProjectItem = new Mock<ProjectItem>();
            mockParentProjectItem.Setup(p => p.Collection).Returns(Mock.Of<ProjectItems>());
            mockParentProjectItem.Setup(p => p.Name).Returns("Folder");

            var mockModelProjectItem = new Mock<ProjectItem>();
            var mockCollection = new Mock<ProjectItems>();
            mockCollection.Setup(p => p.Parent).Returns(mockParentProjectItem.Object);
            mockModelProjectItem.Setup(p => p.Collection).Returns(mockCollection.Object);

            var metadataFileNames =
                ConnectionManager.GetMetadataFileNamesFromArtifactFileName(
                mockDte.Project, @"c:\temp\myModel.edmx", mockDte.ServiceProvider, (_, __) => mockModelProjectItem.Object);

            Assert.Equal(@".\Folder\myModel.csdl", metadataFileNames[0]);
            Assert.Equal(@".\Folder\myModel.ssdl", metadataFileNames[1]);
            Assert.Equal(@".\Folder\myModel.msl", metadataFileNames[2]);
        }

        [Fact]
        public void GetMetadataFileNamesFromArtifactFileName_creates_metadata_file_names_for_null_edmx_ProjectItem()
        {
            var mockDte = new MockDTE(".NETFramework, Version=v4.5", references: new Reference[0]);
            mockDte.SetProjectProperties(new Dictionary<string, object> { { "FullPath", @"C:\Projects\Project\Folder" } });
            
            var metadataFileNames =
                ConnectionManager.GetMetadataFileNamesFromArtifactFileName(
                mockDte.Project, @"c:\temp\myModel.edmx", mockDte.ServiceProvider, (_, __) => null);

            Assert.Equal(@".\..\..\..\temp\myModel.csdl", metadataFileNames[0]);
            Assert.Equal(@".\..\..\..\temp\myModel.ssdl", metadataFileNames[1]);
            Assert.Equal(@".\..\..\..\temp\myModel.msl", metadataFileNames[2]);
        }

        [Fact]
        public void TranslateConnectionString_returns_connectionstring_if_converter_service_not_available()
        {
            const string connString = "fakeConnString";

            Assert.Same(
                connString,
                ConnectionManager.TranslateConnectionString(
                    Mock.Of<IServiceProvider>(), Mock.Of<Project>(), "invariantName", connString, true));

            Assert.Same(
                connString,
                ConnectionManager.TranslateConnectionString(
                    Mock.Of<IServiceProvider>(), Mock.Of<Project>(), "invariantName", connString, false));

        }

        [Fact]
        public void TranslateConnectionString_can_translate_designtime_connectionstring_to_runtime_connectionstring()
        {
            const string runtimeConnString = "runtimeConnString";

            var mockConverter = new Mock<IConnectionStringConverterService>();
            mockConverter
                .Setup(c => c.ToRunTime(It.IsAny<Project>(), It.IsAny<string>(), "My.Db"))
                .Returns(runtimeConnString);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IConnectionStringConverterService)))
                .Returns(mockConverter.Object);

            Assert.Same(
                runtimeConnString,
                ConnectionManager.TranslateConnectionString(
                    mockServiceProvider.Object, Mock.Of<Project>(), "My.Db", "designTimeConnString", true));
        }

        [Fact]
        public void TranslateConnectionString_can_translate_runtime_connectionstring_to_designtime_connectionstring()
        {
            const string designTimeConnString = "designTimeConnString";

            var mockConverter = new Mock<IConnectionStringConverterService>();
            mockConverter
                .Setup(c => c.ToDesignTime(It.IsAny<Project>(), It.IsAny<string>(), "My.Db"))
                .Returns(designTimeConnString);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IConnectionStringConverterService)))
                .Returns(mockConverter.Object);

            Assert.Same(
                designTimeConnString,
                ConnectionManager.TranslateConnectionString(
                    mockServiceProvider.Object, Mock.Of<Project>(), "My.Db", "runtimeTimeConnString", false));
        }

        [Fact]
        public void TranslateConnectionString_handles_ConnectionStringConverterServiceException_from_translation()
        {
            var converterException =
                (ConnectionStringConverterServiceException)
                typeof(ConnectionStringConverterServiceException).GetConstructor(
                    BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null)
                    .Invoke(new object[0]);

            var mockConverter = new Mock<IConnectionStringConverterService>();
            mockConverter
                .Setup(c => c.ToDesignTime(It.IsAny<Project>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(converterException);
            mockConverter
                .Setup(c => c.ToRunTime(It.IsAny<Project>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(converterException);

            var mockDataProvider = new Mock<IVsDataProvider>();
            mockDataProvider
                .Setup(p => p.GetProperty("InvariantName"))
                .Returns("My.Db");

            var mockProviderManager = new Mock<IVsDataProviderManager>();
            mockProviderManager
                .Setup(m => m.Providers)
                .Returns(new Dictionary<Guid, IVsDataProvider> { { Guid.Empty, mockDataProvider.Object } });

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IConnectionStringConverterService)))
                .Returns(mockConverter.Object);
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IVsDataProviderManager)))
                .Returns(mockProviderManager.Object);

            Assert.Equal(
                string.Format(Resources.CannotTranslateRuntimeConnectionString, string.Empty, "connectionString"),
                Assert.Throws<ArgumentException>(
                    () => ConnectionManager.TranslateConnectionString(mockServiceProvider.Object,
                        Mock.Of<Project>(), "My.Db", "connectionString", false)).Message);

            Assert.Equal(
                string.Format(Resources.CannotTranslateDesignTimeConnectionString, string.Empty, "connectionString"),
                Assert.Throws<ArgumentException>(
                    () => ConnectionManager.TranslateConnectionString(mockServiceProvider.Object,
                        Mock.Of<Project>(), "My.Db", "connectionString", true)).Message);
        }

        [Fact]
        public void TranslateConnectionString_handles_checks_if_DDEX_provider_installed_when_handling_ConnectionStringConverterServiceException()
        {
            var converterException =
                (ConnectionStringConverterServiceException)
                typeof(ConnectionStringConverterServiceException).GetConstructor(
                    BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null)
                    .Invoke(new object[0]);

            var mockConverter = new Mock<IConnectionStringConverterService>();
            mockConverter
                .Setup(c => c.ToDesignTime(It.IsAny<Project>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(converterException);
            mockConverter
                .Setup(c => c.ToRunTime(It.IsAny<Project>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(converterException);

            var mockProviderManager = new Mock<IVsDataProviderManager>();
            mockProviderManager
                .Setup(m => m.Providers)
                .Returns(new Dictionary<Guid, IVsDataProvider>());

            // this is to ensure that even if the translation of the provider invariant name succeeded 
            // we will use the runtime provider invariant name in the message
            var mockProviderMapper = new Mock<IDTAdoDotNetProviderMapper2>();
            mockProviderMapper
                .Setup(m => m.MapRuntimeInvariantToInvariantName("My.Db", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns("My.Db.DesignTime");

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IConnectionStringConverterService)))
                .Returns(mockConverter.Object);
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IVsDataProviderManager)))
                .Returns(mockProviderManager.Object);
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IDTAdoDotNetProviderMapper)))
                .Returns(mockProviderMapper.Object);

            var ddexNotInstalledMessage = string.Format(Resources.DDEXNotInstalled, "My.Db");

            Assert.Equal(
                string.Format(Resources.CannotTranslateRuntimeConnectionString, ddexNotInstalledMessage, "connectionString"),
                Assert.Throws<ArgumentException>(
                    () => ConnectionManager.TranslateConnectionString(mockServiceProvider.Object,
                        Mock.Of<Project>(), "My.Db", "connectionString", false)).Message);

            mockProviderMapper
                .Verify(
                    m => m.MapRuntimeInvariantToInvariantName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), 
                    Times.Once());

            Assert.Equal(
                string.Format(Resources.CannotTranslateDesignTimeConnectionString, ddexNotInstalledMessage, "connectionString"),
                Assert.Throws<ArgumentException>(
                    () => ConnectionManager.TranslateConnectionString(mockServiceProvider.Object,
                        Mock.Of<Project>(), "My.Db", "connectionString", true)).Message);
        }
    }
}
