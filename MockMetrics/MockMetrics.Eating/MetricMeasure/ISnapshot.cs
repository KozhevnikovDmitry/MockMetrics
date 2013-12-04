using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }

        void AddVariable(ICSharpDeclaration variable, Metrics metrics);

        void AddOperand(IInvocationExpression operand, Metrics metrics);
        void AddOperand(IReferenceExpression operand, Metrics metrics);
        void AddOperand(ITypeUsage operand, Metrics metrics);
        void AddOperand(IInitializerElement operand, Metrics metrics);
        void AddOperand(ICSharpLiteralExpression operand, Metrics metrics);

        void AddCall(IInvocationExpression invocation, Metrics metrics);
        void AddCall(ITypeofExpression invocation, Metrics metrics);

        void AddFakeOption(ICSharpExpression option, FakeOption fakeOption);

        void AddLabel(ILabelStatement labelStatement);

        IList<IMetricCall> TargetCalls { get; }
        IList<IMetricNode> Targets { get; }
        IList<IMetricNode> Stubs { get; }
        IList<IMetricNode> Results { get; }
        IList<IMetricNode> Mocks { get; }
        IList<IMetricCall> Asserts { get; }

        IList<IMetricOperand > Variables { get; }
        IList<IMetricOperand> Constans { get; }
        IList<IMetricOperand> Operands { get; }
        IList<IMetricCall> Calls { get; }
        IList<IMetricMockOption> FakeOptions { get; }

        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);
        Metrics GetVarMetrics(IVariableDeclaration variable);
        Metrics GetVarMetrics(IParameter paramter);
    }
}