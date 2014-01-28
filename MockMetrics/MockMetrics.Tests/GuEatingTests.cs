﻿using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class GuEatingTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting,
            IContextBoundSettingsStore settingsstore)
        {
            return highlighting is FakeHighlighting;
        }

        [SetUp]
        public void Setup()
        {
            FakesElementProcessor.Results.Clear();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            FakesElementProcessor.Results.Clear();
        }

        protected override string RelativeTestDataPath
        {
            get { return @"D:\GitHub\mm_test\GU\GU\GU.MZ"; }
        }

        protected override string SolutionName
        {
            get { return @"GU.MZ.sln"; }
        }

        [TestCase(@"<GU.MZ.BL.Tests>\DataMapping")]
        public void GU_MZ_BL_DataMapping_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl_data_mapping");
        }

        [TestCase(@"<GU.MZ.BL.Tests>\DomainLogic")]
        public void GU_MZ_BL_DomainLogic_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl_domain_logic");
        }

        [TestCase(@"<GU.MZ.BL.Tests>\Validation")]
        public void GU_MZ_BL_Validation_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl_validation");
        }

        [TestCase(@"<GU.MZ.BL.Tests>\Reporting")]
        public void GU_MZ_BL_Reporting_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl_reporting");
        }

        [TestCase(@"<GU.MZ.BL.Tests>\Common")]
        public void GU_MZ_BL_Common_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl_common");
        }
    }
}
