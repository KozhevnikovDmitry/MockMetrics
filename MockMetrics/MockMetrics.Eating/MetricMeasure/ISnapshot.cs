using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }

        void AddVariable(ICSharpDeclaration varDeclaration, Variable varType);
        void AddVariable(IInvocationExpression varDeclaration, Variable varType);
        void AddVariable(ITypeUsage varDeclaration, Variable varType);
        void AddVariable(IInitializerElement varDeclaration, Variable varType);
        void AddVariable(ICSharpLiteralExpression varDeclaration, Variable varType);
        void AddVariable(IReferenceExpression referenceExpression, Variable varType);
        void AddVariable(IObjectCreationExpression varDeclaration, Variable varType);
        void AddFakeOption(ICSharpExpression option, FakeOption fakeOption);

        List<IMetricVariable> Variables { get; }

        IList<IMetricVariable> Targets { get; }
        IList<IMetricVariable> Stubs { get; }
        IList<IMetricVariable> Mocks { get; }
        IList<IMetricVariable> Librarians { get; }
        IList<IMetricVariable> Services { get; }

        IList<IMetricMockOption> FakeMethods { get; }
        IList<IMetricMockOption> FakeProperties { get; }
        IList<IMetricMockOption> FakeCallbacks { get; }
        IList<IMetricMockOption> FakeExceptions { get; }

        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);

        Variable GetVarMetrics(IVariableDeclaration variable);
        Variable GetVarMetrics(IParameter paramter);
    }
}