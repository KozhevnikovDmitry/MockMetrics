﻿using System;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;

namespace MockMetrics.Fake
{
    public class FakeDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _process;

        public FakeDaemonStageProcess(IDaemonProcess process)
        {
            _process = process;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            // Getting PSI (AST) for the file being highlighted
            var sourceFile = _process.SourceFile;
            var file = sourceFile.GetPsiServices().Files.GetDominantPsiFile<CSharpLanguage>(sourceFile) as ICSharpFile;
            if (file == null)
                return;

            // Running visitor against the PSI
            var elementProcessor = new FakesElementProcessor(_process);
            file.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (_process.InterruptFlag)
                throw new ProcessCancelledException();

            // Commit the result into document
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