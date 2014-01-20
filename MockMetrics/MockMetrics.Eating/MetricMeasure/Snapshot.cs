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
        public List<IMetricVariable> Variables { get; protected set; }
        public List<IMetricMockOption> FakeOptions { get; protected set; }
        public IEnumerable<string> TestedProjectNames { get; private set; }
        public string TestProjectName { get; private set; }

        public IMethodDeclaration UnitTest { get; private set; }

        public Snapshot([NotNull] IMethodDeclaration unitTest)
        {
            if (unitTest == null) throw new ArgumentNullException("unitTest");
            UnitTest = unitTest;
            Variables = new List<IMetricVariable>();
            FakeOptions = new List<IMetricMockOption>();
            GetTestScope(UnitTest);
        }

        #region Add

        public void AddVariable([NotNull] ICSharpDeclaration varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType);
        }

        public void AddVariable(IInvocationExpression varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType);
        }

        public void AddVariable(ITypeUsage varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType);
        }

        public void AddVariable(IInitializerElement varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType);
        }

        public void AddVariable(ICSharpLiteralExpression varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType);
        }

        public void AddVariable(IObjectCreationExpression varDeclaration, Variable varType)
        {
            Add(varDeclaration, varType); ;
        }

        public void AddVariable(IReferenceExpression referenceExpression, Variable varType)
        {
            // TODO : doubling with EatExpressionHelper
            if (referenceExpression.Reference.CurrentResolveResult != null)
            {
                var declaration =
                    referenceExpression.Reference.CurrentResolveResult.DeclaredElement as ICSharpDeclaration;

                if (declaration != null)
                {
                    AddVariable(declaration, varType);
                    return;
                }
            }


            Add(referenceExpression, varType);
        }

        private void Add([NotNull] ICSharpTreeNode varDeclaration, Variable varType)
        {
            if (varDeclaration == null) throw new ArgumentNullException("varDeclaration");

            if (Variables.Any(t => t.NodeEquals(varDeclaration)))
            {
                Variables.Single(t => t.NodeEquals(varDeclaration))
                    .AddVarType(varType);
            }
            else
            {
                Variables.Add(new MetricVariable(varDeclaration, varType));
            }
        }

        public void AddFakeOption(ICSharpExpression option, FakeOption fakeOption)
        {
            if (option == null) throw new ArgumentNullException("option");

            FakeOptions.Add(new MetricMockOption(option, fakeOption));
        }

        #endregion


        #region Statistics

        public IList<ICSharpTreeNode> VariableNodes
        {
            get { return Variables.Select(t => t.Node).ToList(); }
        }

        public IList<IMetricVariable> Targets
        {
            get
            {
                return Variables.Where(t => t.GetVarType().Equals(Variable.Target)).ToList();
            }
        }

        public IList<IMetricVariable> Stubs
        {
            get
            {
                return Variables.Where(t => t.GetVarType().Equals(Variable.Stub)).ToList();
            }
        }

        public IList<IMetricVariable> Mocks
        {
            get
            {
                return Variables.Where(t => t.GetVarType().Equals(Variable.Mock)).ToList();
            }
        }

        public IList<IMetricVariable> Librarians
        {
            get
            {
                return Variables.Where(t => t.GetVarType().Equals(Variable.Library)).ToList();
            }
        }

        public IList<IMetricVariable> Services
        {
            get
            {
                return Variables.Where(t => t.GetVarType().Equals(Variable.Service)).ToList();
            }
        }

        public IList<IMetricMockOption> FakeMethods 
        {
            get { return FakeOptions.Where(t => t.FakeOption == FakeOption.Method).ToList(); }
        }

        public IList<IMetricMockOption> FakeProperties
        {
            get { return FakeOptions.Where(t => t.FakeOption == FakeOption.Property).ToList(); }
        }

        public IList<IMetricMockOption> FakeCallbacks
        {
            get { return FakeOptions.Where(t => t.FakeOption == FakeOption.CallBack).ToList(); }
        }

        public IList<IMetricMockOption> FakeExceptions
        {
            get { return FakeOptions.Where(t => t.FakeOption == FakeOption.Exception).ToList(); }
        }

        #endregion


        #region Service

        public bool IsInTestScope(string projectName)
        {
            return TestedProjectNames.Contains(projectName);
        }

        public bool IsInTestProject(string projectName)
        {
            return TestProjectName.Contains(projectName);
        }

        private void GetTestScope([NotNull] IMethodDeclaration unitTest)
        {
            if (unitTest == null) throw new ArgumentNullException("unitTest");
            var module = unitTest.GetPsiModule();
            var project = module.ContainingProjectModule as ProjectImpl;
            TestProjectName = project.Name;
            TestedProjectNames = project.GetProjectReferences().Select(t => t.Name);
        }

        public Variable GetVarMetrics([NotNull] IVariableDeclaration variable)
        {
            if (variable == null) throw new ArgumentNullException("variable");

            if (Variables.Where(t => t.Node == variable).IsSingle())
            {
                var node = Variables.Single(t => Equals(t.Node, variable));
                return node.GetVarType();
            }

            throw new OperandNotFoundInSnapshotException(this, variable);
        }

        public Variable GetVarMetrics([NotNull] IParameter paramter)
        {
            if (paramter == null) throw new ArgumentNullException("paramter");

            var paramDeclaration = paramter.GetDeclarations().Single();

            if (Variables.Where(t => Equals(t.Node, paramDeclaration)).IsSingle())
            {
                var node = Variables.Single(t => t.Node == paramDeclaration);
                return node.GetVarType();
            }

            throw new OperandNotFoundInSnapshotException(this, paramter as ICSharpTreeNode);
        }

        #endregion
    }
}
