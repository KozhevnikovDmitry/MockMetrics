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

        IEnumerable<IMetricCall> TargetCalls { get; }
        IEnumerable<IMetricNode> Targets { get; }
        IEnumerable<IMetricNode> Stubs { get; }
        IEnumerable<IMetricNode> Results { get; }
        IEnumerable<IMetricNode> Mocks { get; }
        IEnumerable<IMetricCall> Asserts { get; }

        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);
        Metrics GetVarMetrics(IVariableDeclaration variable);
        Metrics GetVarMetrics(IParameter paramter);
    }
}