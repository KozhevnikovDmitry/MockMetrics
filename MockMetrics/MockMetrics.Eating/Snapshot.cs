using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MoqStub;

namespace MockMetrics.Eating
{
    public interface ISnapshot
    {
        IMethodDeclaration UnitTest { get; }
        List<ICSharpTreeNode> TargetCalls { get; }
        List<ICSharpTreeNode> Targets { get; }
        List<ICSharpTreeNode> Stubs { get; }
        List<ICSharpTreeNode> Results { get; }
        List<ICSharpTreeNode> Mocks { get; }
        List<ICSharpTreeNode> Asserts { get; }
        List<ICSharpDeclaration> Variables { get; }
        List<ILabelStatement> Labels { get; }
        void AddTreeNode(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode);
        void AddTreeNode(FakeOptionType fakeOptionType, ICSharpTreeNode sharpTreeNode);
        void AddVariable(ICSharpDeclaration variableDeclaration);
        void AddLabel(ILabelStatement label);
        bool IsInTestScope(string projectName);
        bool IsInTestProject(string projectName);
        ExpressionKind GetVariableKind(IVariableDeclaration localVariable, ITypeEater typeEater);
    }

    public class Snapshot : ISnapshot
    {
        public IEnumerable<string> TestedProjectNames { get; private set; }
        public string TestProjectName { get; private set; }

        public Snapshot([NotNull] IMethodDeclaration unitTest)
        {
            if (unitTest == null)
                throw new ArgumentNullException("unitTest");

            UnitTest = unitTest;
            Targets = new List<ICSharpTreeNode>();
            Stubs = new List<ICSharpTreeNode>();
            Results = new List<ICSharpTreeNode>();
            Mocks = new List<ICSharpTreeNode>();
            Asserts = new List<ICSharpTreeNode>();
            TargetCalls = new List<ICSharpTreeNode>();
            Variables = new List<ICSharpDeclaration>();
            Labels = new List<ILabelStatement>();
            GetTestScope(unitTest);
        }

        public IMethodDeclaration UnitTest { get; private set; }

        public List<ICSharpTreeNode> TargetCalls { get; private set; }

        public List<ICSharpTreeNode> Targets { get; private set; }

        public List<ICSharpTreeNode> Stubs { get; private set; }

        public List<ICSharpTreeNode> Results { get; private set; }

        public List<ICSharpTreeNode> Mocks { get; private set; }

        public List<ICSharpTreeNode> Asserts { get; private set; }

        public List<ICSharpDeclaration> Variables { get; private set; }

        public List<ILabelStatement> Labels { get; private set; }

        public void AddTreeNode(FakeOptionType fakeOptionType, ICSharpTreeNode sharpTreeNode)
        {
            switch (fakeOptionType)
            {
                case FakeOptionType.Property:
                    {
                        break;
                    }
                case FakeOptionType.Method:
                    {
                        break;
                    }
                case FakeOptionType.Event:
                    {
                        break;
                    }
                case FakeOptionType.CallBack:
                    {
                        break;
                    }
            }
        }

        public void AddTreeNode(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode)
        {
            switch (expressionKind)
            {
                case ExpressionKind.Stub:
                    {
                        Stubs.Add(sharpTreeNode);
                        break;
                    }
                case ExpressionKind.Result:
                    {
                        Results.Add(sharpTreeNode);
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
                case ExpressionKind.StubCandidate:
                    {
                        break;
                    }
                case ExpressionKind.None:
                    {
                        break;
                    }
            }
        }

        public void AddVariable(ICSharpDeclaration variableDeclaration)
        {
            Variables.Add(variableDeclaration);
        }

        public void AddLabel(ILabelStatement label)
        {
            Labels.Add(label);
        }

        public bool IsInTestScope(string projectName)
        {
            return TestedProjectNames.Contains(projectName);
        }

        public bool IsInTestProject(string projectName)
        {
            return TestProjectName.Contains(projectName);
        }

        // TODO: get kind of fields, properties and methods
        public ExpressionKind GetVariableKind([NotNull] IVariableDeclaration localVariable, [NotNull] ITypeEater typeEater)
        {
            if (localVariable == null)
                throw new ArgumentNullException("localVariable");

            if (typeEater == null)
                throw new ArgumentNullException("typeEater");

            if (Targets.OfType<IVariableDeclaration>().Contains(localVariable))
            {
                return ExpressionKind.Target;
            }

            if (Stubs.OfType<IVariableDeclaration>().Contains(localVariable))
            {
                return ExpressionKind.Stub;
            }

            if (Mocks.OfType<IVariableDeclaration>().Contains(localVariable))
            {
                return ExpressionKind.Mock;
            }

            if (Results.OfType<IVariableDeclaration>().Contains(localVariable))
            {
                return ExpressionKind.Result;
            }

            if (Variables.OfType<IVariableDeclaration>().Contains(localVariable))
            {
                return typeEater.EatVariableType(this, localVariable.Type);
            }

            throw new NotSupportedException();
        }

        private void GetTestScope(IMethodDeclaration unitTest)
        {
            var module = unitTest.GetPsiModule();
            var project = module.ContainingProjectModule as ProjectImpl;
            TestProjectName = project.Name;
            TestedProjectNames = project.GetProjectReferences().Select(t => t.Name);
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Test [{0}];Stubs [{1}];Variables [{2}];Mocks [{3}];Targets [{4}];TargetCalls [{5}];Asserts [{6}];Labels [{7}];",
                    UnitTest.NameIdentifier, Stubs.Count, Variables.Count, Mocks.Count, Targets.Count, TargetCalls.Count, Asserts.Count, Labels.Count);
        }
    }
}