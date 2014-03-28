using System;
using System.Collections.Generic;
using JetBrains.CommandLine.InspectCode;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.SolutionAnalysis;
using JetBrains.Util;

namespace MockMetrics
{

    public class MockMetricsConsumer : IInspectCodeConsumer
    {
        void IDisposable.Dispose() { }

        public void Consume(IIssue issue)
        {
            int i = 0;
        }
    }


    [SolutionComponent]
    public class MockMetricsFactory : IInspectCodeConsumerFactory
    {
        public IInspectCodeConsumer CreateConsumer(IEnumerable<IProjectModelElement> inspectScope, FileSystemPath outputFile = null)
        {
            return new MockMetricsConsumer();
        }
    }
}
