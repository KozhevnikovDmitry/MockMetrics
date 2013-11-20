using System;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;

namespace MockMetrics
{
    public class MockMetricsDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _process;
        private readonly int _arrangeAmount;

        public MockMetricsDaemonStageProcess(IDaemonProcess process, int arrangeAmount)
        {
            _process = process;
            _arrangeAmount = arrangeAmount;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            var sourceFile = _process.SourceFile;
            var file = sourceFile.GetPsiServices().Files.GetDominantPsiFile<CSharpLanguage>(sourceFile) as ICSharpFile;
            if (file == null)
                return;

            var elementProcessor = new MockMetricsElementProcessor(_process);
            file.ProcessDescendants(elementProcessor);

            if (_process.InterruptFlag)
                throw new ProcessCancelledException();

            committer(new DaemonStageResult(elementProcessor.Highlightings));
        }

        public IDaemonProcess DaemonProcess
        {
            get
            {
                return _process;
            }

        }
    }
}