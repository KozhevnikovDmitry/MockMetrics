using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
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
        void AddVariable(ICSharpDeclaration variable, Metrics metrics);

        void AddOperand(ICSharpTreeNode operand, Scope scope, Aim aim, VarType varType);
        void AddOperand(ICSharpTreeNode operand, Metrics metrics);

        void AddCall(IInvocationExpression invocation, Metrics metrics);
        void AddCall(ITypeofExpression invocation, Metrics metrics);

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
        Metrics GetVarMetrics(IVariableDeclaration variable);
        Metrics GetVarMetrics(IParameter paramter);
        void Except(IVariableDeclaration variableDeclaration);
    }

    public class Snapshot : ISnapshot
    {
        public Snapshot(IMethodDeclaration unitTest)
        {
            
        }

        public IMethodDeclaration UnitTest { get; private set; }
        public void AddVariable(ICSharpDeclaration variable, Scope scope, Aim aim, VarType varType)
        {
            
        }

        // TODO : seek variable in snaphot an add metrics
        public void AddVariable(ICSharpDeclaration variable, Metrics metrics)
        {
            
        }

        public void AddOperand(ICSharpTreeNode operand, Scope scope, Aim aim, VarType varType)
        {
            
        }

        public void AddOperand(ICSharpTreeNode operand, Metrics metrics)
        {
            
        }

        public void AddCall(IInvocationExpression invocation, Metrics metrics)
        {
            
        }

        public void AddCall(ITypeofExpression invocation, Metrics metrics)
        {
           
        }

        public void AddFakeOption(ICSharpExpression opton, FakeOption fakeOption)
        {
            
        }

        public void AddLabel(ILabelStatement labelStatement)
        {
            
        }

        public IEnumerable<ICSharpTreeNode> TargetCalls { get; private set; }
        public IEnumerable<ICSharpTreeNode> Targets { get; private set; }
        public IEnumerable<ICSharpTreeNode> Stubs { get; private set; }
        public IEnumerable<ICSharpTreeNode> Results { get; private set; }
        public IEnumerable<ICSharpTreeNode> Mocks { get; private set; }
        public IEnumerable<ICSharpTreeNode> Asserts { get; private set; }
        public bool IsInTestScope(string projectName)
        {
            return true;
        }

        public bool IsInTestProject(string projectName)
        {
            return true;
        }

        public Metrics GetVarMetrics(IVariableDeclaration localVariable)
        {
           return Metrics.Create();
        }

        public Metrics GetVarMetrics(IParameter paramter)
        {
            return Metrics.Create();
        }

        public void Except(IVariableDeclaration variableDeclaration)
        {
            
        }
    }
}
