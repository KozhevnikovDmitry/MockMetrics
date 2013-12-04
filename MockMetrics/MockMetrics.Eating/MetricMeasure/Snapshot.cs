using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;

namespace MockMetrics.Eating.MetricMeasure
{
    public class Snapshot : ISnapshot
    {
        private readonly EatExpressionHelper _expressionHepler;
        private readonly List<IMetricNode> _nodes = new List<IMetricNode>();

        public IMethodDeclaration UnitTest { get; private set; }

        public Snapshot([NotNull] IMethodDeclaration unitTest, EatExpressionHelper expressionHepler)
        {
            _expressionHepler = expressionHepler;
            if (unitTest == null) 
                throw new ArgumentNullException("unitTest");

            UnitTest = unitTest;
            GetTestScope(UnitTest);
        }

        #region Old Metrics

        public IEnumerable<IMetricCall> TargetCalls
        {
            get
            {
                return Calls.Where(t => t.Call == Call.TargetCall);
            }
        }

        public IEnumerable<IMetricCall> Asserts
        {
            get
            {
                return Calls.Where(t => t.Call == Call.Assert);
            }
        }

        public IEnumerable<IMetricNode> Targets
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(Variable.Target));
            }
        }

        public IEnumerable<IMetricNode> Stubs
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(Variable.Mock));
            }
        }

        public IEnumerable<IMetricNode> Results
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(Variable.Result));
            }
        }

        public IEnumerable<IMetricNode> Mocks
        {
            get
            {
                return Operands.Where(t => t.VarTypes.Values.Max().Equals(Variable.Mock));
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

        public void AddVariable([NotNull] ICSharpDeclaration variable, [NotNull] Metrics metrics)
        {
            if (variable == null) 
                throw new ArgumentNullException("variable");

            if (metrics == null) 
                throw new ArgumentNullException("metrics");

            if (Variables.Any(t => t.NodeEquals(variable)))
            {
                Variables.Single(t => t.NodeEquals(variable))
                         .AddVarType(metrics.Variable);
            }
            else
            {
                _nodes.Add(new MetricOperand(variable, metrics.Scope, metrics.Variable, GetOperandType(variable)));
            }
        }

        public void AddOperand([NotNull] IInvocationExpression operand, [NotNull] Metrics metrics)
        {
            if (operand == null) throw new ArgumentNullException("operand");
            if (metrics == null) throw new ArgumentNullException("metrics");

            if (Operands.Any(t => t.NodeEquals(operand)))
            {
                Operands.Single(t => t.NodeEquals(operand))
                        .AddVarType(metrics.Variable);
            }
            else
            {
                _nodes.Add(new MetricOperand(operand, metrics.Scope, metrics.Variable, GetOperandType(operand)));
            }
        }

        public void AddOperand([NotNull] IReferenceExpression operand, [NotNull] Metrics metrics)
        {
            if (operand == null) throw new ArgumentNullException("operand");
            if (metrics == null) throw new ArgumentNullException("metrics");
            var node = _expressionHepler.GetReferenceElement(operand);

            if (node is IVariableDeclaration)
            {
                
            }

            if (node is IMethod)
            {
                
            }
        }

        public void AddOperand([NotNull] ITypeUsage operand, [NotNull] Metrics metrics)
        {
            if (operand == null) throw new ArgumentNullException("operand");
            if (metrics == null) throw new ArgumentNullException("metrics");
            if (Operands.Any(t => t.NodeEquals(operand)))
            {
                Operands.Single(t => t.NodeEquals(operand))
                         .AddVarType(metrics.Variable);
            }
            else
            {
                _nodes.Add(new MetricOperand(operand, metrics.Scope, metrics.Variable, GetOperandType(operand)));
            }
        }

        public void AddOperand([NotNull] IInitializerElement operand, [NotNull] Metrics metrics)
        {
            if (operand == null) throw new ArgumentNullException("operand");
            if (metrics == null) throw new ArgumentNullException("metrics");
            if (Operands.Any(t => t.NodeEquals(operand)))
            {
                Operands.Single(t => t.NodeEquals(operand))
                         .AddVarType(metrics.Variable);
            }
            else
            {
                _nodes.Add(new MetricOperand(operand, metrics.Scope, metrics.Variable, GetOperandType(operand)));
            }
        }

        public void AddOperand(ICSharpLiteralExpression operand, Metrics metrics)
        {
            if (operand == null) throw new ArgumentNullException("operand");
            if (metrics == null) throw new ArgumentNullException("metrics");
            if (Operands.Any(t => t.NodeEquals(operand)))
            {
                Operands.Single(t => t.NodeEquals(operand))
                         .AddVarType(metrics.Variable);
            }
            else
            {
                _nodes.Add(new MetricOperand(operand, metrics.Scope, metrics.Variable, GetOperandType(operand)));
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

        private IEnumerable<ICSharpDeclaration> GetDeclarations(ICSharpTreeNode operand)
        {
            if (operand is ICSharpDeclaration)
            {
                return new[] {operand as ICSharpDeclaration};
            }

            if (operand is IDeclaredElement)
            {
                return (operand as IDeclaredElement).GetDeclarations().OfType<ICSharpDeclaration>();
            }

            if (operand is IReferenceExpression)
            {
                return GetDeclarationsForReference(operand as IReferenceExpression);
            }
            throw new NotImplementedException();
        }

        private IEnumerable<IMetricOperand> GetOperands(ICSharpTreeNode operand)
        {
            if (operand is ICSharpDeclaration)
            {
                return _nodes.Where(t => t.Node == operand).OfType<IMetricOperand>();
            }

            if (operand is IDeclaredElement)
            {
               // return _nodes.Where(t => (operand as IDeclaredElement).GetDeclarations().OfType<ICSharpDeclaration>().Contains(t.Node)).OfType<IMetricOperand>();
            }

            if (operand is IReferenceExpression)
            {
                return GetOperandsForReference(operand as IReferenceExpression);
            }
            throw new NotImplementedException();
        }

        private IEnumerable<IMetricOperand> GetOperandsForReference(IReferenceExpression referenceExpression)
        {
            var declarations = GetDeclarationsForReference(referenceExpression);
            //return _nodes.Where(t => declarations.Contains(t.Node)).OfType<IMetricOperand>();

            throw new NotImplementedException();
        }

        private IEnumerable<ICSharpDeclaration> GetDeclarationsForReference(IReferenceExpression referenceExpression)
        {
            var declaredElement = referenceExpression.Reference.CurrentResolveResult.DeclaredElement;
            return declaredElement.GetDeclarations().OfType<ICSharpDeclaration>();
        }

        private Operand GetOperandType(ICSharpTreeNode operand)
        {

            if (operand is ILocalVariableDeclaration ||
                operand is ILambdaParameterDeclaration ||
                operand is IAnonymousMemberDeclaration ||
                operand is ICatchVariableDeclaration ||
                operand is IForeachVariableDeclaration ||
                operand is IUnsafeCodeFixedPointerDeclaration ||
                operand is IQueryRangeVariableDeclaration)
            {
                return Operand.Variable;
            }

            if (operand is ILocalConstantDeclaration ||
                operand is IConstantDeclaration ||
                operand is ITypeUsage)
            {
                return Operand.Constant;
            }

            if (operand is IRegularParameterDeclaration)
            {
                return Operand.Argument;
            }

            var node = GetDeclaredNode(operand);

            if (node is IProperty ||
                node is IField)
            {
                return Operand.Property;
            }

            if (node is IEvent)
            {
                return Operand.Event;
            }

            if (node is IMethod)
            {
                return Operand.Method;
            }

            if (node is ITypeElement)
            {
                return Operand.Type;
            }

            throw new UnexpectedOperandTypeException(this, operand);
        }

        private ICSharpTreeNode GetDeclaredNode(ICSharpTreeNode operand)
        {
            if (operand is ICSharpDeclaration)
            {
                return operand;
            }

            if (operand is IParameter)
            {
                return (operand as IParameter).GetDeclarations().Single() as ICSharpTreeNode;
            }

            if (operand is IReferenceExpression)
            {
                return
                    GetDeclaredNode(
                        (operand as IReferenceExpression).Reference.CurrentResolveResult.DeclaredElement as ICSharpTreeNode);
            }


            return operand;
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
                return Metrics.Create(node.Scope, node.VarTypes.Values.Max());
            }

            throw new OperandNotFoundInSnapshotException(this, localVariable);
        }

        public Metrics GetVarMetrics([NotNull] IParameter paramter)
        {
            if (paramter == null)
                throw new ArgumentNullException("paramter");

            var paramDeclaration = paramter.GetDeclarations().Single();

            if (Operands.Where(t => Equals(t.Node, paramDeclaration)).IsSingle())
            {
                var node = Operands.Single(t => t.Node == paramDeclaration);
                return Metrics.Create(node.Scope, node.VarTypes.Values.Max());
            }

            throw new OperandNotFoundInSnapshotException(this, paramter as ICSharpTreeNode);
        }

        #endregion
    }
}
