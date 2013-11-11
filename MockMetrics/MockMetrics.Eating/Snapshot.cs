using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }
        List<ICSharpTreeNode> TargetCalls { get; set; }
        List<ICSharpTreeNode> Targets { get; set; }
        List<ICSharpTreeNode> Stubs { get; set; }
        List<ICSharpTreeNode> Mocks { get; set; }
        List<ICSharpTreeNode> Asserts { get; set; }
        void AddTreeNode(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode);
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
        }

        public IMethodDeclaration UnitTest { get; private set; }

        public List<ICSharpTreeNode> TargetCalls { get; set; }

        public List<ICSharpTreeNode> Targets { get; set; }

        public List<ICSharpTreeNode> Stubs { get; set; }

        public List<ICSharpTreeNode> Mocks { get; set; }

        public List<ICSharpTreeNode> Asserts { get; set; }

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
    }
}