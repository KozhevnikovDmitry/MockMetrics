//using System;
//using System.Collections.Generic;
//using System.Linq;
//using JetBrains.Annotations;
//using JetBrains.ProjectModel;
//using JetBrains.ReSharper.Psi.CSharp.Tree;
//using MockMetrics.Eating.Expression;
//using MockMetrics.Eating.MoqStub;
//using Delegate = JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2.Delegate;

//namespace MockMetrics.Eating
//{
//    public interface ISnapshot
//    {
//        IMethodDeclaration UnitTest { get; }
//        IEnumerable<ICSharpTreeNode> TargetCalls { get; }
//        IEnumerable<ICSharpTreeNode> Targets { get; }
//        IEnumerable<ICSharpTreeNode> Stubs { get; }
//        IEnumerable<ICSharpTreeNode> Results { get; }
//        IEnumerable<ICSharpTreeNode> Mocks { get; }
//        IEnumerable<ICSharpTreeNode> Asserts { get; }
//        IEnumerable<ICSharpTreeNode> Variables { get; }
//        IEnumerable<ICSharpTreeNode> Labels { get; }

//        void Add(ExpressionKind expressionKind, ICSharpStatement statement);
//        void Add(ExpressionKind expressionKind, ICSharpExpression expression);
//        void Add(FakeOptionType fakeOptionType, ICSharpExpression expression);
//        void Add(ExpressionKind expressionKind, IVariableDeclaration declaration);
//        void Add(ExpressionKind expressionKind, IInitializerElement variableInitializer);
//        void Add(ExpressionKind expressionKind, ICSharpArgument argument);
//        void Add(ILabelStatement label);
//        void Add(IVariableDeclaration variableDeclaration);
//        void Add(IQueryRangeVariableDeclaration variableDeclaration);

//        bool IsInTestScope(string projectName);
//        bool IsInTestProject(string projectName);
//        ExpressionKind GetVariableKind(IVariableDeclaration localVariable);
//        void Except(IVariableDeclaration variableDeclaration);
//    }

//    public class Snapshot : ISnapshot
//    {
//        public IEnumerable<string> TestedProjectNames { get; private set; }
//        public string TestProjectName { get; private set; }

//        private List<SnapNode> SnapNodes;

//        public Snapshot([NotNull] IMethodDeclaration unitTest)
//        {
//            if (unitTest == null)
//                throw new ArgumentNullException("unitTest");
//            SnapNodes = new List<SnapNode>();
//            UnitTest = unitTest;
//            GetTestScope(unitTest);
//        }

//        public IMethodDeclaration UnitTest { get; private set; }

//        public IEnumerable<ICSharpTreeNode> TargetCalls
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.TargetCall)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Targets
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.Target)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Stubs
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.Stub)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Results
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.Result)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Mocks
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.Mock)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Asserts
//        {
//            get
//            {
//                return SnapNodes.Where(t => t.Kinds.Contains(ExpressionKind.Assert)).Select(t => t.Node);
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Variables
//        {
//            get
//            {
//                return SnapNodes.Select(t => t.Node).OfType<ICSharpDeclaration>();
//            }
//        }

//        public IEnumerable<ICSharpTreeNode> Labels
//        {
//            get
//            {
//                return SnapNodes.Select(t => t.Node).OfType<ILabelStatement>();
//            }
//        }

//        public void Add(ExpressionKind expressionKind, ICSharpStatement statement)
//        {
//            AddAny(expressionKind, statement);
//        }

//        public void Add(ExpressionKind expressionKind, ICSharpExpression expression)
//        {
//            AddAny(expressionKind, expression);
//        }

//        public void Add(FakeOptionType fakeOptionType, ICSharpExpression expression)
//        {
//            switch (fakeOptionType)
//            {
//                case FakeOptionType.Property:
//                    {
//                        break;
//                    }
//                case FakeOptionType.Method:
//                    {
//                        break;
//                    }
//                case FakeOptionType.Event:
//                    {
//                        break;
//                    }
//                case FakeOptionType.CallBack:
//                    {
//                        break;
//                    }
//            }
//        }

//        public void Add(ExpressionKind expressionKind, IVariableDeclaration declaration)
//        {
//            AddAny(expressionKind, declaration);
//        }

//        public void Add(ExpressionKind expressionKind, IInitializerElement variableInitializer)
//        {
//            AddAny(expressionKind, variableInitializer);
//        }

//        public void Add(ExpressionKind expressionKind, ICSharpArgument argument)
//        {
//            AddAny(expressionKind, argument);
//        }

//        private void AddAny(ExpressionKind expressionKind, ICSharpTreeNode sharpTreeNode)
//        {
//            if (expressionKind == ExpressionKind.None || expressionKind == ExpressionKind.StubCandidate)
//            {
//                return;
//            }

//            var node = SnapNodes.SingleOrDefault(t => t.Node == sharpTreeNode);
//            if (node == null)
//            {
//                SnapNodes.Add(new SnapNode(sharpTreeNode, expressionKind));
//            }
//            else
//            {
//                node.AddKind(expressionKind);
//            }
//        }

//        public void Add(IVariableDeclaration variableDeclaration)
//        {
//            SnapNodes.Add(new SnapNode(variableDeclaration));
//        }

//        public void Add(IQueryRangeVariableDeclaration variableDeclaration)
//        {
//            SnapNodes.Add(new SnapNode(variableDeclaration));
//        }

//        public void Add(ILabelStatement label)
//        {
//            SnapNodes.Add(new SnapNode(label));
//        }

//        public bool IsInTestScope(string projectName)
//        {
//            return TestedProjectNames.Contains(projectName);
//        }

//        public bool IsInTestProject(string projectName)
//        {
//            return TestProjectName.Contains(projectName);
//        }

//        // TODO: get kind of fields, properties and methods
//        public ExpressionKind GetVariableKind([NotNull] IVariableDeclaration localVariable)
//        {
//            if (localVariable == null)
//                throw new ArgumentNullException("localVariable");

//            var variable = SnapNodes.SingleOrDefault(t => t.Node == localVariable);

//            if (variable == null) 
//                throw new VariableNotFoundInSnapshotException(localVariable, this);

//            var lastKind = variable.Kinds.Last();
//            return lastKind;
//        }

//        public void Except(IVariableDeclaration variableDeclaration)
//        {
//            var variable = SnapNodes.SingleOrDefault(t => t.Node == variableDeclaration);

//            if (variable == null) 
//                throw new VariableNotFoundInSnapshotException(variableDeclaration, this);

//            variable.ExceptInitialOccurence();
//        }

//        private void GetTestScope(IMethodDeclaration unitTest)
//        {
//            var module = unitTest.GetPsiModule();
//            var project = module.ContainingProjectModule as ProjectImpl;
//            TestProjectName = project.Name;
//            TestedProjectNames = project.GetProjectReferences().Select(t => t.Name);
//        }

//        public override string ToString()
//        {
//            return
//                string.Format(
//                    "Test [{0}];Stubs [{1}];Variables [{2}];Mocks [{3}];Targets [{4}];TargetCalls [{5}];Asserts [{6}];Labels [{7}];",
//                    UnitTest.NameIdentifier, Stubs.Count(), Variables.Count(), Mocks.Count(), Targets.Count(), TargetCalls.Count(), Asserts.Count(), Labels.Count());
//        }
//    }

//    internal class SnapNode
//    {
//        public ICSharpTreeNode Node { get; private set; }

//        public Dictionary<Guid, ExpressionKind> Occurences { get; private set; }

//        public IEnumerable<ExpressionKind> Kinds
//        {
//            get { return Occurences.Values; }
//        }

//        public Guid InitialOccurence { get; private set; }

//        public SnapNode([NotNull] ICSharpTreeNode node)
//        {
//            if (node == null)
//                throw new ArgumentNullException("node");

//            Node = node;
//            Occurences = new Dictionary<Guid, ExpressionKind>();
//            InitialOccurence = Guid.Empty;
//        }

//        public SnapNode(ICSharpTreeNode node, ExpressionKind kind)
//            : this(node)
//        {
//            AddKind(kind);
//        }

//        public void AddKind(ExpressionKind kind)
//        {
//            Occurences[Guid.NewGuid()] = kind;

//            if (Occurences.Count == 1)
//            {
//                InitialOccurence = Occurences.Single().Key;
//            }

//        }

//        public void ExceptInitialOccurence()
//        {
//            if (InitialOccurence != Guid.Empty &&
//                Occurences.ContainsKey(InitialOccurence))
//            {
//                Occurences.Remove(InitialOccurence);
//            }
//        }
//    }

//    public class VariableNotFoundInSnapshotException : ApplicationException
//    {
//        public IVariableDeclaration LocalVariable { get; private set; }
//        public Snapshot Snapshot { get; private set; }

//        public VariableNotFoundInSnapshotException(IVariableDeclaration localVariable, Snapshot snapshot)
//            :base("Variable is not found in snapshot")
//        {
//            LocalVariable = localVariable;
//            Snapshot = snapshot;
//        }
//    }
//}