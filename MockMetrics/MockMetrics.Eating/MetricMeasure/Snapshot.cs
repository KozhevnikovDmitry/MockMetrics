using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MetricMeasure
{
    public class Snapshot : ISnapshot
    {
        private readonly List<IMetricNode> _nodes = new List<IMetricNode>();

        public IMethodDeclaration UnitTest { get; private set; }

        public Snapshot([NotNull] IMethodDeclaration unitTest)
        {
            if (unitTest == null) 
                throw new ArgumentNullException("unitTest");

            UnitTest = unitTest;
            GetTestScope(UnitTest);
        }

        #region Old Metrics

        public IEnumerable<ICSharpTreeNode> TargetCalls
        {
            get
            {
                return Calls.Where(t => t.Call == Call.TargetCall).Select(t => t.Node);
            }
        }

        public IEnumerable<ICSharpTreeNode> Asserts
        {
            get
            {
                return Calls.Where(t => t.Call == Call.Assert).Select(t => t.Node);
            }
        }

        public IEnumerable<ICSharpTreeNode> Targets
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(VarType.Target)).Select(t => t.Node);
            }
        }

        public IEnumerable<ICSharpTreeNode> Stubs
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(VarType.Stub)).Select(t => t.Node);
            }
        }

        public IEnumerable<ICSharpTreeNode> Results
        {
            get
            {
                return Operands.Where(t => t.Aims.Values.Max().Equals(Aim.Result)).Select(t => t.Node);
            }
        }

        public IEnumerable<ICSharpTreeNode> Mocks
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(VarType.Mock)).Select(t => t.Node);
            }
        }
        
        #endregion


        #region Nodes

        public IEnumerable<IMetricOperand> Variables 
        {
            get { return _nodes.OfType<IMetricOperand>().Where(t => t.Operand == Operand.Variable); }
        }

        public IEnumerable<IMetricOperand> Constans
        {
            get { return _nodes.OfType<IMetricOperand>().Where(t => t.Operand == Operand.Constant); }
        }

        public IEnumerable<IMetricOperand> Operands
        {
            get { return _nodes.OfType<IMetricOperand>(); }
        }

        public IEnumerable<IMetricCall> Calls
        {
            get { return _nodes.OfType<IMetricCall>(); }
        }

        public IEnumerable<IMetricMockOption> FakeOptions
        {
            get { return _nodes.OfType<IMetricMockOption>(); }
        }

        #endregion


        #region Add To Snapshot

        public void AddVariable(ICSharpDeclaration variable, Metrics metrics)
        {
            if (Variables.Any(t => t.Node == variable))
            {
                Variables.Single(t => t.Node == variable)
                         .AddAim(metrics.Aim)
                         .AddVarType(metrics.VarType);
            }
            else
            {
                _nodes.Add(new MetricOperand(variable, metrics.Scope, metrics.Aim, metrics.VarType));
            }
        }

        public void AddOperand(ICSharpTreeNode operand, Metrics metrics)
        {
            if (Operands.Any(t => t.Node == operand))
            {
                Operands.Single(t => t.Node == operand)
                         .AddAim(metrics.Aim)
                         .AddVarType(metrics.VarType);
            }
            else
            {
                _nodes.Add(new MetricOperand(operand, metrics.Scope, metrics.Aim, metrics.VarType));
            }
        }

        public void AddCall(IInvocationExpression invocation, Metrics metrics)
        {
            if (Calls.All(t => t.Node != invocation))
            {
                _nodes.Add(new MetricCall(invocation, metrics.Scope, metrics.Call));
            }
        }

        public void AddCall(ITypeofExpression invocation, Metrics metrics)
        {

            if (Calls.All(t => t.Node != invocation))
            {
                _nodes.Add(new MetricCall(invocation, metrics.Scope, metrics.Call));
            }
        }

        public void AddFakeOption(ICSharpExpression option, FakeOption fakeOption)
        {

        }

        public void AddLabel(ILabelStatement labelStatement)
        {

        }

        #endregion

        
        #region Helpers

        public IEnumerable<string> TestedProjectNames { get; private set; }

        public string TestProjectName { get; private set; }

        public bool IsInTestScope(string projectName)
        {
            return TestedProjectNames.Contains(projectName);
        }

        public bool IsInTestProject(string projectName)
        {
            return TestProjectName.Contains(projectName);
        }
        
        private void GetTestScope(IMethodDeclaration unitTest)
        {
            var module = unitTest.GetPsiModule();
            var project = module.ContainingProjectModule as ProjectImpl;
            TestProjectName = project.Name;
            TestedProjectNames = project.GetProjectReferences().Select(t => t.Name);
        }

        public Metrics GetVarMetrics([NotNull] IVariableDeclaration localVariable)
        {
            if (localVariable == null)
                throw new ArgumentNullException("localVariable");

            if (Variables.Where(t => t.Node == localVariable).IsSingle())
            {
                var node = Variables.Single(t => Equals(t.Node, localVariable));
                return Metrics.Create(node.Scope, node.VarTypes.Values.Max(), node.Aims.Values.Max());
            }

            throw new OperandNotFoundInSnapshotException(this, localVariable);
        }

        public Metrics GetVarMetrics([NotNull] IParameter paramter)
        {
            if (paramter == null)
                throw new ArgumentNullException("paramter");

            if (Operands.Where(t => Equals(t.Node, paramter)).IsSingle())
            {
                var node = Operands.Single(t => t.Node == paramter);
                return Metrics.Create(node.Scope, node.VarTypes.Values.Max(), node.Aims.Values.Max());
            }

            throw new OperandNotFoundInSnapshotException(this, paramter as ICSharpTreeNode);
        }

        #endregion
    }
}
