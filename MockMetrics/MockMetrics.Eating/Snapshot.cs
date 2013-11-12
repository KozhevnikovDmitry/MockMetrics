using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;

namespace MockMetrics.Eating
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }
        List<ICSharpTreeNode> TargetCalls { get; }
        List<ICSharpTreeNode> Targets { get; }
        List<ICSharpTreeNode> Stubs { get; }
        List<ICSharpTreeNode> Mocks { get; }
        List<ICSharpTreeNode> Asserts { get; }
        List<IReference> Variables { get; }
        void AddTreeNode(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode);
        void AddVariable(IReference reference);
    }

    public class Snapshot : ISnapshot
    {
        public Snapshot(IMethodDeclaration unitTest)
        {
            UnitTest = unitTest;
            Targets = new List<ICSharpTreeNode>();
            Stubs = new List<ICSharpTreeNode>();
            Mocks = new List<ICSharpTreeNode>();
            Asserts = new List<ICSharpTreeNode>();
            TargetCalls = new List<ICSharpTreeNode>();
            Variables = new List<IReference>();
        }

        public IMethodDeclaration UnitTest { get; private set; }

        public List<ICSharpTreeNode> TargetCalls { get; private set; }

        public List<ICSharpTreeNode> Targets { get; private set; }

        public List<ICSharpTreeNode> Stubs { get; private set; }

        public List<ICSharpTreeNode> Mocks { get; private set; }

        public List<ICSharpTreeNode> Asserts { get; private set; }

        public List<IReference> Variables { get; private set; }

        public void AddTreeNode(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode)
        {
            switch (expressionKind)
            {
                case ExpressionKind.Stub:
                    {
                        Stubs.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.Mock:
                    {
                        Mocks.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.Target:
                    {
                        Targets.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.TargetCall:
                    {
                        TargetCalls.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.Assert:
                    {
                        Asserts.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.None:
                    {
                        break;
                    }
            }
        }

        public void AddVariable(IReference reference)
        {
            Variables.Add(reference);
        }
    }
}