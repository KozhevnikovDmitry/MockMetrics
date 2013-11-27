using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface IMetricNode
    {
        int Depth { get; }
    }

    public interface IMetricNodeVariable : IMetricNode
    {
        Scope Scope { get; }

        Dictionary<Guid, Aim> Aims { get; }

        Dictionary<Guid, VarType> VarTypes { get; }
    }

    public interface IMetricOperand : IMetricNode
    {
        Scope Scope { get; }

        Aim Aim { get; }

        VarType VarType { get; }
    }

    public interface IMetricCall : IMetricNode
    {
        Call Call { get; }
    }

    public interface IMetricMockOption : IMetricNode
    {
        FakeOption FakeOption { get; }
    }

    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }

        void AddVariable(ICSharpDeclaration variable, Scope scope, Aim aim, VarType varType);

        void AddOperand(ICSharpTreeNode operand, Scope scope, Aim aim, VarType varType);

        void AddCall(IInvocationExpression invokation, Call call);

        void AddFakeOption(ICSharpExpression opton, FakeOption fakeOption);

        void AddLabel(ILabelStatement labelStatement);

        IEnumerable<ICSharpTreeNode> TargetCalls { get; }
        IEnumerable<ICSharpTreeNode> Targets { get; }
        IEnumerable<ICSharpTreeNode> Stubs { get; }
        IEnumerable<ICSharpTreeNode> Results { get; }
        IEnumerable<ICSharpTreeNode> Mocks { get; }
        IEnumerable<ICSharpTreeNode> Asserts { get; }

        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);
        VarType GetVarType(IVariableDeclaration localVariable);
        void Except(IVariableDeclaration variableDeclaration);
    }
}
