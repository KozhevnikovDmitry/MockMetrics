using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class AcceptanceTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
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
            get { return @"daemon\PostGrad"; }
        }

        protected override string SolutionName
        {
            get { return @"PostGrad.sln"; }
        }

        /// <summary>
        /// Test#0
        /// </summary>
        [TestCase(@"<PostGrad.BL.Tests>\AddInList\Before\LinkagerTests.cs")]
        public void PrimitiveVariableTests(string testName)
        {
            try
            {
                DoTestFiles(testName);
                Console.WriteLine(Enumerable.ToArray(FakesElementProcessor.Results.Values).Count());
            }
            catch (EatingException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
