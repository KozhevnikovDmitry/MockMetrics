﻿using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestFixture]
    public class EducationalTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
        {
            return highlighting is MockMetricInfo;
        }

        [SetUp]
        public void Setup()
        {
            MockMetricsElementProcessor.Results.Clear();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MockMetricsElementProcessor.Results.Clear();
        }

        protected override string RelativeTestDataPath
        {
            get { return @"daemon\Tested"; }
        }

        protected override string SolutionName
        {
            get { return @"Tested.sln"; }
        }

        [TestCase(@"<Tested.Tests>\FooTests\MoqStubPositioningTests.cs")]
        public void MoqStubPositioningTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(MockMetricsElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);
        }

        [TestCase(@"<Tested.Tests>\FooTests\ReferenceTypesTests.cs")]
        public void ReferenceTypesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(MockMetricsElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);
        }

        [TestCase(@"<Tested.Tests>\FooTests\MethodInvocationTypesTests.cs")]
        public void MethodInvocationTypesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(MockMetricsElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);
        }

        [TestCase(@"<Tested.Tests>\FooTests\ItIsFakeOptionTests.cs")]
        public void ItIsFakeOptionTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(MockMetricsElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);
        }

        [TestCase(@"<Tested.Tests>\FooTests\LinqQuerySyntaxTests.cs")]
        public void LinqQuerySyntaxTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(MockMetricsElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);
        }
    }
}
