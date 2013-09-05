﻿using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics
{
    public class Snapshot
    {
        public Snapshot()
        {
            TargetCandidates = new List<ILocalVariableDeclaration>();
            Stubs = new List<ILocalVariableDeclaration>();
            Mocks = new List<ILocalVariableDeclaration>();
            Asserts = new List<IExpressionStatement>();
        }

        public List<IInvocationExpression> TargetCalls { get; set; }

        public List<ILocalVariableDeclaration> TargetCandidates { get; set; }

        public List<ILocalConstantDeclaration> Constants { get; set; }

        public List<ILocalVariableDeclaration> Stubs { get; set; }

        public List<ILocalVariableDeclaration> Mocks { get; set; }

        public List<IExpressionStatement> Asserts { get; set; }
    }
}