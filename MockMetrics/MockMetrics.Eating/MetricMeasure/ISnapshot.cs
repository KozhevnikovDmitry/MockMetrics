using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }

        void AddVariable(ICSharpDeclaration variable, Metrics metrics);

        void AddOperand(ICSharpTreeNode operand, Metrics metrics);

        void AddCall(IInvocationExpression invocation, Metrics metrics);
        void AddCall(ITypeofExpression invocation, Metrics metrics);

        void AddFakeOption(ICSharpExpression option, FakeOption fakeOption);

        void AddLabel(ILabelStatement labelStatement);

        IEnumerable<ICSharpTreeNode> TargetCalls { get; }
        IEnumerable<ICSharpTreeNode> Targets { get; }
        IEnumerable<ICSharpTreeNode> Stubs { get; }
        IEnumerable<ICSharpTreeNode> Results { get; }
        IEnumerable<ICSharpTreeNode> Mocks { get; }
        IEnumerable<ICSharpTreeNode> Asserts { get; }

        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);
        Metrics GetVarMetrics(IVariableDeclaration variable);
        Metrics GetVarMetrics(IParameter paramter);
    }
}