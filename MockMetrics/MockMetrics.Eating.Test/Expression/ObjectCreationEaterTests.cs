using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Test.Expression
{
    [TestFixture]
    public class ObjectCreationExpressionEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater.Object, Mock.Of<EatExpressionHelper>());

            // Act
            objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression));
        }

        [Test]
        public void AddArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.Stub);
            var objectCreationEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>());

            // Act
            objectCreationEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, cSharpArgument));
        }

        [Test]
        public void AddStubCandidateArgumentToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var cSharpArgument = Mock.Of<ICSharpArgument>(t => t.Value == expression);
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new[] { cSharpArgument }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == ExpressionKind.StubCandidate);
            var objectCreationEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>());

            // Act
            objectCreationEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(It.IsAny<ExpressionKind>(), It.IsAny<ICSharpArgument>()), Times.Never);
        }

        [Test]
        public void EatValueTypeTest()
        {   
            // Arrange
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>());

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatTargetTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "Project");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType);
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("Project") == true);
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatHandyMockTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType);
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestProject("ProjectTest") == true);
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatMoqMockTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType
                                                        && t.GetCreationTypeName(objectCreationExpression) == "Moq.Mock OLOLO");
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatStubWhenNoOneOtherTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0]));
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType
                                                        && t.GetCreationTypeName(objectCreationExpression) == "OLOLO");
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}